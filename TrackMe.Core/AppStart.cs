using MvvmCross.Core.ViewModels;
using TrackMe.Core.ViewModels;
using TrackMe.Core.ViewModels.HomeViewModel;

namespace TrackMe.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public void Start(object startType = null)
        {
            if (startType is StartType)
            {
                var type = (StartType)startType;
                ShowViewModel<HomeViewModel>(new { startType = type });
                return;
            }
            ShowViewModel<HomeViewModel>(new { startType = StartType.Normal });
        }
    }
}