using System;

namespace TrackMe.Core.Services.Interfaces
{
    public interface ITimerService
    {
        void Start(TimeSpan interval, Func<bool> callback);
    }
}
