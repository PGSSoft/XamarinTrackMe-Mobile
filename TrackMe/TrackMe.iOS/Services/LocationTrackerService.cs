


using System;
using System.Diagnostics;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using TrackMe.Core;
using TrackMe.Core.Messages;
using TrackMe.Core.Models;
using TrackMe.Core.Services.Interfaces;
using UIKit;

namespace TrackMe.iOS.Services
{

    public class LocationTrackerService
    {
        private  IMvxMessenger _messenger;

        private ISendPositionService _sendPositionService;
        private ISettings _settings;
        private MvxSubscriptionToken _locationMessageToken;
        MvxSubscriptionToken _tokenStop;
        MvxSubscriptionToken _tokenStart;
        MvxSubscriptionToken _tokenLocation;
        private string _token;
        private int _duration;
        private DateTime _startTime;
        private DateTime EndTime => _startTime + TimeSpan.FromMinutes(_duration);
        private bool IsTracking { get;  set; }


        public LocationTrackerService()
        {
            _sendPositionService = Mvx.Resolve<ISendPositionService>();
            _settings = Mvx.Resolve<ISettings>();
            _messenger = Mvx.Resolve<IMvxMessenger>();
            TryRestore();
        }
     
        private void TryRestore()
        {
            var token = _settings.GetValue<string>(SettingKeys.Token);
            var endTime = _settings.GetValue<DateTime>(SettingKeys.EndTime, DateTime.MinValue);
            var shouldRestore = (endTime - DateTime.Now) > TimeSpan.Zero;
            if (!string.IsNullOrEmpty(token) && shouldRestore)
            {
                _token = token;
                _duration = _settings.GetValue<int>(SettingKeys.Duration, 240);
                _startTime = endTime.AddMinutes(-_duration);
                IsTracking = true;
            }
        }


        public void Subscribe()
        {

            if (_tokenStart != null)
            {
                return;
            }

            _tokenStart = _messenger.Subscribe<StartTrackingMessage>(m =>
            {
                _startTime = m.TimeStamp;
                _duration = m.Duration;
                _token = m.Token;
                IsTracking = true;
            });

            _tokenStop = _messenger.Subscribe<StopTrackingMessage>(m =>
            {
                IsTracking = false;
            });

            _tokenLocation = _messenger.Subscribe<LocationMessage>(m =>
            {
                if (CheckIfNotExpired() && !m.IsError)
                {
                    _sendPositionService.SendPosition(_token, new Position(m.Lat, m.Lng), m.Address, m.Accuracy);
                }
            });
        }

        private void Unsubscribe()
        {
          

            _messenger.Unsubscribe<LocationMessage>(_locationMessageToken);

        }
        private bool CheckIfNotExpired()
        {
            if (IsTracking && DateTime.Now < EndTime)
                return true;

            if (IsTracking && DateTime.Now > EndTime)
            {
                IsTracking = false;
            }

            return false;
        }
    }
}