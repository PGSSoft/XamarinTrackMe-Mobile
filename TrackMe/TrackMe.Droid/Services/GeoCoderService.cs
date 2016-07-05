using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Locations;
using TrackMe.Core.Services.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;

namespace TrackMe.Droid.Services
{
    public class GeoCoderService : IGeoCoderService
    {
        private readonly Geocoder _geocoder;

        public GeoCoderService()
        {
            var globals = Mvx.Resolve<IMvxAndroidGlobals>();
            _geocoder = new Geocoder(globals.ApplicationContext);
        }

        public async Task<IEnumerable<string>> GetAddress(double lat, double lng)
        {
            var addresses = await _geocoder.GetFromLocationAsync(lat, lng, 1);

            return addresses.Select(ConvertToString);
        }

        public async Task<IEnumerable<Address>> GetAddress2(double lat, double lng)
        {
            var globals = Mvx.Resolve<IMvxAndroidGlobals>();
            var geocoder = new Geocoder(globals.ApplicationContext);

            var addresses = await geocoder.GetFromLocationAsync(lat, lng, 1);

            return addresses.Select(Convert);
        }

        private static string ConvertToString(Android.Locations.Address address)
        {
            return address.GetAddressLine(0)+ ", " + address.Locality;
        }

        private static Address Convert(Android.Locations.Address address)
        {
            string addressLine = address.GetAddressLine(0);

            return new Address
            {
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                Name = address.FeatureName,
                Country = address.CountryName,
                PostalCode = address.PostalCode,
                Locality = address.Locality,
                SubLocality = address.SubLocality,
                Thoroughfare = address.Thoroughfare,
                SubThoroughfare = address.SubThoroughfare,
                AdministrativeArea = address.AdminArea,
                SubAdministrativeArea = address.SubAdminArea,

                AddressLine = addressLine,
            };
        }

        public class Address
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }

            public string Name { get; set; }

            public string Country { get; set; }

            public string PostalCode { get; set; }

            public string Locality { get; set; }

            public string SubLocality { get; set; }

            public string Thoroughfare { get; set; }

            public string SubThoroughfare { get; set; }

            public string AdministrativeArea { get; set; }

            public string SubAdministrativeArea { get; set; }

            public string AddressLine { get; set; }
        }
    }
}