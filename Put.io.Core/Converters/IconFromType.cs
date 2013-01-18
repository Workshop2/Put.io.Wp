using System;
using System.Globalization;
using System.Windows.Data;
using Put.io.Core.Models;

namespace Put.io.Core.Converters
{
    public class IconFromType : IValueConverter
    {
        private const string IconDirectory = "/Assets/Icons/{0}";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var icon = "ribbon.png";

            if (value is ContentType)
            {
                switch ((ContentType)value)
                {
                    case ContentType.Directory:
                        icon = "folder.png";
                        break;
                    case ContentType.Music:
                        icon = "music.png";
                        break;
                    case ContentType.Video:
                        icon = "video.png";
                        break;
                    case ContentType.Image:
                        icon = "image.png";
                        break;
                }
            }

            return string.Format(IconDirectory, icon);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}