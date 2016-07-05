using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using TrackMe.Core.Helpers;
using TrackMe.Core.Localization.Resx;
using TrackMe.Core.Messages;
using TrackMe.Core.Models;
using TrackMe.Core.Services;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Core.ViewModels.HomeViewModel
{
    public partial class HomeViewModel : BaseViewModel
    {
        #region Services
        private readonly IDeviceService _deviceService;
        private readonly IShareService _shareService;
        private readonly ISettings _settings;
        private readonly ILocationService _locationService;
        private readonly IMvxMessenger _messenger;
        private readonly ISendPositionService _sendPositionService;
        private readonly IPopupService _popupService;
        private readonly ITimerService _timer;
        private MvxSubscriptionToken _locationMessageToken;
        private MvxSubscriptionToken _requestStartTrackingToken;
        private DateTime _timeEnd;
        #endregion

        public HomeViewModel(IDeviceService deviceService, IShareService shareService, ISettings settings,
            ILocationService locationService, IMvxMessenger messenger, ISendPositionService sendPositionService, IPopupService popupService, ITimerService timer)
        {
            _deviceService = deviceService;
            _shareService = shareService;
            _settings = settings;
            _locationService = locationService;
            _messenger = messenger;
            _sendPositionService = sendPositionService;
            _popupService = popupService;
            _timer = timer;
           
            Title = "TrackMe";
            PossibleTimes = new PossibleTimeProvider().GetPossibleTimes().ToObservable();  
            SelectedTime = new PossibleTime(240);
            
            _deviceService.LocationStatusChanged += (sender, args) =>
            {
                GpsStatus = args.Status;
                
                if (!_locationService.IsWatching && args.Status == LocationStatus.Started)
                {
                    _locationService.StartWatching();
                }
            };
            GpsStatus = _deviceService.LocationStatus;
            _locationMessageToken = _messenger.Subscribe<LocationMessage>(GotLocation);
            _requestStartTrackingToken = _messenger.Subscribe<RequestStartTrackingMessage>(OnRequestStartTracking);
            _messenger.Subscribe<CloseAppMessage>(message =>
            {
                _messenger.Publish(new StopTrackingMessage(this, true, Token, PrivateToken, _timeEnd));
            });

            TrackConnectionStatus();
        }
        
        public async Task Init(StartType startType)
        {
            var restored = await RestoreState();
            if (!restored && startType == StartType.Voice)
            {
              await DoStartTrackingCommand();
              Debug.WriteLine("Tracking voice activated - Init");
            }
        }
        
        private double? _lastAccuracy;
        private void GotLocation(LocationMessage locationMessage)
        {
            Dispatcher.RequestMainThreadAction(() =>
            {
                Position = new Position(locationMessage.Lat, locationMessage.Lng);
            });
            
            Address = locationMessage.Address;
            _lastAccuracy = locationMessage.Accuracy;
        }

        #region Position, SelectedTime, PossibleTime
        public ObservableCollection<PossibleTime> PossibleTimes { get; set; }

        private PossibleTime _selectedTime;
        public PossibleTime SelectedTime
        {
            get { return _selectedTime; }
            set { _selectedTime = value; RaisePropertyChanged(() => SelectedTime); StartTrackingCommand.RaiseCanExecuteChanged(); }
        }

        private Position _position = Constants.DefaultPosition;
        public Position Position
        {
            get { return _position; }
            set
            {
                SetProperty(ref _position, value);
                StartTrackingCommand.RaiseCanExecuteChanged();
                RefreshGpsStatusText();
            }
        }
        #endregion

        #region UserInfo - address & tokens
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; RaisePropertyChanged(() => Address); }
        }

        private string _token;
        public string Token
        {
            get { return _token; }
            set { _token = value; RaisePropertyChanged(() => Token); ShareCommand.RaiseCanExecuteChanged(); }
        }

        private string _privateToken;
        public string PrivateToken
        {
            get { return _privateToken; }
            set { _privateToken = value; RaisePropertyChanged(() => PrivateToken); }
        }
        #endregion

        private TimeSpan _timeLeft;
        public TimeSpan TimeLeft
        { 
            get { return _timeLeft; }
            set { _timeLeft = value; RaisePropertyChanged(() => TimeLeft); }
        }

        #region Tracking
        private bool _isTracking;
        public bool IsTracking
        {
            get { return _isTracking; }
            set
            {
                _isTracking = value; RaisePropertyChanged(() => IsTracking);
                StartTrackingCommand.RaiseCanExecuteChanged();
                StopTrackingCommand.RaiseCanExecuteChanged();
                ShareCommand.RaiseCanExecuteChanged();
            }
        }
        private async void OnRequestStartTracking(RequestStartTrackingMessage requestStartTrackingMessage)
        {
            if (!IsTracking)
            {
                await DoStartTrackingCommand();
                Debug.WriteLine("Tracking voice activated - Message");
            }
        }


        public async Task<bool> CheckConditions()
        {
            bool returnValue = true;
            if (!_deviceService.IsInternetAvailable())
            {
                await _popupService.OpenBasicPopup(AppResources.NoInternetEnabled);
                returnValue = false;
            }

            if (!Mvx.Resolve<IDeviceService>().IsGpsEnabled() && returnValue)
            {
                await Mvx.Resolve<IPopupService>().OpenBasicPopup(AppResources.NoGpsEnabled);
                returnValue = false;
            }

            return returnValue;
        }

        private async Task DoStartTrackingCommand(bool restore = false)
        {
            try
            {
                IsBusy = true;

                IsTracking = true;
                if (!restore)
                {
                    if (!await CheckConditions())
                    {
                        IsBusy = IsTracking = false;
                        return;
                    }
                   
                    var selectedTime = TimeSpan.FromMinutes(SelectedTime.Minutes);
                    _timeEnd = DateTime.Now + selectedTime;
                    var result = await _sendPositionService.GetToken(Position, SelectedTime.Minutes, Address, _lastAccuracy);
                    IsBusy = false;
                    if (!result.IsError)
                    {
                        Token = result.Result.PublicToken;
                        PrivateToken = result.Result.PrivateToken;
                        StartCountdown();

                        SaveCurrentState();

                        _messenger.Publish(new StartTrackingMessage(this, Token, SelectedTime.Minutes));
                    }
                    else
                    {
                        await PresentErrorAndStopTracking(result.ErrorMessage);
                    }
                }
                else
                {
                    StartCountdown();
                    int duration = (_timeEnd - DateTime.Now).Minutes;
                    _messenger.Publish(new StartTrackingMessage(this, Token, duration)); 
                }
            }
            catch (Exception e)
            {
#if DEBUG
                await PresentErrorAndStopTracking(e.Message);
#else
                await PresentErrorAndStopTracking(AppResources.ShareStartError);
#endif
            }
            IsBusy = false;
        }

        private void StartCountdown()
        {
            _timer.Start(TimeSpan.FromSeconds(1), () =>
            {
                if (!IsTracking)
                {

                    return false;
                }
                
                TimeLeft = _timeEnd - DateTime.Now;

                if (TimeLeft < TimeSpan.FromSeconds(0))
                {
                    StopTracking();
                }

                return IsTracking;
            });
        }

        private async Task StopTracking()
        {
            var requestResoult = await _sendPositionService.StopSession(PrivateToken);
            
            if (requestResoult.IsError)
            {
                _popupService.OpenBasicPopup(requestResoult.ErrorMessage);
                return;
            }
            IsTracking = false;
            _messenger.Publish(new StopTrackingMessage(this,false));
         
                TimeLeft = TimeSpan.Zero;

                Token = PrivateToken = string.Empty;
                _settings.AddOrUpdate("Token", "");
                _settings.AddOrUpdate("PrivateToken", "");
        }
        #endregion

        #region Save/Restore state
        private void SaveCurrentState()
        {
            _settings.AddOrUpdate(SettingKeys.Token, Token);
            _settings.AddOrUpdate(SettingKeys.PrivateToken, PrivateToken);
            _settings.AddOrUpdate(SettingKeys.EndTime, _timeEnd);
            _settings.AddOrUpdate(SettingKeys.Duration, SelectedTime.Minutes);
        }

        private async Task<bool> RestoreState()
        {
            string token = _settings.GetValue<string>(SettingKeys.Token);
            string privateToken = _settings.GetValue<string>(SettingKeys.PrivateToken);
            DateTime endTime = _settings.GetValue<DateTime>(SettingKeys.EndTime, DateTime.MinValue); //SettingKeys.EndTime
             var shouldRestore = (endTime - DateTime.Now) > TimeSpan.Zero;
            if (!string.IsNullOrEmpty(token) && shouldRestore)
            {
                Token = token;
                PrivateToken = privateToken;
                _timeEnd = endTime;
                await DoStartTrackingCommand(true);
                return true;
            }
            
            return false;
        }
        #endregion
    }
}
