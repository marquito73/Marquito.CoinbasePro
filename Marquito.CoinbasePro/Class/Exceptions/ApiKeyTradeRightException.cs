using Marquito.CoinbasePro.Class.Client.Data.Account;
using Marquito.CoinbasePro.Class.Client.Data.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquito.CoinbasePro.Class.Exceptions
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
