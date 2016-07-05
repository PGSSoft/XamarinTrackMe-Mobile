using System;
using System.Globalization;
using TrackMe.Core.Services.Interfaces;
using Xamarin.Forms;

namespace TrackMe.Infrastructure.Converters
{
    public class GpsStatusToImageFormsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = string.Empty;

            switch ((LocationStatus)value)
            {
                case LocationStatus.Initializing:
                    path = "gps_ini.png";
                    break;
                case LocationStatus.Started:
                    path = "gps_active.png";
                    break;
                default:
                    path = "gps_notactive.png";
                    break;
            }
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
