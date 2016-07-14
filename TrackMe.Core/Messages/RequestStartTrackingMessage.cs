using MvvmCross.Plugins.Messenger;

namespace TrackMe.Core.Messages
{
    public class RequestStartTrackingMessage : MvxMessage
    {
        public RequestStartTrackingMessage(object sender) : base(sender)
        {
        }
    }
}