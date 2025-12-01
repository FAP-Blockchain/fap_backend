using Fap.Domain.DTOs.Wallet;
using Nethereum.Web3.Accounts;

namespace Fap.Api.Interfaces
{
    /// <summary>
    /// Service for blockchain wallet management, key storage, and transaction signing
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// Generate a new Ethereum wallet with encrypted private key storage
        /// </summary>
        /// <returns>Wallet information</returns>
        Task<CreateWalletResult> GenerateWalletAsync();
        
        /// <summary>
        /// Get wallet info by address (does not expose private key)
        /// </summary>
        /// <param name="address">Wallet address</param>
        /// <returns>Wallet information or null if not found</returns>
        Task<WalletInfo?> GetWalletByAddressAsync(string address);
        
        /// <summary>
        /// Get or create wallet - if address is provided and exists, return it; otherwise generate new
        /// </summary>
        /// <param name="existingAddress">Optional existing wallet address</param>
        /// <param name="userId">Optional user ID to associate wallet with</param>
        /// <returns>Wallet information</returns>
        Task<CreateWalletResult> GetOrCreateWalletAsync(string? existingAddress, Guid? userId = null);
        
        /// <summary>
    /// Get Account object for signing transactions (decrypts private key)
    /// INTERNAL USE ONLY - never expose to API
        /// </summary>
        /// <param name="address">Wallet address</param>
        /// <returns>Nethereum Account object</returns>
        Task<Account> GetAccountForSigningAsync(string address);
        
        /// <summary>
        /// Validate if an Ethereum address is valid format
        /// </summary>
        /// <param name="address">Address to validate</param>
        /// <returns>True if valid Ethereum address</returns>
        bool IsValidAddress(string address);
        
        /// <summary>
        /// Check if wallet exists in database
        /// </summary>
        /// <param name="address">Wallet address</param>
        /// <returns>True if wallet exists</returns>
        Task<bool> WalletExistsAsync(string address);
        
        /// <summary>
        /// Associate wallet with a user
        /// </summary>
        /// <param name="address">Wallet address</param>
        /// <param name="userId">User ID</param>
        /// <returns>True if successful</returns>
        Task<bool> AssociateWalletWithUserAsync(string address, Guid userId);
        
        /// <summary>
        /// Update last used timestamp for wallet
        /// </summary>
        /// <param name="address">Wallet address</param>
        Task UpdateLastUsedAsync(string address);
    }
}
