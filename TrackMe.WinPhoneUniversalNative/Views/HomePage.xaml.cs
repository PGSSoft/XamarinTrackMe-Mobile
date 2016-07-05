using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.WindowsUWP.Views;
using TrackMe.Core.Helpers;
using TrackMe.Core.Messages;
using TrackMe.Core.ViewModels;
using TrackMe.Core.ViewModels.HomeViewModel;
using TrackMe.WinPhoneUniversalNative.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackMe.WinPhoneUniversalNative.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : MvxWindowsPage
    {
        private MvxSubscriptionToken _locationToken;
        private bool _initialized;
        private ILocationTrackerService _locationTracker;
        private MapPolygon _accuracyPolygon;

        public HomePage()
        {
            App.Current.Resources["PositionOrigin"] = new Point(0.5, 0.5);
            this.InitializeComponent();
            Map.MapServiceToken = Constants.MapServiceToken;
            var messenger = Mvx.Resolve<IMvxMessenger>();

            UserInfo.Tapped += (sender, args) =>
            {
                Debug.WriteLine("position tapped");
                args.Handled = true;
            };

            _locationToken = messenger.Subscribe<LocationMessage>(async message =>
            {
                if (!message.IsError)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        var geoPoint = new Geopoint(new BasicGeoposition { Latitude = message.Lat, Longitude = message.Lng });
                        await InitializeUserPositionIfNeeded(geoPoint, message.Accuracy);

                        var vals = GeoHelper.CalculatePolygon(message.Lat, message.Lng, message.Accuracy ?? 0).Select(p => p.ToNative().Position);
                        _accuracyPolygon.Path = new Geopath(vals);
                    });
                }
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _locationTracker = Mvx.Resolve<ILocationTrackerService>();
            _locationTracker.Subscribe();
        }

        private async Task InitializeUserPositionIfNeeded(Geopoint point, double? accuracy)
        {
            if (_initialized)
                return;

            _accuracyPolygon = new MapPolygon();
            _accuracyPolygon.FillColor = Color.FromArgb(50, 0, 0, 250); 
            _accuracyPolygon.Path = new Geopath(new[]{ new BasicGeoposition(), });
            Map.MapElements.Add(_accuracyPolygon);

            // center the view
            Map.Center = point;
            Map.ZoomLevel = 18;
            var horizontalShift = 0.005;
            await Map.TrySetViewAsync(point, Map.ZoomLevel, null, null, MapAnimationKind.Bow);

           _initialized = true;
        }

        private void Map_OnMapTapped(MapControl sender, MapInputEventArgs args)
        {
             UserInfo.ToolTipVisibility = Visibility.Collapsed;
        }

        private async void CenterButton_OnClick(object sender, RoutedEventArgs e)
        {
            var point = Vm.Position.ToNative();
            await Map.TrySetViewAsync(point, Map.ZoomLevel, null, null, MapAnimationKind.Bow);
        }

        private HomeViewModel Vm => ViewModel as HomeViewModel;
    }
}
