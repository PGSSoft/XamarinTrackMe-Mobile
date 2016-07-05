using System;
using System.Globalization;
using System.Windows.Data;
using TrackMe.Core.Services.Interfaces;


namespace TrackMe.WinPhoneNative.Helpers.Converters
{
    public class InternetStatusToImageFormsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = String.Empty;

            if((bool)value)
                path = "../Assets/wifi_on.png";
            else
                path = "../Assets/wifi_off.png";
    
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
