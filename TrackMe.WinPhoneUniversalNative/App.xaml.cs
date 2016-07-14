using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using TrackMe.Core;
using TrackMe.Core.Messages;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private MvxSubscriptionToken _stopToken;
        private Frame _rootFrame;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            Resuming += OnResuming;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var storageFile =
                await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///TalkCommands.xml"));
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(storageFile);

            _rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (_rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                _rootFrame = new Frame();

                _rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = _rootFrame;
            }

            InitRootFrame();
        }

        private void InitRootFrame(StartType startType = StartType.Normal)
        {
            if (_rootFrame == null)
            {
                _rootFrame = new Frame();
                _rootFrame.NavigationFailed += OnNavigationFailed;
                Window.Current.Content = _rootFrame;
            }

            if (_rootFrame.Content == null)
            {
                var setup = new Setup(_rootFrame);
                setup.Initialize();
                Mvx.Resolve<IDeviceService>().StartLocationStatusWatching();
                Mvx.Resolve<IMvxAppStart>().Start(startType);

                Window.Current.Activate();

                _stopToken = Mvx.Resolve<IMvxMessenger>().Subscribe<StopTrackingMessage>(m =>
                {
                    if (IsRunningInBackground)
                    {
                        StopWatching();
                    }

                    Debug.WriteLine("App: StopTracking");
                });
            }

            StartWatching();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            Debug.WriteLine("App: OnSuspending");
            var deferral = e.SuspendingOperation.GetDeferral();
            IsRunningInBackground = true;

            try // trowns NPE if turn off too fast
            {
                Mvx.Resolve<IMvxMessenger>().Publish(new GoingToBackgroundMessage(this));
            }
            catch (Exception x)
            {
                
            }
            if (!Mvx.Resolve<ILocationTrackerService>().IsTracking)
                StopWatching();
            
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            var messenger = Mvx.Resolve<IMvxMessenger>();
            if (args.Kind == ActivationKind.VoiceCommand)
            {    
                var commandArgs = args as VoiceCommandActivatedEventArgs;
                var startType = StartType.Normal;

                if (commandArgs != null && commandArgs.Result.Text == "share my location?")
                {
                    startType = StartType.Voice;                            
                    messenger.Publish(new RequestStartTrackingMessage(this));
                }

                InitRootFrame(startType);
            }

            IsRunningInBackground = false;
            messenger.Publish(new ResumingApplicationMessage(this));

            StartWatching();
            Debug.WriteLine("App OnActivated");
        }

        private void OnResuming(object sender, object o)
        {
            IsRunningInBackground = false;
            Mvx.Resolve<IMvxMessenger>().Publish(new ResumingApplicationMessage(this));

            StartWatching();

            Debug.WriteLine("App OnResuming");
        }

        public bool IsRunningInBackground { get; set; }

        private void StartWatching()
        {
            Debug.WriteLine("StartWatching");
            Mvx.Resolve<ILocationService>().StartWatching();
            Mvx.Resolve<IDeviceService>().StartLocationStatusWatching();
        }

        private void StopWatching()
        {
            Debug.WriteLine("StopWatching");
            Mvx.Resolve<ILocationService>().Stop();
            Mvx.Resolve<IDeviceService>().StopLocationStatusWatching();
        }
    }
}
