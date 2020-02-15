using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hoster.Converters
{
    public sealed class BooleanToVisibilityCollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = bool.Parse(value.ToString());
            return visible && visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
