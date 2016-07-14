using MvvmCross.Platform.Converters;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace TrackMe.Infrastructure
{
    class MvxFormsValueConverter<T> : IValueConverter where T: IMvxValueConverter, new()
    {
        private readonly T _wrapped = new T();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _wrapped.Convert(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _wrapped.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
