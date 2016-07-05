using System;
using System.Diagnostics;
using MvvmCross.Plugins.Messenger;
using TrackMe.Core;
using TrackMe.Core.Messages;
using TrackMe.Core.Models;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative
{
    public class LocationTrackerService : ILocationTrackerService
    {
        private readonly IMvxMessenger _messenger;
        private readonly ISendPositionService _sendPositionService;
        private readonly ISettings _settings;

        MvxSubscriptionToken _tokenStart;
        MvxSubscriptionToken _tokenLocation;
        MvxSubscriptionToken _tokenStop;

        public string Token { get; set; }
        public DateTime EndTime => StartTime.AddMinutes(Duration);
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public bool IsTracking { get; private set; }

        public LocationTrackerService(IMvxMessenger messenger, ISendPositionService sendPositionService, ISettings settings)
        {
            _messenger = messenger;
            _sendPositionService = sendPositionService;
            _settings = settings;
            TryRestore();
        }

        private void TryRestore()
        {
            var token = _settings.GetValue<string>(SettingKeys.Token);
            var endTime = _settings.GetValue<DateTime>(SettingKeys.EndTime, DateTime.MinValue);
            var shouldRestore = (endTime - DateTime.Now) > TimeSpan.Zero;
            if (!string.IsNullOrEmpty(token) && shouldRestore)
            {
                Token = token;
                Duration = _settings.GetValue<int>(SettingKeys.Duration, 240);
                StartTime = endTime.AddMinutes(-Duration);
                IsTracking = true;
            }
        }

        public void Subscribe()
        {
            Debug.WriteLine("Starting subscribing tracking messages");

            if (_tokenStart != null)
            {
                Debug.WriteLine("You are trying to subscribe, already subscribed");
                return;
            }

            _tokenStart = _messenger.Subscribe<StartTrackingMessage>(m =>
            {
                Debug.WriteLine("Started tracking");
                StartTime = m.TimeStamp;
                Duration = m.Duration;
                Token = m.Token;
                IsTracking = true;
            });

            _tokenStop = _messenger.Subscribe<StopTrackingMessage>(m =>
            {
                Debug.WriteLine("Stopped tracking");
                IsTracking = false;
            });

            _tokenLocation = _messenger.Subscribe<LocationMessage>(m =>
            {
                Debug.WriteLine($"Location(WP) tracking {m.Lat} {m.Lng}");
                if (CheckIfNotExpired() && !m.IsError)
                {
                    Debug.WriteLine("Sending position");
                    _sendPositionService.SendPosition(Token, new Position(m.Lat, m.Lng), m.Address, m.Accuracy);
                }
            });
        }

        private bool CheckIfNotExpired()
        {
            if (IsTracking && DateTime.Now < EndTime)
                return true;

            if (IsTracking && DateTime.Now > EndTime)
            {
                IsTracking = false;
                Debug.WriteLine("Stopped tracking. Expired");
            }

            return false;
        }

        public void Unsubscribe()
        {
            _messenger.Unsubscribe<StartTrackingMessage>(_tokenStart);
            _messenger.Unsubscribe<LocationMessage>(_tokenLocation);
            _messenger.Unsubscribe<LocationMessage>(_tokenStop);
        }
    }
}