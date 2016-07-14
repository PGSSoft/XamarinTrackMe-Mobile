using System;
using System.Globalization;
using TrackMe.Core.Services.Interfaces;
using Xamarin.Forms;

namespace TrackMe.Infrastructure.Converters
{
    class GpsStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Color))
                throw new InvalidOperationException("The target must be a Color");
            
            switch ((LocationStatus)value)
            {
                case LocationStatus.Started:
                    return (Color) Application.Current.Resources["StatusStartedColor"];
                case LocationStatus.Initializing:
                    return (Color) Application.Current.Resources["StatusInitializingColor"];
            }
            return (Color)Application.Current.Resources["StatusStoppedColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}