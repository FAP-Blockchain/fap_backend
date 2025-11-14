using System.Security.Claims;

namespace Fap.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Try to extract Guid user ID from NameIdentifier claim.
        /// </summary>
        public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId)
        {
            userId = Guid.Empty;

            var claimValue = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(claimValue))
            {
                return false;
            }

            return Guid.TryParse(claimValue, out userId);
        }

        /// <summary>
        /// Get user ID from claims or throw InvalidOperationException.
        /// </summary>
        public static Guid GetRequiredUserId(this ClaimsPrincipal user)
        {
            if (user.TryGetUserId(out var guid))
            {
                return guid;
            }

            throw new InvalidOperationException("Unable to resolve user ID from authentication claims.");
        }
    }
}


