using Newtonsoft.Json;

namespace Marquito.CoinbasePro.Class.Client.Data.Product
{
    public class TradingProductResponse
    {
        [JsonProperty("products")]
        public List<TradingProduct> Products { get; set; }
    }
}
