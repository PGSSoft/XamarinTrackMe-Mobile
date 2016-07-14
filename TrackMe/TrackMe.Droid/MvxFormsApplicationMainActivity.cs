using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Util;
using TrackMe.Core.Messages;
using TrackMe.Core.Services.Interfaces;
using TrackMe.Droid.Presenters;
using TrackMe.Droid.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Core.Views;

namespace TrackMe.Droid
{
    [Activity(Label = "MainActivity", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MvxFormsApplicationMainActivity
        : FormsAppCompatActivity
    {
        private MvxSubscriptionToken _stopToken;
        private MvxSubscriptionToken _startToken;

        protected override void OnCreate(Bundle bundle)
        {
            ToolbarResource = Resource.Layout.Toolbar;
            TabLayoutResource = Resource.Layout.TabLayout;
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            
            var listener = new MyGpslistener(this);

            try
            {
                Mvx.RegisterSingleton<IDeviceService>(() => listener);
            }
            catch (Exception exception)
            {
                // application was killed by system but it's memory wasn't fully flushed Mvx.* are throwing NullException
                // this catch is restarting app by forcing it to call OnDestroy(Finish)
                Finish();
                return;
            }

            locationManager.AddGpsStatusListener(listener);
            locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 10, 1, listener);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsAndroidViewPresenter;
            if (presenter == null) return;

            Subscribe();

            LoadApplication(presenter.MvxFormsApp);
            Mvx.Resolve<IMvxAppStart>().Start();
            LifetimeMonitor.OnCreate(this);
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(false);
        }

        private LocationManager locationManager => GetSystemService(LocationService) as LocationManager;
        private MvxAndroidLifetimeMonitor LifetimeMonitor => Mvx.Resolve<IMvxAndroidCurrentTopActivity>() as MvxAndroidLifetimeMonitor;

        private void Subscribe()
        {
            var messenger = Mvx.Resolve<IMvxMessenger>();
            Intent intent = new Intent(this, typeof(LocationTrackerService));
            
            _startToken = messenger.Subscribe<StartTrackingMessage>(message =>
            {
                intent.PutExtra("duration", message.Duration);
                intent.PutExtra("token", message.Token);
                StartService(intent);
            });

            _stopToken = messenger.Subscribe<StopTrackingMessage>(message =>
            {
                var settings = Mvx.Resolve<ISettings>();
                StopService(intent);
                
                if (message.IsAppClosed)
                {
                    if (!String.IsNullOrEmpty(message.Token) || !String.IsNullOrEmpty(message.PrivateToken))
                    {
                        settings.AddOrUpdate("Token", message.Token);
                        settings.AddOrUpdate("PrivateToken", message.PrivateToken);
                        settings.AddOrUpdate("EndTime", message.EndTime);
                    }
                }

            });
        }

        protected override void OnStart()
        {
            base.OnStart();
            LifetimeMonitor.OnStart(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            LifetimeMonitor.OnRestart(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            LifetimeMonitor.OnResume(this);
        }
        
        protected override void OnDestroy()
        {
            try
            {
                var messneger = Mvx.Resolve<IMvxMessenger>();
                messneger.Publish(new CloseAppMessage(this));

                base.OnDestroy();
                LifetimeMonitor.OnDestroy(this);
            }
            catch (Exception exceptionz)
            {
                Intent launchIntent = PackageManager.GetLaunchIntentForPackage("TrackMe.TrackMe");
                StartActivity(launchIntent); // app restart

                base.OnDestroy();
            }

            Process.KillProcess(Process.MyPid());
        }

        protected override void OnPause()
        {
            base.OnPause();
            LifetimeMonitor.OnPause(this);
        }

        protected override void OnStop()
        {
            base.OnStop();
            LifetimeMonitor.OnStop(this);
        }
    }
}