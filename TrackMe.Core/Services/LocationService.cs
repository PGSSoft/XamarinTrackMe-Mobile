using System;
using System.Diagnostics;
using System.Linq;
using MvvmCross.Plugins.Location;
using MvvmCross.Plugins.Messenger;
using TrackMe.Core.Messages;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly IMvxLocationWatcher _watcher;
        private readonly IMvxMessenger _messenger;
        private readonly IGeoCoderService _geoCoder;

        public LocationService(IMvxLocationWatcher watcher, IMvxMessenger messenger, IGeoCoderService geoCoder)
        {
            _watcher = watcher;
            _messenger = messenger;
            _geoCoder = geoCoder;
        }

        private void Error(MvxLocationError error)
        {
            var message = new LocationMessage(this, -1, -1, true, error.Code.ToString());
            _watcher.Stop();
            _messenger.Publish(message);
        }

        private async void Success(MvxGeoLocation location)
        {
            var address = string.Empty;
            try
            {
                var add = await _geoCoder.GetAddress(location.Coordinates.Latitude, location.Coordinates.Longitude);
                address = add.Aggregate(string.Empty, (current, s) => current + s);
#if DEBUG
                address += DateTime.Now;
#endif
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            var message = new LocationMessage(this, location.Coordinates.Latitude, location.Coordinates.Longitude)
            {
                Accuracy = location.Coordinates.Accuracy,
                Address = address
            };  
            Debug.WriteLine($"LS: {message.Lat} {message.Lng}");
            _messenger.Publish(message);
        }

        public void StartWatching()
        {
            if (!_watcher.Started)
            {        
                _watcher.Start(new MvxLocationOptions { TimeBetweenUpdates = Constants.TimeBetweenUpdates, TrackingMode = MvxLocationTrackingMode.Background}, Success, Error);       
            }
            Debug.WriteLine("Start watching");
        }

        public void Stop()
        {
            _watcher.Stop();
        }

        public bool IsWatching => _watcher.Started;
    }


}
