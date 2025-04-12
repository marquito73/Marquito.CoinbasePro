using Flurl.Http;
using Jose;
using Marquito.CoinbasePro.Class.Client.Data.Account;
using Marquito.CoinbasePro.Class.Client.Data.Order;
using Marquito.CoinbasePro.Class.Client.Data.Permissions;
using Marquito.CoinbasePro.Class.Client.Data.Product;
using Marquito.CoinbasePro.Class.Entities.File;
using Marquito.CoinbasePro.Class.Enums;
using Marquito.CoinbasePro.Class.Enums.Extensions;
using Marquito.CoinbasePro.Class.Exceptions;
using MarquitoUtils.Main.Class.Enums.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace Marquito.CoinbasePro.Class.Client.Coinbase
{
    public class CoinbaseProClient : TradingClient
    {
        /// <summary>
        /// The secret key
        /// </summary>
        protected string SecretKey { get; set; }
        /// <summary>
        /// The organization name
        /// </summary>
        protected string OrganizationName { get; set; }
        /// <summary>
        /// The api key
        /// </summary>
        protected string ApiKey { get; set; }

        /// <summary>
        /// Coinbase trading client
        /// </summary>
        /// <param name="secretKey"><The secret key/param>
        /// <param name="organizationName">The organization name</param>
        /// <param name="apiKey">The token</param>
        public CoinbaseProClient(string secretKey, string organizationName, string apiKey)
        {
            Endpoint = "api.coinbase.com/api/v3/brokerage";
            SecretKey = secretKey;
            OrganizationName = organizationName;
            ApiKey = apiKey;
        }

        /// <summary>
        /// Coinbase trading client
        /// </summary>
        /// <param name="tradingConfiguration">The trading configuration</param>
        public CoinbaseProClient(TradingConfiguration tradingConfiguration)
            : this(tradingConfiguration.SecretKey, tradingConfiguration.OrganizationName, tradingConfiguration.ApiKey)
        {

        }

        #region Commons methods

        /// <summary>
        /// Generate a JWT token for a specific resource and return it
        /// </summary>
        /// <param name="resource">A specific resource</param>
        /// <param name="httpMethod">The HTTP method type</param>
        /// <returns>A JWT token for a specific resource</returns>
        protected string GenerateToken(string resource, HttpMethodType httpMethod = HttpMethodType.GET)
        {
            string name = $"organizations/{OrganizationName}/apiKeys/{ApiKey}";

            byte[]? privateKeyBytes = Convert.FromBase64String(SecretKey);

            string? encodedToken;
            using (ECDsa key = ECDsa.Create())
            {

                key.ImportECPrivateKey(privateKeyBytes, out _);

                Dictionary<string, object> payload = new Dictionary<string, object>
                {
                    { "sub", name },
                    { "iss", "coinbase-cloud" },
                    { "nbf", Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) },
                    { "exp", Convert.ToInt64((DateTime.UtcNow.AddMinutes(1) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds) },
                    { "uri", $"{httpMethod.ToString()} {GetMainEndpoint()}/{resource}" },
                };

                Dictionary<string, object> extraHeaders = new Dictionary<string, object>
                {
                    { "kid", name },
                    // add nonce to prevent replay attacks with a random 10 digit number
                    { "nonce", RandomHex(10) },
                    { "typ", "JWT"},
                };

                encodedToken = JWT.Encode(payload, key, JwsAlgorithm.ES256, extraHeaders).Trim();
            }

            return encodedToken;
        }

        protected bool IsTokenValid(string token)
        {
            string tokenId = $"organizations/{OrganizationName}/apiKeys/{ApiKey}";
            if (token == null)
                return false;

            var key = ECDsa.Create();
            key?.ImportECPrivateKey(Convert.FromBase64String(SecretKey), out _);

            var securityKey = new ECDsaSecurityKey(key) { KeyId = tokenId };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string RandomHex(int digits)
        {
            Random random = new Random();
            byte[] buffer = new byte[digits / 2];
            random.NextBytes(buffer);
            string result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
                return result;
            return result + random.Next(16).ToString("X");
        }

        #endregion Commons methods

        #region Accounts

        public override List<Account> GetAccounts()
        {
            string resource = $"accounts";

            IFlurlRequest request = GetMainUrl()
                .AppendPathSegment(resource)
                .WithOAuthBearerToken(GenerateToken(resource));

            return request.GetJsonAsync<AccountResponse>().Result.Accounts.Where(x => x.AccountBalance.Value > 0).ToList();
        }

        public override Account GetAccount(Guid accountID)
        {
            string resource = $"accounts";

            IFlurlRequest request = GetMainUrl()
                .AppendPathSegment(resource)
                .AppendPathSegment(accountID.ToString())
                .WithOAuthBearerToken(GenerateToken(resource));

            return request.GetJsonAsync<Account>().Result;
        }

        public override Account? GetAccount(string productCurrency)
        {
            return this.GetAccounts().Where(x => x.Currency == productCurrency).FirstOrDefault();
        }

        #endregion Accounts

        #region Products

        public override List<TradingProduct> GetProducts()
        {
            string resource = $"products";

            IFlurlRequest request = GetMainUrl()
                .AppendPathSegment(resource)
                .WithOAuthBearerToken(GenerateToken(resource));

            return request.GetJsonAsync<TradingProductResponse>().Result.Products;
        }

        public override List<Candle> GetCandles(string product, DateTime start, DateTime end, TradingPeriod period)
        {
            string resource = $"products/{product}/candles";

            IFlurlRequest request = GetMainUrl()
                .AppendPathSegment(resource)
                .SetQueryParam("start", new DateTimeOffset(start).ToUnixTimeSeconds())
                .SetQueryParam("end", new DateTimeOffset(end).ToUnixTimeSeconds())
                .SetQueryParam("granularity", period.GetPeriodValue())
                .WithOAuthBearerToken(GenerateToken(resource));

            return request.GetJsonAsync<CandleResponse>().Result.Candles
                .OrderBy(x => x.Start).ToList();
        }

        public override List<Candle> GetCandles(string product, TradingPeriod period, short candleCount = 300)
        {
            DateTime now = DateTime.Now;

            long seconds = period.GetPeriodInSeconds() * candleCount;

            return GetCandles(product, now.AddSeconds(seconds * -1), now, period);
        }

        public override MarketTrade GetLastMarketTrade(string product)
        {
            string resource = $"products/{product}/ticker";

            IFlurlRequest request = GetMainUrl()
                .AppendPathSegment(resource)
                .SetQueryParam("limit", 1)
                .WithOAuthBearerToken(GenerateToken(resource));

            return request.GetJsonAsync<MarketTradeResponse>().Result.Trades.First();
        }

        #endregion Products

        #region Orders

        public void CanBuyOrSell(Account fromAccount, Account targetAccount, double amountToConvert, TradingSide side)
        {
            AccountPermissions accountPermissions = this.GetAccountPermissions();
            if (!accountPermissions.CanTrade)
            {
                throw new ApiKeyTradeRightException(accountPermissions);
            }
            if (side == TradingSide.SELL && fromAccount.AccountBalance.Value < amountToConvert)
            {
                throw new NoFundsAvailableException(fromAccount, $"{fromAccount.Currency}-{targetAccount.Currency}", side);
            }
            else if (side == TradingSide.BUY && targetAccount.AccountBalance.Value < amountToConvert)
            {
                throw new NoFundsAvailableException(targetAccount, $"{fromAccount.Currency}-{targetAccount.Currency}", side);
            }
        }

        public override Order ConvertCrypto(Account fromAccount, Account targetAccount, double amountToConvert, TradingSide side)
        {
            this.CanBuyOrSell(fromAccount, targetAccount, amountToConvert, side);

            string resource = "orders";

            IFlurlRequest request = GetMainUrl()
                .AppendPathSegment(resource)
                .WithOAuthBearerToken(this.GenerateToken(resource, HttpMethodType.POST));

            return request.PostJsonAsync(
                new
                {
                    client_order_id = Guid.NewGuid().ToString(),
                    product_id = $"{fromAccount.Currency}-{targetAccount.Currency}",
                    side = side.ToString(),
                    order_configuration = new
                    {
                        market_market_ioc = new
                        {
                            base_size = amountToConvert.ToString()
                        }
                    },
                }).ReceiveJson<OrderResponse>().Result.Order;
        }

        public override Order ConvertCrypto(string productID, double amountToConvert, TradingSide side)
        {
            List<Account> accounts = GetAccounts();

            Account fromAccount = accounts.Where(x => x.Currency == productID.Split('-')[0]).First();
            Account targetAccount = accounts.Where(x => x.Currency == productID.Split('-')[1]).First();

            return this.ConvertCrypto(fromAccount, targetAccount, amountToConvert, side);
        }

        public override Order ConvertAllCrypto(Account fromAccount, Account targetAccount, TradingSide side)
        {
            double amount;

            if (side == TradingSide.SELL)
            {
                amount = fromAccount.AccountBalance.Value;
            }
            else
            {
                amount = targetAccount.AccountBalance.Value;
            }

            return this.ConvertCrypto(fromAccount, targetAccount, amount, side);
        }

        public override Order ConvertAllCrypto(string productID, TradingSide side)
        {
            List<Account> accounts = GetAccounts();

            Account fromAccount = accounts.Where(x => x.Currency == productID.Split('-')[0]).First();
            Account targetAccount = accounts.Where(x => x.Currency == productID.Split('-')[1]).First();

            return this.ConvertAllCrypto(fromAccount, targetAccount, side);
        }

        #endregion Orders

        #region Permissions

        public override AccountPermissions GetAccountPermissions()
        {
            string resource = "key_permissions";

            IFlurlRequest request = GetMainUrl()
                .AppendPathSegment(resource)
                .WithOAuthBearerToken(GenerateToken(resource));

            return request.GetJsonAsync<AccountPermissions>().Result;
        }

        #endregion Permissions
    }
}
