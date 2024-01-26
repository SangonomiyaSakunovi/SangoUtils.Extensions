using System;
using System.Text;

namespace SangoUtils_Extensions_Universal.Utils
{
    public static class DateTimeUtils
    {
        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - _unixEpoch).TotalSeconds;
        }
        public static DateTime FromUnixTimestamp(long timestamp)
        {
            return _unixEpoch.AddSeconds(timestamp).ToLocalTime();
        }
        public static string ToUnixTimestampString(this DateTime dateTime)
        {
            return dateTime.ToUnixTimestamp().ToString();
        }
        public static DateTime FromUnixTimestampString(string timestampString)
        {
            return FromUnixTimestamp(Convert.ToInt64(timestampString)).ToLocalTime();
        }
        public static string ToUnixTimestampString(this DateTime dateTime, string format)
        {
            return dateTime.ToUnixTimestamp().ToString(format);
        }
        public static DateTime FromUnixTimestampString(string timestampString, string format)
        {
            return FromUnixTimestamp(Convert.ToInt64(timestampString)).ToLocalTime();
        }
        public static string ToUnixTimestampString(this DateTime dateTime, string format, string cultureName)
        {
            return dateTime.ToUnixTimestamp().ToString(format, new System.Globalization.CultureInfo(cultureName));
        }
        public static DateTime FromUnixTimestampString(string timestampString, string format, string cultureName)
        {
            return FromUnixTimestamp(Convert.ToInt64(timestampString)).ToLocalTime();
        }
        public static string ToUnixTimestampString(this DateTime dateTime, string format, System.Globalization.CultureInfo culture)
        {
            return dateTime.ToUnixTimestamp().ToString(format, culture);
        }
        public static DateTime FromUnixTimestampString(string timestampString, string format, System.Globalization.CultureInfo culture)
        {
            return FromUnixTimestamp(Convert.ToInt64(timestampString)).ToLocalTime();
        }
        public static string ToUnixTimestampString(this DateTime dateTime, string format, System.Globalization.DateTimeFormatInfo dateTimeFormat)
        {
            return dateTime.ToUnixTimestamp().ToString(format, dateTimeFormat);
        }
        public static DateTime FromUnixTimestampString(string timestampString, string format, System.Globalization.DateTimeFormatInfo dateTimeFormat)
        {
            return FromUnixTimestamp(Convert.ToInt64(timestampString)).ToLocalTime();
        }

        public static DateTime ToDataTime(int year, int month, int day)
        {
            if (year < 1970 || year > 2100 || month < 1 || month > 12)
            {
                return DateTime.MinValue;
            }
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day < 1 || day > daysInMonth)
            {
                return DateTime.MinValue;
            }
            return new DateTime(year, month, day);
        }
        public static DateTime ToDataTime(int year, int month, int day, int hour, int minute, int second)
        {
            if (year < 1970 || year > 2100 || month < 1 || month > 12)
            {
                return DateTime.MinValue;
            }
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day < 1 || day > daysInMonth)
            {
                return DateTime.MinValue;
            }
            return new DateTime(year, month, day, hour, minute, second);
        }
        public static DateTime ToDataTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            if (year < 1970 || year > 2100 || month < 1 || month > 12)
            {
                return DateTime.MinValue;
            }
            int daysInMonth = DateTime.DaysInMonth(year, month);
            if (day < 1 || day > daysInMonth)
            {
                return DateTime.MinValue;
            }
            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string ToBase64(long timestamp)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(timestamp.ToString());
            return Convert.ToBase64String(bytes);
        }
        public static string ToBase64(string timestamp)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(timestamp);
            return Convert.ToBase64String(bytes);
        }
        public static string FromBase64ToString(string base64Data)
        {
            byte[] bytes = Convert.FromBase64String(base64Data);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
