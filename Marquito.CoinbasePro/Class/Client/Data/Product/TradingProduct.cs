using Newtonsoft.Json;
using System.Diagnostics;

namespace Marquito.CoinbasePro.Class.Client.Data.Product
{
    [DebuggerDisplay("Product ID = {ProductID}")]
    public class TradingProduct
    {
        [JsonProperty("product_id")]
        public string ProductID { get; set; }
        [JsonProperty("base_currency_id")]
        public string CryptoID { get; set; }
        [JsonProperty("base_name")]
        public string CryptoName { get; set; }
        [JsonProperty("quote_currency_id")]
        public string CurrencyID { get; set; }
        [JsonProperty("quote_name")]
        public string CurrencyName { get; set; }
    }
}
