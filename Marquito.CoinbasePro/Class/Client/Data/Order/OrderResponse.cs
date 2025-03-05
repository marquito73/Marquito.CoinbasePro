using Newtonsoft.Json;

namespace Marquito.CoinbasePro.Class.Client.Data.Order
{
    public class OrderResponse
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }
        [JsonProperty("success_response")]
        public Order Order { get; set; }
    }
}
