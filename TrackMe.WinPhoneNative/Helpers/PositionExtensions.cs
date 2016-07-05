using System.Device.Location;
using TrackMe.Core.Models;

namespace TrackMe.WinPhoneNative.Helpers
{
    public static class PositionExtensions
    {
        public static GeoCoordinate ToNative(this Position position)
        {
            var geoCoordinate = new GeoCoordinate()
            {
                Longitude = position.Longitude,
                Latitude = position.Latitude,
                HorizontalAccuracy = position.Accuracy ?? 0
            };

            return geoCoordinate;
        }
    }
}
