using System;
using System.Linq;
using System.Reflection;
using Android.Content;
using Android.Util;
using TrackMe.Core;
using TrackMe.Core.Services;
using TrackMe.Core.Services.Interfaces;
using TrackMe.Droid.Presenters;
using TrackMe.Droid.Services;
using TrackMe.Services;
using GeoCoderService = TrackMe.Droid.Services.GeoCoderService;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using TrackMe.Core.Localization;

namespace TrackMe.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new MvxFormsAndroidViewPresenter(new FormsApp());
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }

        protected override MvvmCross.Platform.Platform.IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeFirstChance()
        {
            Log.Info(".TrackMe", DateTime.Now + ": Setup called");
            
            CreatableTypes(typeof (FormsApp).Assembly)
                .EndingWith("Service")
                .AsInterfaces().ToList()
                .RegisterAsLazySingleton();

            Mvx.LazyConstructAndRegisterSingleton<ISendPositionService, SendPositionService>();
            Mvx.LazyConstructAndRegisterSingleton<IShareService, ShareService>();
            Mvx.LazyConstructAndRegisterSingleton<ISettings, Settings>();
            Mvx.LazyConstructAndRegisterSingleton<ILocalize, Localize>();

            base.InitializeFirstChance();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            Mvx.LazyConstructAndRegisterSingleton<IGeoCoderService, GeoCoderService>();
        }
    }
}