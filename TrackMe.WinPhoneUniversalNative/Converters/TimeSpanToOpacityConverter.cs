using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace TrackMe.WinPhoneUniversalNative.Converters
{
    public class TimeSpanToOpacityConverter : IValueConverter
    {
        public double OpacityIfZero { get; set; } = 1;
        public double OpacityGreaterThanZero { get; set; } = 0.4;

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = (TimeSpan) value;

            if (val == TimeSpan.Zero)
                return OpacityIfZero;
            else
                return OpacityGreaterThanZero;
        }
    }
}