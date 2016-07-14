using System;
using Android.Content;
using TrackMe.Core.Services.Interfaces;
using MvvmCross.Droid.Platform;

namespace TrackMe.Droid.Services
{
	public class ShareService : MvxAndroidTask, IShareService
    {
		public void Share (string message, string title = "", ShareType shareType = ShareType.Sms)
		{
			Share(message, shareType );
		}

        public void Share(string message, ShareType shareType = ShareType.Sms)
        {
            var shareIntent = new Intent(Intent.ActionSend);

            shareIntent.PutExtra(Intent.ExtraSubject, "TrackMe");
            shareIntent.PutExtra(Intent.ExtraText, message);
            shareIntent.SetType("text/plain");
            
            StartActivity(shareIntent);
        }
    }
}