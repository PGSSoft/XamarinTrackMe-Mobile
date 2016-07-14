using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Services
{
    public class PopupService : IPopupService
    {
        public async Task OpenBasicPopup(string message, Action afterHideCallback = null)
        {
            var dialog = new MessageDialog(message, "Info");
            await dialog.ShowAsync();
            afterHideCallback?.Invoke();
        }

        public async Task<bool> OpenYesNoPopup(string message, Action afterHideCallback = null)
        {
            var dialog = new MessageDialog(message, "Info");
            var tcs = new TaskCompletionSource<bool>();
       
            UICommandInvokedHandler handler = uiCommand =>
            {
                tcs.SetResult(uiCommand.Label == "Ok");
            };
            dialog.Commands.Add(new UICommand("Ok", handler));
            dialog.Commands.Add(new UICommand("Cancel", handler));
            await dialog.ShowAsync();
            afterHideCallback?.Invoke();

            return await tcs.Task;
        }



        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText,
            Action afterHideCallback = null)
        {
            var dialog = new MessageDialog(message, title);
            var tcs = new TaskCompletionSource<bool>();

            UICommandInvokedHandler handler = uiCommand =>
            {
                tcs.SetResult(uiCommand.Label == "Ok");
            };
            dialog.Commands.Add(new UICommand(buttonConfirmText, handler));
            dialog.Commands.Add(new UICommand(buttonCancelText, handler));
            await dialog.ShowAsync();
            afterHideCallback?.Invoke();

            return await tcs.Task;
        }
    }
}