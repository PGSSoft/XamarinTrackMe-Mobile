using Windows.UI.Xaml.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.WindowsUWP.Platform;
using MvvmCross.WindowsUWP.Views;
using TrackMe.Core;
using TrackMe.Core.Services;
using TrackMe.Core.Services.Interfaces;
using TrackMe.WinPhoneUniversalNative.Services;

namespace TrackMe.WinPhoneUniversalNative
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame, string suspensionManagerSessionStateKey = null) : base(rootFrame, suspensionManagerSessionStateKey)
        {
        }

        public Setup(IMvxWindowsFrame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
#if EMUDEBUG
                    Constants.BaseUrl = @"http://169.254.80.80:3005/";
#endif
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();
            //MvvmCross.Plugins.Location.PluginLoader.Instance.EnsureLoaded();

/*            CreatableTypes(typeof(Setup).Assembly)
                .EndingWith("Service")
                .AsInterfaces().ToList()
                .RegisterAsLazySingleton();*/

            Mvx.ConstructAndRegisterSingleton<IDeviceService, DeviceService>();
            Mvx.ConstructAndRegisterSingleton<IShareService, ShareService>();
            Mvx.ConstructAndRegisterSingleton<ISettings, Settings>();

            Mvx.ConstructAndRegisterSingleton<ITimerService, TimerService>();
            Mvx.ConstructAndRegisterSingleton<IPopupService, PopupService>();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            Mvx.ConstructAndRegisterSingleton<IGeoCoderService, GeoCoderService>();
            Mvx.ConstructAndRegisterSingleton<IGeoCoderService, CachedGeoCoderService>();
            Mvx.ConstructAndRegisterSingleton<ILocationTrackerService, LocationTrackerService>();
        }
    }
}
