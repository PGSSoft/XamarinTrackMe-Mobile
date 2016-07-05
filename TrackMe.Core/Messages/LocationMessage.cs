using MvvmCross.Plugins.Messenger;

namespace TrackMe.Core.Messages
{
    public class LocationMessage : MvxMessage
    {
        public double Lat { get; private set; }
        public double Lng { get; private set; }
        public double? Accuracy { get; set; }
        public string Address { get; set; }

        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

        public LocationMessage(object sender, double lat, double lng, bool isError = false, string errorMessage = "") : base(sender)
        {
            Lat = lat;
            Lng = lng;
            IsError = isError;
            ErrorMessage = errorMessage;
        }
    }
}
