using System;
using System.Threading.Tasks;

namespace TrackMe.Core.Services.Interfaces
{
    public interface IPopupService
    {
        Task OpenBasicPopup(string message, Action afterHideCallback = null);
        Task<bool> OpenYesNoPopup(string message, Action afterHideCallback = null);
        Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action afterHideCallback = null);
    }
}
