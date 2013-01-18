using System;

namespace Put.io.Core.Extensions
{
    public static class SizeExtensions
    {
        public static string ToTransferSpeed(this int source)
        {
            return ToTransferSpeed(Convert.ToInt64(source));
        }

        public static string ToTransferSpeed(this long source)
        {
            const int byteConversion = 1024;
            var bytes = Convert.ToDouble(source);

            if (bytes >= Math.Pow(byteConversion, 3)) //GB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB/s");
            }

            if (bytes >= Math.Pow(byteConversion, 2)) //MB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB/s");
            }

            if (bytes >= byteConversion) //KB Range
            {
                return string.Concat(Math.Round(bytes / byteConversion, 2), " KB/s");
            }

            //Bytes
            return string.Concat(bytes, " B/s");
        }

        public static string ToFileSize(this int source)
        {
            return ToFileSize(Convert.ToInt64(source));
        }

        public static string ToFileSize(this long source)
        {
            const int byteConversion = 1024;
            double bytes = Convert.ToDouble(source);

            if (bytes >= Math.Pow(byteConversion, 3)) //GB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
            }
            else if (bytes >= Math.Pow(byteConversion, 2)) //MB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
            }
            else if (bytes >= byteConversion) //KB Range
            {
                return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
            }
            else //Bytes
            {
                return string.Concat(bytes, " Bytes");
            }
        }
    }
}