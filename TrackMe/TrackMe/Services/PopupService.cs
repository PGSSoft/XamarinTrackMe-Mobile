using System;
using System.Threading.Tasks;
using TrackMe.Core;
using TrackMe.Core.Localization.Resx;
using TrackMe.Core.Services.Interfaces;
using Xamarin.Forms;

namespace TrackMe.Services
{
    public class PopupService : IPopupService
    {
        private readonly IPageService _pageService;
        private Page Page => _pageService.CurrentPage;
         
        public PopupService(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task OpenBasicPopup(string message, Action afterHideCallback = null)
        {
            await Page.DisplayAlert("", message, "OK");
            afterHideCallback?.Invoke();
        }

        public async Task<bool> OpenYesNoPopup(string message, Action afterHideCallback = null)
        {
            var result = await Page.DisplayAlert(
                "",
                message,
                AppResources.Yes,
                AppResources.No);

            afterHideCallback?.Invoke();

            return result;
        }

        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action afterHideCallback = null)
        {
            var result = await Page.DisplayAlert(
                title,
                message,
                buttonConfirmText,
                buttonCancelText);

            afterHideCallback?.Invoke();

            return result;
        }
    }
}
