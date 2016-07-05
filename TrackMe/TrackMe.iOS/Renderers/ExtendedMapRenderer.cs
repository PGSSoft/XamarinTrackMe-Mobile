using CoreLocation;
using MapKit;
using TrackMe.Controls;
using TrackMe.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer))]

namespace TrackMe.iOS.Renderers
{
    class ExtendedMapRenderer : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe from event handlers
            }

            if (e.NewElement != null)
            {
                // Configure the native control and subscribe to event handlers
                var nativeMap = Control as MKMapView;

            //    nativeMap.DidUpdateUserLocation += NativeMapOnDidUpdateUserLocation;
                var d = nativeMap.UserLocation;
            }

            var c = Control;
            var i = 9;
        }

        private MKMapView NativeControl => Control as MKMapView;

        private void NativeMapOnDidUpdateUserLocation(object sender, MKUserLocationEventArgs mkUserLocationEventArgs)
        {
            var location = mkUserLocationEventArgs.UserLocation.Location.Coordinate;
            
            var center = new CLLocationCoordinate2D(location.Latitude, location.Longitude);
            var region = new MKCoordinateRegion(center, new MKCoordinateSpan(0.01, 0.01));

            NativeControl.SetRegion(region, true);
        }


        class MyMKAnnotation : MKAnnotation
        {
            public override CLLocationCoordinate2D Coordinate { get; }

            
        }
    }
}
