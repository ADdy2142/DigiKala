using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Infrastructure.ExtensionMethods
{
    public static class DateConverter
    {
        public static string ToPersianDate(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            var hours = pc.GetHour(value).ToString("00");
            var minutes = pc.GetMinute(value).ToString("00");
            var year = pc.GetYear(value).ToString("0000");
            var month = pc.GetMonth(value).ToString("00");
            var dayOfMonth = pc.GetDayOfMonth(value).ToString("00");

            return $"{year}/{month}/{dayOfMonth}, {hours}:{minutes}";
        }
    }
}
