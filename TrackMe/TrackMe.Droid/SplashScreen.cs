using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Xamarin.Forms;
using MvvmCross.Droid.Views;

namespace TrackMe.Droid
{
    [Activity(
        Label = "Track my location"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
            Log.Info(".TrackMe", DateTime.Now + ": SplashScreen ctor called");
        }
        
        public override void InitializationComplete()
        {
            StartActivity(typeof(MvxFormsApplicationMainActivity));
        }
        
        protected override void OnCreate(Bundle bundle)
        {
            Log.Info(".TrackMe", DateTime.Now + ": SplashScreen OnCreate called");
            
            if (!Forms.IsInitialized)
            {
                Forms.Init(this, bundle);
                // Leverage controls' StyleId attrib. to Xamarin.UITest
                Forms.ViewInitialized += (sender, e) =>
                {
                    if (!String.IsNullOrWhiteSpace(e.View.StyleId))
                    {
                        e.NativeView.ContentDescription = e.View.StyleId;
                    }
                };
            }
            base.OnCreate(bundle);
        }
    }
}