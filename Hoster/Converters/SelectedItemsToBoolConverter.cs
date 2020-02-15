﻿using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Hoster.Converters
{
    public sealed class SelectedItemsToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && ((IList) value).Count > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
