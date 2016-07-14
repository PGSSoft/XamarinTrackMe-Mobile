using System;
using System.Globalization;
using System.Windows.Data;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneNative.Helpers.Converters
{
    public class GpsStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
