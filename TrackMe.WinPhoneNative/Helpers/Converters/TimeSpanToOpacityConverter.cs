using System;
using System.Globalization;
using System.Windows.Data;

namespace TrackMe.WinPhoneNative.Helpers.Converters
{
    public class TimeSpanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (TimeSpan)value;

            if (val == TimeSpan.Zero)
                return OpacityIfZero;
            else
                return OpacityGreaterThanZero;
        }

        public double OpacityIfZero { get; set; } = 1;
        public double OpacityGreaterThanZero { get; set; } = 0.4;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
