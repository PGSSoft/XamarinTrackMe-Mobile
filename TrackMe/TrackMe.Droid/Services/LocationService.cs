using System;
using Android.Content;
using Android.Locations;
using Android.Net;
using Android.OS;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Droid.Services
{
	public class MyGpslistener : Java.Lang.Object, GpsStatus.IListener, IDeviceService, ILocationListener
    {
		public bool StartLocationStatusWatching ()
		{
			throw new NotImplementedException ();
		}

		public bool StopLocationStatusWatching ()
		{
			throw new NotImplementedException ();
		}

        private readonly Context _context;

        public MyGpslistener(Context context)
        {
            _context = context;
        }

        public void OnGpsStatusChanged(GpsEvent e)
        {
            LocationStatus gpsStatus = LocationStatus.Started;
            if (e == GpsEvent.Stopped)
                gpsStatus= LocationStatus.Stopped;
            var handler = LocationStatusChanged;
            handler?.Invoke(this, new LocationStatusChangedEventArgs() {Status = gpsStatus});
        }

        private LocationStatus GetLocationStatus(GpsEvent e)
        {
            switch (e)
            {
                case GpsEvent.FirstFix:
                        return LocationStatus.Started;
                case GpsEvent.SatelliteStatus:
                        if (_lastLocation != null)
                            _isGpsFix = (SystemClock.ElapsedRealtime() - _lastLocationMillis) < 3000;
                        return _isGpsFix ? LocationStatus.Started : LocationStatus.Initializing;
                case GpsEvent.Stopped:
                    return LocationStatus.Stopped;
                case GpsEvent.Started:
                    return LocationStatus.Started;
                default:
                    return LocationStatus.Stopped;
            }
        }

        Location _lastLocation;
        bool _isGpsFix;
        private long _lastLocationMillis;

        public bool IsGpsEnabled()
        {
            return LocationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        public LocationStatus LocationStatus => LocationStatus.Initializing;

        public event EventHandler<LocationStatusChangedEventArgs> LocationStatusChanged;

        public bool IsInternetAvailable()
        {
            ConnectivityManager cm = _context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;
            NetworkInfo netInfo = cm.ActiveNetworkInfo;
            return netInfo != null && netInfo.IsConnectedOrConnecting;
        }

        private LocationManager LocationManager => _context.GetSystemService(Context.LocationService) as LocationManager;

        public void OnLocationChanged(Location location)
        {
            if (location == null)
                return;

            _lastLocationMillis = SystemClock.ElapsedRealtime();

            _lastLocation = location;
        }

        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
           
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            
        }
    }
}