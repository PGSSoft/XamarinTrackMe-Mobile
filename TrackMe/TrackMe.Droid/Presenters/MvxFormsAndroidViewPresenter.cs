using TrackMe.Infrastructure;
using MvvmCross.Droid.Views;


namespace TrackMe.Droid.Presenters
{
    public class MvxFormsAndroidViewPresenter
        : MvxFormsBaseViewPresenter
        , IMvxAndroidViewPresenter
    {
        public MvxFormsAndroidViewPresenter(FormsApp mvxFormsApp)
            : base(mvxFormsApp)
        {
        }
    }


}