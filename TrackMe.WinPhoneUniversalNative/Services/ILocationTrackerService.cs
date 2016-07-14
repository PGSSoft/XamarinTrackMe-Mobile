namespace TrackMe.WinPhoneUniversalNative
{
    public interface ILocationTrackerService
    {
        void Subscribe();
        void Unsubscribe();
        bool IsTracking { get; }
    }
}