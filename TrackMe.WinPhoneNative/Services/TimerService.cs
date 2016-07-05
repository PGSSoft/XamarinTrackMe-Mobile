using System;
using System.Windows.Threading;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneNative.Services
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