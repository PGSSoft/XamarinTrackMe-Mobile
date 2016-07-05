using System;
using Windows.UI.Xaml.Data;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Converters
{
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
}