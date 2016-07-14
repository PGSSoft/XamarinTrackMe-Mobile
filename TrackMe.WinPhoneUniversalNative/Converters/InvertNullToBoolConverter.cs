using System;
using Windows.UI.Xaml.Data;

namespace TrackMe.WinPhoneUniversalNative.Converters
{
    public class InvertNullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}