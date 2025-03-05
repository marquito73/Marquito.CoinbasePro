using Marquito.CoinbasePro.Class.Enums;

namespace Marquito.CoinbasePro.Class.Entities.File
{
    /// <summary>
    /// The trading configuration
    /// </summary>
    public class TradingConfiguration
    {
        /// <summary>
        /// The secret key
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// The organization name
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// The api key
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// The default product
        /// </summary>
        public string DefaultProduct { get; set; }
        /// <summary>
        /// The defalt period
        /// </summary>
        public TradingPeriod DefaultPeriod { get; set; }
    }
}
