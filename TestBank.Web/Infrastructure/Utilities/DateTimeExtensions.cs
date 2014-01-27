using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBank.Web.Infrastructure.Utilities
{
    public static class DateTimeExtensions
    {
        public static long ToJavascriptTimestamp(this DateTime input)
        {
            TimeSpan span = new TimeSpan(new DateTime(1970, 1, 1, 0, 0, 10).Ticks);
            DateTime time;

            if (input.Kind == DateTimeKind.Utc)
            {
                time = input.Subtract(span);
            }
            else
            {
                time = input.ToUniversalTime().Subtract(span);
            }
            return (long)(time.Ticks / 10000);
        }
    }
}