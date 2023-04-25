using System.Globalization;

namespace Utils
{
    /// <summary>
    /// 日期时间工具类
    /// </summary>
    public class DateUtil
    {
        public static readonly string yyyyMMddHHmmss = "yyyyMMddHHmmss";

        public static readonly string yyyyMMdd = "yyyyMMdd";

        #region GetDate
        public static DateTime GetDate(int? time, int? timeUnit)
        {
            if (timeUnit == 1)
            {
                return DateTime.Now.Date.AddDays(0 - time.Value);
            }
            else if (timeUnit == 2)
            {
                return DateTime.Now.Date.AddDays(0 - time.Value * 7);
            }
            else if (timeUnit == 3)
            {
                return DateTime.Now.Date.AddMonths(0 - time.Value);
            }
            else
            {
                throw new Exception("GetTime错误");
            }
        }
        #endregion

        public static bool Validate(string strDateTime, string format)
        {
            return DateTime.TryParseExact(strDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _);
        }

        public static DateTime TryParse(string strDateTime, string format)
        {
            DateTime.TryParseExact(strDateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
            return result;
        }

        public static DateTime Parse(string strDateTime, string format)
        {
            return DateTime.ParseExact(strDateTime, format, CultureInfo.InvariantCulture);
        }

    }
}
