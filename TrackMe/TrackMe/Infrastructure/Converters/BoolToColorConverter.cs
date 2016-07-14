using System;
using System.Globalization;
using Xamarin.Forms;

namespace TrackMe.Infrastructure.Converters
{
    class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Color))
                throw new InvalidOperationException("The target must be a Color");
            
            if (!(bool)value)
                return Color.FromHex("ce2323");

            return Color.FromHex("00a561");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
