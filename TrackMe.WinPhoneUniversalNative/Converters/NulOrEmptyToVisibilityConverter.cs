using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Converters
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeSpanToOpacityConverter : IValueConverter
    {
        public double OpacityIfZero { get; set; } = 1;
        public double OpacityGreaterThanZero { get; set; } = 0.4;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (TimeSpan)value;

            if (val == TimeSpan.Zero)
                return OpacityIfZero;
            else
                return OpacityGreaterThanZero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanConverter<T> : IValueConverter
    {
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public T True { get; set; }
        public T False { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool && ((bool)value) ? True : False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }
    }

    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }

    public sealed class InvertBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public InvertBooleanToVisibilityConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        { }
    }

    public class GpsStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (LocationStatus)value;
            var path = string.Empty;

            if (val == LocationStatus.Initializing)
                path = "../Assets/gps_ini.png";
            else if (val == LocationStatus.Started)
                path = "../Assets/gps_active.png";
            else
                path = "../Assets/gps_notactive.png";

            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertNullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
