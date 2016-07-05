using System;
using TrackMe.Core.Services.Interfaces;
using Xamarin.Forms;

namespace TrackMe.Services
{
    public class TimerService : ITimerService
    {
        public void Start(TimeSpan interval, Func<bool> callback)
        {
            Device.StartTimer(interval, callback);
        }
    }
}
