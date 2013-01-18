using System;

namespace Put.io.Core.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ToDateTime(this string @this)
        {
            return DateTime.Parse(@this);
        }
    }
}