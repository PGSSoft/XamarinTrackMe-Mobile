using System;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;
using TrackMe.Core.Models;
using TrackMe.WinPhoneUniversalNative.Helpers;

namespace TrackMe.WinPhoneUniversalNative.Converters
{
    class PositionToGeoPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var position = value as Position;
            var geoPosition = position?.ToNative() ?? new Geopoint(new BasicGeoposition());
            return geoPosition;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
