using System;
using System.Globalization;
using Xamarin.Forms;

namespace TrackMe.Infrastructure.Converters
{
    class InternetStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? (Color) Application.Current.Resources["StatusStartedColor"] : (Color)Application.Current.Resources["StatusStoppedColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}