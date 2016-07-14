namespace TrackMe.Core.Services.Interfaces
{
    public interface ILocationService
    {
        void StartWatching();
        void Stop();
        bool IsWatching { get; }
    }
}