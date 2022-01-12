using System;
using System.Globalization;
using System.Text;

namespace WA4D0G.MaintenanceTools
{
    public static class TimeStampGenerator
    {
        public static string GetDateTimeString(DateTime dateTime)
        {
            StringBuilder dateTimeString = new StringBuilder(dateTime.ToString());
            dateTimeString.Replace(DateTimeFormatInfo.CurrentInfo.DateSeparator, "-")
                          .Replace(" ", "_")
                          .Replace(":", "-");
            return dateTimeString.ToString();
        }
    }
}
