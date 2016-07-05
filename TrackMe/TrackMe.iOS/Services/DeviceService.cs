using System;
using CoreLocation;
using TrackMe.Core.Services.Interfaces;
using UIKit;

namespace TrackMe.iOS.Services
{
    public class DeviceService : IDeviceService
    {
        CLLocationManager _locationManager;

        public DeviceService()
        {
            _locationManager = new CLLocationManager();
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                _locationManager.AllowsBackgroundLocationUpdates = true;
            }
       }

        public bool IsGpsEnabled()
        {
            return CLLocationManager.LocationServicesEnabled;
        }

		LocationStatus _locationStatus; 

		public LocationStatus LocationStatus { 
			get{ 

				if (IsGpsEnabled () == false) {
					return LocationStatus.Stopped;
				}
				return LocationStatus.Started;
			}

		}

        public event EventHandler<LocationStatusChangedEventArgs> LocationStatusChanged = delegate {
			
		};
        public bool StartLocationStatusWatching()
        {
            return true;
        }

        public bool StopLocationStatusWatching()
        {
            _locationManager.Delegate = null;
            _locationManager.StopUpdatingLocation();
            _locationManager.Dispose();
            _locationManager = null;
            return false;
        }

        public bool IsInternetAvailable()
        {
            return true;
        }
    }
}
