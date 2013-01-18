using System;
using System.Globalization;
using System.Windows.Data;

namespace Put.io.Core.Converters
{
    public class InvertBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var actualVal = value as bool?;

            if (actualVal.HasValue)
                return !actualVal.Value;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}