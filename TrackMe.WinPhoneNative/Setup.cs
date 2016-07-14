using System.Linq;
using Microsoft.Phone.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.WindowsPhone.Platform;
using TrackMe.Core.Services;
using TrackMe.Core.Services.Interfaces;
using TrackMe.WinPhoneNative.Services;

namespace TrackMe.WinPhoneNative
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {

#if EMUDEBUG
            Constants.BaseUrl = @"http://169.254.80.80:3005/";
#endif

        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();

            CreatableTypes(typeof(Setup).Assembly)
                .EndingWith("Service")
                .AsInterfaces().ToList()
                .RegisterAsLazySingleton();

            Mvx.ConstructAndRegisterSingleton<IDeviceService, DeviceService>();
            Mvx.ConstructAndRegisterSingleton<IShareService, ShareService>();
            Mvx.ConstructAndRegisterSingleton<ISettings, Settings>();
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
