using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.Messenger;
using TrackMe.Core.Messages;
using TrackMe.Core.Services;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Core
{
    public class App : MvxApplication
    {
        private MvxSubscriptionToken _goingToBackgroundToken;
        private MvxSubscriptionToken _resumingToken;

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.ConstructAndRegisterSingleton<ISendPositionService, SendPositionService>();

            RegisterAppStart(new AppStart());

            var messenger = Mvx.Resolve<IMvxMessenger>();
            _goingToBackgroundToken = messenger.Subscribe<GoingToBackgroundMessage>(GoinngToBackground);
            _resumingToken = messenger.Subscribe<ResumingApplicationMessage>(Resuming);
        }

        private void Resuming(ResumingApplicationMessage resumingApplicationMessage)
        {
           // Mvx.Resolve<ILocationService>().StartWatching();
        }

        private void GoinngToBackground(GoingToBackgroundMessage goingToBackgroundMessage)
        {
           // Mvx.Resolve<ILocationService>().Stop();
        }
    }

    public enum StartType
    {
        Normal,
        Voice
    }
}
