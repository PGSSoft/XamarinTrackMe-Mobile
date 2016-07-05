using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using TrackMe.Core.Messages;
using TrackMe.Core.Models;
using TrackMe.Core.Services.Interfaces;
using Debug = System.Diagnostics.Debug;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;

namespace TrackMe.Droid.Services
{
    [Service]
    public class LocationTrackerService : Service
    {
        private ISendPositionService _sendPositionService;
        private MvxSubscriptionToken _locationMessageToken;
        private string _token;
        private int _duration;
        private DateTime _startTime;
        private DateTime EndTime => _startTime + TimeSpan.FromMinutes(_duration);
        private bool _nullInttent;
        
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // TODO this method can return value that force to redeliver intent, but app is still crashing, because Resolvers aren't working due to main process kill
            if (intent == null)
            {
                _nullInttent = true;
                StopSelf();
            }
            else
            {
                _sendPositionService = Mvx.Resolve<ISendPositionService>();

                var extras = intent.Extras;

                _token = extras.GetString("token");
                _duration = extras.GetInt("duration");
                _startTime = DateTime.Now;
                Subscribe();
            }
            return base.OnStartCommand(intent ?? new Intent(this, typeof(LocationTrackerService)), flags, startId);
        }

        public override void OnDestroy()
        {
      
            base.OnDestroy();
            Unsubscribe();
        }

        private void Subscribe()
        {
            var messenger = Mvx.Resolve<IMvxMessenger>();

            _locationMessageToken = messenger.Subscribe<LocationMessage>(async message =>
            {
                if (message.Lat == (-1) && message.Lng == (-1))
                    return;


                if (DateTime.Now > EndTime)
                    StopSelf();

                if(Mvx.Resolve<IDeviceService>().IsInternetAvailable())
                    await _sendPositionService.SendPosition(_token, new Position(message.Lat, message.Lng), message.Address, message.Accuracy);
            });
        }

        private void Unsubscribe()
        {
            if (_nullInttent)
                return;

            var messenger = Mvx.Resolve<IMvxMessenger>();
            messenger.Unsubscribe<LocationMessage>(_locationMessageToken);

        }
    }
}