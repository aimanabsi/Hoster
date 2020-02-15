using System;
using System.Globalization;
using System.Windows.Data;

namespace Hoster.Converters
{
    public sealed class ApplicationNameToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return ApplicationViewManager.GetIconByName((ApplicationViewManager.Name)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
