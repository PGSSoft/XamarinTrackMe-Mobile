using System;
using MvvmCross.Plugins.Messenger;

namespace TrackMe.Core.Messages
{
    public class StartTrackingMessage : MvxMessage
    {
        public int Duration { get; private set; }
        public string Token { get; private set; }
        public DateTime TimeStamp { get; set; }

        public StartTrackingMessage(object sender, string token, int duration) : base(sender)
        {
            Token = token;
            Duration = duration;
            TimeStamp = DateTime.Now;           
        }
    }
}