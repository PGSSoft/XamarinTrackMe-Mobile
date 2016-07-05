using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace TrackMe.WinPhoneUniversalNative.Converters
{
    public class InternetStatusToImageFormsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? "../Assets/wifi_on.png" : "../Assets/wifi_off.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
