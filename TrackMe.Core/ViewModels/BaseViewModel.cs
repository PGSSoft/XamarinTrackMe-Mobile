using MvvmCross.Core.ViewModels;

namespace TrackMe.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        private string _title;
        public string Title
        { 
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        private bool _isBusy;
        public bool IsBusy
        { 
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }
    }
}
