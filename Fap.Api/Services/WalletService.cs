using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Wallet;
using Fap.Domain.Repositories;
using NBitcoin;  // For Mnemonic, Wordlist, WordCount
using Nethereum.HdWallet;
using Nethereum.Web3.Accounts;
using System.Text.RegularExpressions;
using WalletEntity = Fap.Domain.Entities.Wallet;  // Alias to avoid conflict

namespace Fap.Api.Services
{
    /// <summary>
    /// Full-featured wallet service with encryption, key management, and transaction signing
    /// </summary>
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger<WalletService> _logger;

        // Ethereum address regex pattern
        private static readonly Regex EthAddressRegex = new(@"^0x[a-fA-F0-9]{40}$", RegexOptions.Compiled);

        public WalletService(
            IUnitOfWork uow,
            IEncryptionService encryptionService,
            ILogger<WalletService> logger)
        {
            _uow = uow;
            _encryptionService = encryptionService;
            _logger = logger;
        }

        public async Task<CreateWalletResult> GenerateWalletAsync()
        {
            var result = new CreateWalletResult();

            try
            {
                _logger.LogInformation("Generating new Ethereum wallet...");

                // 1. Generate HD Wallet with secure mnemonic
                var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
                var hdWallet = new Wallet(mnemonic.ToString(), string.Empty);

                // 2. Get first account from HD wallet (derivation path: m/44'/60'/0'/0/0)
                var account = hdWallet.GetAccount(0);

                _logger.LogInformation($"Generated wallet address: {account.Address}");

                // 3. Encrypt private key before storage
                var encryptedPrivateKey = await _encryptionService.EncryptAsync(account.PrivateKey);

                // 4. Create Wallet entity
                var wallet = new WalletEntity
                {
                    Id = Guid.NewGuid(),
                    Address = account.Address,
                    EncryptedPrivateKey = encryptedPrivateKey,
                    PublicKey = account.PublicKey,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                // 5. Save to database
                await _uow.Wallets.AddAsync(wallet);
                await _uow.SaveChangesAsync();

                _logger.LogInformation($"Wallet saved to database: {wallet.Address}");

                result.Success = true;
                result.Message = "Wallet generated successfully";
                result.Wallet = new WalletInfo
                {
                    Address = wallet.Address,
                    PublicKey = wallet.PublicKey,
                    IsNewWallet = true,
                    CreatedAt = wallet.CreatedAt
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate wallet");
                result.Success = false;
                result.Message = "Failed to generate wallet";
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<WalletInfo?> GetWalletByAddressAsync(string address)
        {
            try
            {
                if (!IsValidAddress(address))
                {
                    _logger.LogWarning($"Invalid wallet address format: {address}");
                    return null;
                }

                var wallet = await _uow.Wallets.GetByAddressAsync(address);

                if (wallet == null)
                {
                    _logger.LogWarning($"Wallet not found: {address}");
                    return null;
                }

                return new WalletInfo
                {
                    Address = wallet.Address,
                    PublicKey = wallet.PublicKey,
                    IsNewWallet = false,
                    CreatedAt = wallet.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving wallet: {address}");
                return null;
            }
        }

        public async Task<CreateWalletResult> GetOrCreateWalletAsync(string? existingAddress, Guid? userId = null)
        {
            var result = new CreateWalletResult();

            try
            {
                // Case 1: Existing address provided
                if (!string.IsNullOrEmpty(existingAddress))
                {
                    if (!IsValidAddress(existingAddress))
                    {
                        result.Success = false;
                        result.Message = "Invalid Ethereum address format";
                        result.Errors.Add($"Address must match pattern: 0x followed by 40 hex characters");
                        return result;
                    }

                    var existingWallet = await GetWalletByAddressAsync(existingAddress);

                    if (existingWallet != null)
                    {
                        _logger.LogInformation($"Using existing wallet: {existingAddress}");
                        result.Success = true;
                        result.Message = "Using existing wallet";
                        result.Wallet = existingWallet;
                        
                        // Associate with user if provided
                        if (userId.HasValue)
                        {
                            await AssociateWalletWithUserAsync(existingAddress, userId.Value);
                        }
                        
                        return result;
                    }

                    // Wallet address provided but not in our database
                    // This is valid - user may have external wallet
                    _logger.LogInformation($"External wallet address provided: {existingAddress}");
                    result.Success = true;
                    result.Message = "Using external wallet address";
                    result.Wallet = new WalletInfo
                    {
                        Address = existingAddress,
                        PublicKey = string.Empty, // Unknown for external wallets
                        IsNewWallet = false,
                        CreatedAt = DateTime.UtcNow
                    };
                    return result;
                }

                // Case 2: No address provided - generate new wallet
                _logger.LogInformation("No wallet provided, generating new one...");
                result = await GenerateWalletAsync();

                // Associate with user if provided
                if (result.Success && userId.HasValue && result.Wallet != null)
                {
                    await AssociateWalletWithUserAsync(result.Wallet.Address, userId.Value);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrCreateWallet");
                result.Success = false;
                result.Message = "Failed to get or create wallet";
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public async Task<Account> GetAccountForSigningAsync(string address)
        {
            try
            {
                if (!IsValidAddress(address))
                    throw new ArgumentException("Invalid Ethereum address format", nameof(address));

                var wallet = await _uow.Wallets.GetByAddressAsync(address);

                if (wallet == null)
                    throw new InvalidOperationException($"Wallet not found: {address}");

                // Decrypt private key
                var privateKey = await _encryptionService.DecryptAsync(wallet.EncryptedPrivateKey);

                // Create Account for signing
                var account = new Account(privateKey);

                // Update last used timestamp
                await UpdateLastUsedAsync(address);

                _logger.LogInformation($"Account retrieved for signing: {address}");

                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get account for signing: {address}");
                throw;
            }
        }

        public bool IsValidAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;

            return EthAddressRegex.IsMatch(address);
        }

        public async Task<bool> WalletExistsAsync(string address)
        {
            try
            {
                if (!IsValidAddress(address))
                    return false;

                return await _uow.Wallets.AddressExistsAsync(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking wallet existence: {address}");
                return false;
            }
        }

        public async Task<bool> AssociateWalletWithUserAsync(string address, Guid userId)
        {
            try
            {
                var wallet = await _uow.Wallets.GetByAddressAsync(address);

                if (wallet == null)
                {
                    _logger.LogWarning($"Cannot associate wallet - not found: {address}");
                    return false;
                }

                wallet.UserId = userId;
                _uow.Wallets.Update(wallet);
                await _uow.SaveChangesAsync();

                _logger.LogInformation($"Wallet {address} associated with user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to associate wallet with user");
                return false;
            }
        }

        public async Task UpdateLastUsedAsync(string address)
        {
            try
            {
                var wallet = await _uow.Wallets.GetByAddressAsync(address);

                if (wallet != null)
                {
                    wallet.LastUsedAt = DateTime.UtcNow;
                    _uow.Wallets.Update(wallet);
                    await _uow.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to update last used timestamp for {address}");
                // Non-critical error, don't throw
            }
        }
    }
}
