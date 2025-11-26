using Marquito.CoinbasePro.Client.Data.Common;
using MarquitoUtils.TradingAPI.Enums;

namespace Marquito.CoinbasePro.Exceptions
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
