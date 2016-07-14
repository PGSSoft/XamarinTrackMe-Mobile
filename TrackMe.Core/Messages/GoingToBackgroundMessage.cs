using MvvmCross.Plugins.Messenger;

namespace TrackMe.Core.Messages
{
    public class GoingToBackgroundMessage : MvxMessage
    {
        public GoingToBackgroundMessage(object sender) : base(sender)
        {
        }
    }
}
