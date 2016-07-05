using System;
using CoreLocation;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.iOS;
using MvvmCross.Platform.iOS.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Location;
using TrackMe.Core;
using TrackMe.Core.Localization;
using TrackMe.Core.Services;
using TrackMe.Core.Services.Interfaces;
using TrackMe.iOS.Services;
using TrackMe.Services;
using UIKit;
using Xamarin;
using Xamarin.Forms;

namespace TrackMe.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Forms.Init();
            FormsMaps.Init();
            var xamarinFormsApp = new FormsApp();

            return new MvxFormsTouchPagePresenter(Window, xamarinFormsApp);
        }

        protected override void InitializeFirstChance()
        {
            Mvx.ConstructAndRegisterSingleton<IPageService, PageService>();
            Mvx.ConstructAndRegisterSingleton<ITimerService, TimerService>();
            Mvx.ConstructAndRegisterSingleton<IDeviceService, DeviceService>();
            Mvx.ConstructAndRegisterSingleton<IPopupService, PopupService>();
            
            Mvx.ConstructAndRegisterSingleton<ISettings, TrackMe.Services.Settings>();
            Mvx.ConstructAndRegisterSingleton<ILocalize, Localize>();

            base.InitializeFirstChance();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            Mvx.ConstructAndRegisterSingleton<IGeoCoderService, GeoCoderService>();
            Mvx.ConstructAndRegisterSingleton<IGeoCoderService, CachedGeoCoderService>();
            Mvx.ConstructAndRegisterSingleton<IShareService, ShareService>();
            Mvx.Resolve<ILocationService>().StartWatching();
        }
    }
}
