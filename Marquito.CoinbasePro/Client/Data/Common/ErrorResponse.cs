using Newtonsoft.Json;

namespace Marquito.CoinbasePro.Client.Data.Common
{
    /// <summary>
    /// Error response from the Coinbase Pro API
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Error code indicating the type of error that occurred
        /// </summary>
        [JsonProperty("error")]
        public string ErrorCode { get; set; }
        /// <summary>
        /// Gets the error message associated with the current operation
        /// </summary>
        [JsonProperty("message")]
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets the details of the error that occurred
        /// </summary>
        [JsonProperty("error_details")]
        public string ErrorDetails { get; set; }
    }
}
