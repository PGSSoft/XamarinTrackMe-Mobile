using System;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneNative.Services
{
    public class PopupService : IPopupService
    {
        public Task OpenBasicPopup(string message, Action afterHideCallback = null)
        {
            var messagebox = new CustomMessageBox()
            {
                Message = message,
                RightButtonContent = "OK"
            };

            messagebox.Show();

            afterHideCallback?.Invoke();

            return Task.FromResult(false);
        }

        public Task<bool> OpenYesNoPopup(string message, Action afterHideCallback = null)
        {
            var completion = new TaskCompletionSource<bool>();
            var messagebox = new CustomMessageBox()
            {
                Message = message,
                LeftButtonContent = "Yes",
                RightButtonContent = "No"
            };
            messagebox.Dismissed += (sender, args) =>
            {
                completion.SetResult(args.Result == CustomMessageBoxResult.LeftButton);
                afterHideCallback?.Invoke();
            };

            messagebox.Show();

            return completion.Task;
        }

        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText,
            Action afterHideCallback = null)
        {
            var completion = new TaskCompletionSource<bool>();
            var messagebox = new CustomMessageBox()
            {
                Message = message,
                LeftButtonContent = buttonConfirmText,
                RightButtonContent = buttonCancelText
            };
            messagebox.Dismissed += (sender, args) =>
            {
                completion.SetResult(args.Result == CustomMessageBoxResult.LeftButton);
                afterHideCallback?.Invoke();
            };

            messagebox.Show();

            return completion.Task;
        }
    }
}