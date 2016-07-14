using MvvmCross.iOS.Views.Presenters;
using TrackMe.Infrastructure;
using UIKit;
using Xamarin.Forms;

namespace TrackMe.iOS
{
    public class MvxFormsTouchPagePresenter
        : MvxFormsBaseViewPresenter
            , IMvxIosViewPresenter
    {
        private readonly UIWindow _window;

        public MvxFormsTouchPagePresenter(UIWindow window, Xamarin.Forms.Application mvxFormsApp)
            : base(mvxFormsApp)
        {
            _window = window;
        }

        public virtual bool PresentModalViewController(UIViewController controller, bool animated)
        {
            _window.RootViewController.PresentModalViewController(controller, animated);
            return true;
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
        }

        protected override void CustomPlatformInitialization(NavigationPage mainPage)
        {
            _window.RootViewController = mainPage.CreateViewController();
        }
    }
}