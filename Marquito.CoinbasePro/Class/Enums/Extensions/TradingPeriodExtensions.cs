using MarquitoUtils.TradingAPI.Class.Enums;

namespace Marquito.CoinbasePro.Class.Enums.Extensions
{
    public static class TradingPeriodExtensions
    {
        public static int GetPeriodInSeconds(this TradingPeriod period)
        {
            int intPeriod;

            switch (period)
            {
                case TradingPeriod.M1:
                    intPeriod = 60;
                    break;
                case TradingPeriod.M5:
                    intPeriod = 300;
                    break;
                case TradingPeriod.M15:
                    intPeriod = 900;
                    break;
                case TradingPeriod.M30:
                    intPeriod = 1800;
                    break;
                case TradingPeriod.H1:
                    intPeriod = 3600;
                    break;
                case TradingPeriod.H2:
                    intPeriod = 7200;
                    break;
                case TradingPeriod.H6:
                    intPeriod = 21600;
                    break;
                case TradingPeriod.D1:
                    intPeriod = 86400;
                    break;
                default:
                    intPeriod = 60;
                    break;
            }

            return intPeriod;
        }
        public static string GetPeriodValue(this TradingPeriod period)
        {
            string periodValue;

            switch (period)
            {
                case TradingPeriod.M1:
                    periodValue = "ONE_MINUTE";
                    break;
                case TradingPeriod.M5:
                    periodValue = "FIVE_MINUTE";
                    break;
                case TradingPeriod.M15:
                    periodValue = "FIFTEEN_MINUTE";
                    break;
                case TradingPeriod.M30:
                    periodValue = "THIRTY_MINUTE";
                    break;
                case TradingPeriod.H1:
                    periodValue = "ONE_HOUR";
                    break;
                case TradingPeriod.H2:
                    periodValue = "TWO_HOUR";
                    break;
                case TradingPeriod.H6:
                    periodValue = "SIX_HOUR";
                    break;
                case TradingPeriod.D1:
                    periodValue = "ONE_DAY";
                    break;
                default:
                    periodValue = "ONE_DAY";
                    break;
            }

            return periodValue;
        }
    }
}
