using System.Diagnostics;
using MessageUI;
using MvvmCross.Platform.iOS.Views;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.iOS.Services
{
    public class ShareService : IShareService
    {
        private readonly IMvxIosModalHost _modalHost;

        public ShareService(IMvxIosModalHost modelHost)
        {
            _modalHost = modelHost;

        }

        public void Share(string message, string title = "", ShareType shareType = ShareType.Sms)
        {
/*          if (MFMessageComposeViewController.CanSendText)
            {
                var smsCtrl = new MFMessageComposeViewController();
                smsCtrl.Body = message;
                smsCtrl.Subject = title;
                smsCtrl.Finished += (sender, args) =>
                {
                    Debug.WriteLine("finished");
                };

                _modalHost.PresentModalViewController(smsCtrl, true);
            }*/

            if (MFMailComposeViewController.CanSendMail)
            {
                var mail = new MFMailComposeViewController();
                mail.SetSubject(title);
               // mail.SetToRecipients(new[] { "aa@0x0x0.com" });
                mail.SetMessageBody(message, false);

                mail.Finished += (sender, args) =>
                {
                    Debug.WriteLine("Finished");
                    args.Controller.DismissViewController(true, null);
                };

                _modalHost.PresentModalViewController(mail, true);
            }
        }
    }
}
