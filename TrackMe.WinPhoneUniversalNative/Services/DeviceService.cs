using System;
using Windows.Devices.Geolocation;
using Windows.Networking.Connectivity;
using MvvmCross.Platform.Core;
using TrackMe.Core.Services.Interfaces;
using TrackMe.WinPhoneUniversalNative.Helpers;

namespace TrackMe.WinPhoneUniversalNative.Services
{
    public class DeviceService : IDeviceService
    {
        Geolocator _geolocator;
        
        public bool IsGpsEnabled()
        {
            return _geolocator.LocationStatus != PositionStatus.Disabled;
        }

        public LocationStatus LocationStatus => _geolocator.LocationStatus.ToLocationStatus();
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

        private void GeolocatorOnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            var handler = LocationStatusChanged;
            if (handler != null)
            {
                MvxMainThreadDispatcher.Instance.RequestMainThreadAction(
                    () => handler(this, new LocationStatusChangedEventArgs() { Status = args.Status.ToLocationStatus() })
                );
            }
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
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() ==
                   NetworkConnectivityLevel.InternetAccess;
        }
    }

    
}