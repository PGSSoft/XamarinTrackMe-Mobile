using System;

namespace TrackMe.Core.Services.Interfaces
{
    public interface IDeviceService
    {
        bool IsGpsEnabled();
        LocationStatus LocationStatus { get; }
        event EventHandler<LocationStatusChangedEventArgs> LocationStatusChanged;
        bool StartLocationStatusWatching();
        bool StopLocationStatusWatching();
        bool IsInternetAvailable();
    }

    public enum LocationStatus
    {
        Stopped,
        Initializing,
        Started
    }

    public class LocationStatusChangedEventArgs
    {
        public LocationStatus Status { get; set; }
    }
}
