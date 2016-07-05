using MvvmCross.Plugins.Messenger;

namespace TrackMe.Core.Messages
{
    public class CloseAppMessage:MvxMessage
    {
        public CloseAppMessage(object sender) : base(sender)
        {
        }
    }
}
