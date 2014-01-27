using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Infrastructure.Extensions
{
    public static class StringExtentions
    {
        public static string FormatWith(this string format, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return string.Format(format, args);
        }

        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            return string.Format(provider, format, args);
        }

        public static string RemoveRavenIdPrefix(this string ravenId)
        {
            if (string.IsNullOrWhiteSpace(ravenId))
            {
                return "";
            }
            var index = ravenId.IndexOf("/");
            if (index >= 0)
	        {
                return ravenId.Substring(index + 1);
	        }
            return ravenId;
        }
            
        public static int ConvertToInt32(this string ravenId)
        {
            if (string.IsNullOrWhiteSpace(ravenId))
            {
                return 0;
            }
            var index = ravenId.IndexOf("/");
            if (index >= 0)
            {
                int id;
                if (int.TryParse(ravenId.Substring(index + 1), out id))
                    return id;
                else
                    return 0;
            }
            return Convert.ToInt32(ravenId);
        }
    }
}
