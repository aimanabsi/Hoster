using Hoster.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Hoster.Converters
{
    public sealed class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool) value)
                return "Yes";

            return "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
