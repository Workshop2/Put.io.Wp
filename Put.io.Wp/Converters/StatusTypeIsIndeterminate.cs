using System;
using System.Globalization;
using System.Windows.Data;
using Put.io.Core.Models;

namespace Put.io.Wp.Converters
{
    public class StatusTypeIsIndeterminate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StatusType)
            {
                switch ((StatusType)value)
                {
                    case StatusType.InQueue:
                    case StatusType.Completing:
                    case StatusType.Seeding:
                        return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}