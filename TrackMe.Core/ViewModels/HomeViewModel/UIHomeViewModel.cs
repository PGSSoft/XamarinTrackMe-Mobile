using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using TrackMe.Core.Localization.Resx;
using TrackMe.Core.Messages;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Core.ViewModels.HomeViewModel
{
    public partial class HomeViewModel
    {
        #region TextProperties
        public string ShareString => AppResources.ShareButton;
        public string StopSharing => AppResources.StopTrackingButton;
        #endregion

        #region Status        
        public bool ConnectionsStatus { get; set; }
        public bool IsInternetEnabled => _deviceService.IsInternetAvailable();

        private Task _trackConnectionTask;
        private async void TrackConnectionStatus()
        {
            // bool lastStatus = true;
            bool lastInternetStatus = true;
            var lastGpsStatus = LocationStatus.Started;

            _trackConnectionTask = new Task(async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);

                    if (lastInternetStatus != IsInternetEnabled)
                    {
                        Dispatcher.RequestMainThreadAction(async () =>
                        {
                            RaisePropertyChanged(() => IsInternetEnabled);

                            ShareCommand.RaiseCanExecuteChanged();

                            if (!IsInternetEnabled)
                                await _popupService.OpenBasicPopup(AppResources.NoInternetEnabled);
                        });
                        lastInternetStatus = IsInternetEnabled;
                    }

                    if (lastGpsStatus != GpsStatus)
                    {
                        Dispatcher.RequestMainThreadAction(async () =>
                        {
                            ShareCommand.RaiseCanExecuteChanged();

                            if (GpsStatus != LocationStatus.Started)
                                await _popupService.OpenBasicPopup(AppResources.NoGpsEnabled);
                        });
                        lastGpsStatus = GpsStatus;
                    }

                    ConnectionsStatus = IsInternetEnabled && GpsStatus == LocationStatus.Started;
                    RaisePropertyChanged(() => ConnectionsStatus);
                }

            }, TaskCreationOptions.LongRunning);
            _trackConnectionTask.Start();
        }

        private LocationStatus _gpsStatus;
        public LocationStatus GpsStatus
        {
            get { return _gpsStatus; }
            set { _gpsStatus = value; RaisePropertyChanged(() => GpsStatus); RefreshGpsStatusText(); }
        }

        private string _gpsStatusText;
        public string GpsStatusText
        {
            get { return _gpsStatusText; }
            set { _gpsStatusText = value; RaisePropertyChanged(() => GpsStatusText); }
        }

        private void RefreshGpsStatusText()
        {
            if (GpsStatus == LocationStatus.Stopped)
                GpsStatusText = AppResources.GPSTurnedOff;
            else if (GpsStatus == LocationStatus.Initializing || Position == null || Position.Longitude == -1)
                GpsStatusText = AppResources.GPSSearching;
            else
                GpsStatusText = string.Empty;
        }
        #endregion

        #region Commands
        private MvxCommand _shareCommand;

        public MvxCommand ShareCommand
        {
            get
            {
                _shareCommand = _shareCommand ?? new MvxCommand(DoShareCommand, () =>
                {
                    if (!IsInternetEnabled)
                        return false;

                    if (GpsStatus == LocationStatus.Stopped)
                        return false;

                    return Position != null;
                });
                return _shareCommand;
            }
        }

        private async void DoShareCommand()
        {
            if (!IsTracking)
                await DoStartTrackingCommand();
            if (!string.IsNullOrEmpty(Token))
            {
                _shareService.Share(string.Format(Constants.BrowseUrl, Token), AppResources.MailContent, ShareType.Email);
            }
        }

        private MvxCommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                _refreshCommand = _refreshCommand ?? new MvxCommand(DoRefreshCommand);
                return _refreshCommand;
            }
        }

        private void DoRefreshCommand()
        {
            _messenger.Publish(new StopTrackingMessage(this, false));
            Token = "";
        }

        private MvxCommand<bool> _startTrackingCommand;
        public MvxCommand<bool> StartTrackingCommand
        {
            get
            {
                _startTrackingCommand = _startTrackingCommand ?? new MvxCommand<bool>(async b => await DoStartTrackingCommand(b),
                    CanStartTrackingCommand);
                return _startTrackingCommand;
            }
        }
        private bool CanStartTrackingCommand(bool restore)
        {
            var res = !IsTracking && Position != null && SelectedTime != null;
            return res;
        }

        private MvxCommand _stopTrackingCommand;
        public MvxCommand StopTrackingCommand
        {
            get
            {
                _stopTrackingCommand = _stopTrackingCommand ?? new MvxCommand(DoStopTrackingCommand, () => IsTracking);
                return _stopTrackingCommand;
            }
        }
        #endregion

        private async void DoStopTrackingCommand()
        {
            if (await _popupService.OpenYesNoPopup(AppResources.StopSharingQuestion))
            {
                IsBusy = true;
                await StopTracking();
                IsBusy = false;
            }
        }
        
        private async Task PresentErrorAndStopTracking(string message)
        {
            IsTracking = false;
            await _popupService.OpenBasicPopup(message);
        }
        

    }
}