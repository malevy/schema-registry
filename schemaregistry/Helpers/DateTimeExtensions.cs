using System;
using System.Globalization;

namespace SchemaRegistry.Helpers
{
    public static class DateTimeExtensions
    {
        public static string To8601String(this DateTime @this)
        {
            return @this.ToString("O", CultureInfo.InvariantCulture);
        }
    }
}