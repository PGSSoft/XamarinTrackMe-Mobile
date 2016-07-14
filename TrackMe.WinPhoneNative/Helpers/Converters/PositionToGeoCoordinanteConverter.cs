using System;
using System.Device.Location;
using System.Globalization;
using System.Windows.Data;
using TrackMe.Core.Models;

namespace TrackMe.WinPhoneNative.Helpers.Converters
{
    public class PositionToGeoCoordinanteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var position = value as Position;
            var coords = new GeoCoordinate();
            if (position != null)
            {
                coords.Longitude = position.Longitude;
                coords.Latitude = position.Latitude;
            }     

            return position;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
