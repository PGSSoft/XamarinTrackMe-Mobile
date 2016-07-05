using System;
using Microsoft.Phone.Tasks;
using MvvmCross.Platform.WindowsPhone.Tasks;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneNative.Services
{
    public class ShareService : MvxWindowsPhoneTask, IShareService
    {
        public void Share(string message, string title="", ShareType shareType = ShareType.Sms)
        {
            if (shareType == ShareType.Sms)
            {
                var shareTask = new SmsComposeTask()
                {

                    Body = message
                };

                DoWithInvalidOperationProtection(shareTask.Show);
            }
            else if (shareType == ShareType.Email)
            {
                var emailTask = new ShareLinkTask
                {
                    Title = title,
                    LinkUri = new Uri(message)
                };
                
                DoWithInvalidOperationProtection(emailTask.Show);
            }      
        }

        public void ShareUrl(Uri url, string title)
        {
            var emailTask = new ShareLinkTask
            {
                Title = title,
                LinkUri = url
            };

            DoWithInvalidOperationProtection(emailTask.Show);
        }
    }
}
