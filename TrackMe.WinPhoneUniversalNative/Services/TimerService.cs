using System;
using Windows.UI.Xaml;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Services
{
    public class TimerService : ITimerService
    {
        public void Start(TimeSpan interval, Func<bool> callback)
        {
            var timer = new DispatcherTimer();
            timer.Interval = interval;
            timer.Tick += (sender, args) =>
            {
                var shouldContinue = callback();
                if (!shouldContinue)
                {
                    timer.Stop();
                }
            };
            timer.Start();
        }
    }
}