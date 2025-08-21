using Marquito.CoinbasePro.Class.Client.Data.Common;
using MarquitoUtils.TradingAPI.Class.Enums;

namespace Marquito.CoinbasePro.Class.Exceptions
{
    public class CoinbaseProOrderException : Exception
    {
        public CoinbaseProOrderException(string productID, TradingSide side, ErrorResponse error) 
            : base($"An exception occurs when trying to convert crypto : [{error.ErrorCode}] {error.ErrorMessage} - details : {error.ErrorDetails}")
        {
            this.Data.Add("ProductID", productID);
            this.Data.Add(nameof(TradingSide), side);
        }
    }
}
