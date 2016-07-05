namespace TrackMe.Core.Services.Interfaces
{
    public interface IShareService
    {
        void Share(string message, string title="", ShareType shareType = ShareType.Sms);
    }

    public enum ShareType
    {
        Sms, Email
    }
}
