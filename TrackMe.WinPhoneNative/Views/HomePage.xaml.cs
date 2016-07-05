using System;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.WindowsPhone.Views;
using TrackMe.Core.Helpers;
using TrackMe.Core.Messages;
using TrackMe.Core.ViewModels;
using TrackMe.Core.ViewModels.HomeViewModel;
using TrackMe.WinPhoneNative.Helpers;
using TrackMe.WinPhoneNative.Services;

namespace TrackMe.WinPhoneNative.Views
{
    public partial class HomePage : MvxPhonePage
    {
        private ILocationTrackerService _locationTracker;
        private MvxSubscriptionToken _locationToken;
        private MapOverlay _myLocationOverlay;

        private HomeViewModel ViewModel => DataContext as HomeViewModel;

        public HomePage()
        {
            InitializeComponent();

            if ((Visibility) Application.Current.Resources["PhoneDarkThemeVisibility"] == Visibility.Visible)
            {
                LogoLightTheme.Visibility = Visibility.Collapsed;
                LogoDarkTheme.Visibility = Visibility.Visible;
            }
            else
            {
                LogoLightTheme.Visibility = Visibility.Visible;
                LogoDarkTheme.Visibility = Visibility.Collapsed;
            }

            // for some reason InitializeComponent doesn't initialize PushPin
            AddressPushPin = MapExtensions.GetChildren(map).FirstOrDefault() as Pushpin;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _locationTracker = Mvx.Resolve<ILocationTrackerService>();
            _locationTracker.Subscribe();

            var messenger = Mvx.Resolve<IMvxMessenger>();
            _locationToken = messenger.Subscribe<LocationMessage>(message =>
            {
                if (!message.IsError)
                {
                    var coords = new GeoCoordinate(message.Lat, message.Lng) { HorizontalAccuracy = message.Accuracy ?? 0};
                    InitializeUserPositionIfNeeded(coords);
                    ShowLocation(coords);
       
                    AddressPushPin.Content = message.Address;
                    AddressPushPin.GeoCoordinate = coords;
                    if (message.Accuracy.HasValue)
                    {      
                        var coordinates = GeoHelper.CalculatePolygon(coords.Latitude, coords.Longitude, coords.HorizontalAccuracy).Select(c => c.ToNative());
                        var geoCoordsCollection = new GeoCoordinateCollection();
                        
                        foreach (var coord in coordinates)
                        {
                            geoCoordsCollection.Add(coord);
                        }
                        _mapPolygon.Path = geoCoordsCollection;
                    }
                }
            });
        }

        private bool _initialized;
        private MapPolygon _mapPolygon;

        private void InitializeUserPositionIfNeeded(GeoCoordinate coordinate)
        {
            if (_initialized)
                return;

            AddressPushPin.Visibility = Visibility.Visible;

            var border = new Border { Margin = new Thickness(25), CornerRadius = new CornerRadius(2), BorderThickness = new Thickness(1), Background = new SolidColorBrush(Colors.Transparent)};

            var myCircle = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Blue),
                Height = 20,
                Width = 20,
                Opacity = 50,
                Margin = new Thickness(20)
            };

            border.Child = myCircle;
            border.Tap += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(ViewModel.Address))
                    AddressPushPin.Visibility = Visibility.Visible;
                args.Handled = true;
            };

            map.Center = coordinate;
            map.ZoomLevel = 16;
            var horizontalShift = 0.005;
            map.SetView(new GeoCoordinate(coordinate.Latitude, coordinate.Longitude + horizontalShift), 15, MapAnimationKind.Parabolic);
            
            _myLocationOverlay = new MapOverlay
            {
                Content = border,
                PositionOrigin = new Point(0.5, 0.5),
                GeoCoordinate = coordinate,            
            };

            var myLocationLayer = new MapLayer { _myLocationOverlay };
            map.Layers.Add(myLocationLayer);

            _mapPolygon = new MapPolygon
            {
                FillColor = Color.FromArgb(100, 0, 0, 255)
            };

            map.MapElements.Add(_mapPolygon);
            _initialized = true;
        }

        private void ShowLocation(GeoCoordinate myGeoCoordinate)
        {
            _myLocationOverlay.GeoCoordinate = myGeoCoordinate;
        }

        private void ShowMe_OnClick(object sender, RoutedEventArgs e)
        {
            if (_myLocationOverlay?.GeoCoordinate != null)
            {
                map.SetView(_myLocationOverlay.GeoCoordinate, map.ZoomLevel, MapAnimationKind.Parabolic);
            }
        }

        private void Map_OnTap(object sender, GestureEventArgs e)
        {
            AddressPushPin.Visibility = Visibility.Collapsed;
        }
    }

    public static class CoordinateConverter
    {
        public static GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
        {
            return new GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }
    }
}