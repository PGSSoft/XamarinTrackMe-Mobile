using System;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Services
{
    public class ShareService : IShareService
    {
        DataTransferManager _dataTransferManager;

        private string _title;
        private string _message;

        public ShareService()
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += (sender, args) =>
            {
                try
                {
                    DataRequest request = args.Request;
                    request.Data.Properties.Title = _title ?? Windows.ApplicationModel.Package.Current.DisplayName;
                    request.Data.SetText(_message);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("unable to share text: " + ex);
                    throw;
                }
            };
        }

        public void Share(string message, string title = "", ShareType shareType = ShareType.Sms)
        {
            _message = message;
            _title = title;
            DataTransferManager.ShowShareUI();
        }
    }
}