using System;
using System.Globalization;
using System.Windows.Data;

namespace Hoster.Converters
{
    public sealed class NullableDateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            

            return "-/-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
