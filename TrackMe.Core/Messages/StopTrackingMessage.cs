using System;
using MvvmCross.Plugins.Messenger;

namespace TrackMe.Core.Messages
{
    public class StopTrackingMessage : MvxMessage
    {
        public bool IsAppClosed { get; set; }
        public string Token { get; set; }
        public  string PrivateToken { get; set; }
        public DateTime EndTime { get; set; }
        public StopTrackingMessage(object sender,bool isAppClosed,string token,string privateToken,DateTime endTime) : base(sender)
        {
            IsAppClosed = isAppClosed;
            Token = token;
            PrivateToken = privateToken;
            EndTime = endTime;
        }
        public StopTrackingMessage(object sender, bool isAppClosed) : base(sender)
        {
            IsAppClosed = isAppClosed;
        }
    }
}
