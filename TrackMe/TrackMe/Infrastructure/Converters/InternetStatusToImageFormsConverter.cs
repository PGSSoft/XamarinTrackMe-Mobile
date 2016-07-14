using System;
using System.Globalization;
using Xamarin.Forms;

namespace TrackMe.Infrastructure.Converters
{
    public class InternetStatusToImageFormsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "wifi_on.png" : "wifi_off.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
