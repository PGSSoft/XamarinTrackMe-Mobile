using Windows.Devices.Geolocation;
using TrackMe.Core.Models;

namespace TrackMe.WinPhoneUniversalNative.Helpers
{
    static class Extensions
    {
        public static Geopoint ToNative(this Position position)
        {
            return new Geopoint(new BasicGeoposition(){ Latitude = position.Latitude, Longitude = position.Longitude });
        }
    }
}
