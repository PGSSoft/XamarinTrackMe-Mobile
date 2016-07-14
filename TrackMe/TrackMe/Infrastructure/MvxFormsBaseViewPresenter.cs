using System;
using System.Threading.Tasks;
using TrackMe.Views;
using Xamarin.Forms;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;

namespace TrackMe.Infrastructure
{
    public abstract class MvxFormsBaseViewPresenter : IMvxViewPresenter
    {
        protected Application _mvxFormsApp;

        public Application MvxFormsApp
        {
            get { return _mvxFormsApp; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("MvxFormsApp cannot be null");
                }
                _mvxFormsApp = value;
            }
        }

        public MvxFormsBaseViewPresenter()
        { }

        public MvxFormsBaseViewPresenter(Application mvxFormsApp)
        {
            _mvxFormsApp = mvxFormsApp;
        }

        public async void ChangePresentation(MvxPresentationHint hint)
        {
         //   if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                var mainPage = _mvxFormsApp.MainPage as NavigationPage;

                if (mainPage == null)
                {
                    Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Application loading error");
                }
                else
                {
                    await mainPage.PopAsync();
                }
            }
        }

        public async void Show(MvxViewModelRequest request)
        {
            if (await TryShowPage(request))
                return;

            Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
        }

        protected virtual void CustomPlatformInitialization(NavigationPage mainPage)
        {
        }

        private async Task<bool> TryShowPage(MvxViewModelRequest request)
        {
            var page = MvxPresenterHelpers.CreatePage(request);
            if (page == null)
                return false;

            var viewModel = MvxPresenterHelpers.LoadViewModel(request);

            var mainPage = _mvxFormsApp.MainPage as NavigationPage;
            page.BindingContext = viewModel;

            if (mainPage == null)
            {
                _mvxFormsApp.MainPage = new CustomNavigationPage(page);
                mainPage = _mvxFormsApp.MainPage as NavigationPage;
                CustomPlatformInitialization(mainPage);
            }
            else
            {
                try
                {
                    await mainPage.PushAsync(page);
                }
                catch (Exception e)
                {
                    Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
                }
            }

            return true;
        }

        public void AddPresentationHintHandler<THint>(Func<THint, bool> action) where THint : MvxPresentationHint
        {
            throw new NotImplementedException();
        }
    }
}