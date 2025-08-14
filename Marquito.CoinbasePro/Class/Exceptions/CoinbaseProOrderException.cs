using MarquitoUtils.TradingAPI.Class.Enums;

namespace Marquito.CoinbasePro.Class.Exceptions
{
    public class CoinbaseProOrderException : Exception
    {
        public CoinbaseProOrderException(string productID, TradingSide side) 
            : base("An exception occurs when trying to convert crypto")
        {
            this.Data.Add("ProductID", productID);
            this.Data.Add(nameof(TradingSide), side);
        }
    }
}
