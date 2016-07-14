using System;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Net.NetworkInformation;
using MvvmCross.Platform.Core;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneNative.Services
{
    public class DeviceService : IDeviceService
    {
        private Geolocator _geolocator;

        public bool IsGpsEnabled()
        {
            return _geolocator.LocationStatus != PositionStatus.Disabled;
        }

        public LocationStatus LocationStatus => _geolocator.LocationStatus.ToLocationStatus();

        private void GeolocatorOnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            var handler = LocationStatusChanged;
            if (handler != null)
            {
                MvxMainThreadDispatcher.Instance.RequestMainThreadAction(
                    () =>  handler(this, new LocationStatusChangedEventArgs() {Status = args.Status.ToLocationStatus()})
                );
            }
        }
        
        public event EventHandler<LocationStatusChangedEventArgs> LocationStatusChanged;

        public bool StartLocationStatusWatching()
        {
            if (_geolocator == null)
            {
                _geolocator = new Geolocator();
                _geolocator.ReportInterval = 1000;
                _geolocator.StatusChanged += GeolocatorOnStatusChanged;
            }

            return true;
        }

        public bool StopLocationStatusWatching()
        {
            if (_geolocator != null)
            {
                _geolocator.StatusChanged -= GeolocatorOnStatusChanged;
                _geolocator = null;
            }

            return true;
        }

        public bool IsInternetAvailable()
        {    
            var ni = NetworkInterface.NetworkInterfaceType;
            bool isConnected = false;
            if ((ni == NetworkInterfaceType.Wireless80211) || (ni == NetworkInterfaceType.MobileBroadbandCdma) || (ni == NetworkInterfaceType.MobileBroadbandGsm))
                isConnected = true;
            else if (ni == NetworkInterfaceType.None)
                isConnected = false;
            return isConnected;
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
}
