namespace TrackMe.WinPhoneNative.Services
{
    public interface ILocationTrackerService
    {
        void Subscribe();
        void Unsubscribe();
        bool IsTracking { get; }
    }
}