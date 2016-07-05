using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace TrackMe.Infrastructure
{
    public static class MvxPresenterHelpers
    {
        public static IMvxViewModel LoadViewModel(MvxViewModelRequest request)
        {
            var viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = viewModelLoader.LoadViewModel(request, null);
            return viewModel;
        }

        public static Page CreatePage(MvxViewModelRequest request)
        {
            IMvxFormsPageLoader viewPageLoader;
            Mvx.TryResolve(out viewPageLoader);
            if (viewPageLoader == null)
            {
                viewPageLoader = new MvxFormsPageLoader();
                Mvx.RegisterSingleton(viewPageLoader);
            }
            var page = viewPageLoader.LoadPage(request);
            return page;
        }
    }
}