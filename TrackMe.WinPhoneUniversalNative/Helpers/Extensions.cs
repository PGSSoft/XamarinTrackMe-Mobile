using System;
using Windows.Devices.Geolocation;
using TrackMe.Core.Models;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Helpers
{
    static class Extensions
    {
        public static Geopoint ToNative(this Position position)
        {
            return new Geopoint(new BasicGeoposition(){ Latitude = position.Latitude, Longitude = position.Longitude });
        }
    }

    public static class PhoneExtensions
    {
        public static LocationStatus ToLocationStatus(this PositionStatus status)
        {
            switch (status)
            {
                case PositionStatus.Disabled | PositionStatus.NotAvailable | PositionStatus.NoData | PositionStatus.NotInitialized:
                    return LocationStatus.Stopped;
                case PositionStatus.Ready:
                    return LocationStatus.Started;
                case PositionStatus.Initializing:
                    return LocationStatus.Initializing;
                default:
                    return LocationStatus.Stopped;
            }
        }
    }

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string stringToCheck)
        {
            return String.IsNullOrEmpty(stringToCheck);
        }
        public static bool IsNotNullOrEmpty(this string stringToCheck)
        {
            return !String.IsNullOrEmpty(stringToCheck);
        }
    }
}
