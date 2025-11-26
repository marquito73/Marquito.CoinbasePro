using Marquito.CoinbasePro.Client.Data.Permissions;

namespace Marquito.CoinbasePro.Exceptions
{
    public class ApiKeyTradeRightException : Exception
    {
        public ApiKeyTradeRightException(AccountPermissions accountPermissions)
            : base($"API key doesn't have rights to trade")
        {
            this.Data.Add(nameof(AccountPermissions), accountPermissions);
        }
    }
}
