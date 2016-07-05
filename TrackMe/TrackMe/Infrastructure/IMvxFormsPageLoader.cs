using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace TrackMe.Infrastructure
{
    public interface IMvxFormsPageLoader
    {
        Page LoadPage(MvxViewModelRequest request);
    }
}