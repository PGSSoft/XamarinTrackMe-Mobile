using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TrackMe.WinPhoneUniversalNative
{
    public class Notifer
    {
        public static void Notify(string message)
        {
            string xml = $@"
            <toast activationType='foreground' launch='args'>
                <visual>
                    <binding template='ToastGeneric'>
                        <text>{message}</text>
                    </binding>
                </visual>
            </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            ToastNotification notification = new ToastNotification(doc);
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }
    }
}