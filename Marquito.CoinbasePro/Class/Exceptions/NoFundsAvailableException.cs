using Marquito.CoinbasePro.Class.Client.Data.Account;
using Marquito.CoinbasePro.Class.Enums;

namespace Marquito.CoinbasePro.Class.Exceptions
{
    /// <summary>
    /// Represents an error happen when an account don't have enough funds to convert an amount of crypto.
    /// </summary>
    public class NoFundsAvailableException : Exception
    {
        /// <summary>
        /// Represents an error happen when an account don't have enough funds to convert an amount of crypto.
        /// </summary>
        /// <param name="account">The account for the funds</param>
        /// <param name="productID">The product ID</param>
        /// <param name="side">Buy or sell ?</param>
        public NoFundsAvailableException(Account account, string productID, TradingSide side) 
            : base($"Not enough funds available on the account {account.Currency} for the product {productID} with {side.ToString()} side.")
        {
            this.Data.Add("AccountID", account.AccountID);
            this.Data.Add("ProductID", productID);
            this.Data.Add(nameof(TradingSide), side);
        }
    }
}
