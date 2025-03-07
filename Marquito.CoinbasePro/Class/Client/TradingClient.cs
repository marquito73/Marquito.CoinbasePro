using Flurl;
using Marquito.CoinbasePro.Class.Client.Data.Account;
using Marquito.CoinbasePro.Class.Client.Data.Order;
using Marquito.CoinbasePro.Class.Client.Data.Permissions;
using Marquito.CoinbasePro.Class.Client.Data.Product;
using Marquito.CoinbasePro.Class.Enums;
using Marquito.CoinbasePro.Class.Exceptions;

namespace Marquito.CoinbasePro.Class.Client
{
    /// <summary>
    /// Common trading client
    /// </summary>
    public abstract class TradingClient
    {
        /// <summary>
        /// An URL for sandbox use only
        /// </summary>
        protected string SandboxEndpoint { get; set; }
        /// <summary>
        /// An URL for real use
        /// </summary>
        protected string Endpoint { get; set; }
        /// <summary>
        /// Use sandbox mode ?
        /// </summary>
        protected bool UseSandbox { get; set; } = false;

        #region Commons

        /// <summary>
        /// Get main url with the correct Endpoint
        /// </summary>
        /// <returns>The main url with the correct Endpoint</returns>
        protected Url GetMainUrl()
        {
            return new Url($"https://{this.GetMainEndpoint()}");
        }

        /// <summary>
        /// Get the main endpoint
        /// </summary>
        /// <returns>The main endpoint</returns>
        protected string GetMainEndpoint()
        {
            if (this.UseSandbox)
            {
                return this.SandboxEndpoint;
            }
            else
            {
                return this.Endpoint;
            }
        }

        #endregion Commons

        #region Accounts

        /// <summary>
        /// Get accounts
        /// </summary>
        /// <returns>The list of accounts available</returns>
        public abstract List<Account> GetAccounts();

        /// <summary>
        /// Get an account by it's ID
        /// </summary>
        /// <param name="accountID">The account's ID</param>
        /// <returns>An account by it's ID</returns>
        public abstract Account GetAccount(Guid accountID);

        public abstract Account? GetAccount(string productCurrency);

        #endregion Accounts

        #region Products

        /// <summary>
        /// Get list of products
        /// </summary>
        /// <returns>Products</returns>
        public abstract List<TradingProduct> GetProducts();

        /// <summary>
        /// Get candle data (historical data) for a specific product
        /// </summary>
        /// <param name="product">The product</param>
        /// <param name="start">The start date</param>
        /// <param name="end">The end date</param>
        /// <param name="period">The period</param>
        /// <returns>Candle data (historical data) for a specific product</returns>
        public abstract List<Candle> GetCandles(string product, DateTime start, DateTime end, TradingPeriod period);

        /// <summary>
        /// Get candle data (historical data) for a specific product
        /// </summary>
        /// <param name="product">The product</param>
        /// <param name="period">The period</param>
        /// <param name="candleCount">The amount of candle (max 300 candles per request)</param>
        /// <returns>Candle data (historical data) for a specific product</returns>
        public abstract List<Candle> GetCandles(string product, TradingPeriod period, short candleCount = 300);

        /// <summary>
        /// Get last market trade price for a specific product
        /// </summary>
        /// <param name="product">The product</param>
        /// <returns>The last market trade price for a specific product</returns>
        public abstract MarketTrade GetLastMarketTrade(string product);

        #endregion Products

        #region Orders

        /// <summary>
        /// Buy or sell crypto between two currencies of a product
        /// </summary>
        /// <param name="fromAccount">The first part of the product</param>
        /// <param name="targetAccount">The second part of the product</param>
        /// <param name="amountToConvert">The amount to convert between the pair</param>
        /// <param name="side">The side : we want to buy or sell ?</param>
        /// <returns></returns>
        /// <exception cref="NoFundsAvailableException">An account don't have enough funds to convert an amount of crypto.</exception>
        public abstract Order ConvertCrypto(Account fromAccount, Account targetAccount, double amountToConvert, TradingSide side);

        /// <summary>
        /// Buy or sell crypto between two currencies of a product
        /// </summary>
        /// <param name="productID">The product pair</param>
        /// <param name="amountToConvert">The amount to convert between the pair</param>
        /// <param name="side">The side : we want to buy or sell ?</param>
        /// <returns></returns>
        /// <exception cref="NoFundsAvailableException">An account don't have enough funds to convert an amount of crypto.</exception>
        public abstract Order ConvertCrypto(string productID, double amountToConvert, TradingSide side);

        public abstract Order ConvertAllCrypto(Account fromAccount, Account targetAccount, TradingSide side);

        public abstract Order ConvertAllCrypto(string productID, TradingSide side);

        #endregion Orders

        #region Permissions

        /// <summary>
        /// Get permissions for your API key
        /// </summary>
        /// <returns>Permissions for your API key</returns>
        public abstract AccountPermissions GetAccountPermissions();

        #endregion Permissions
    }
}
