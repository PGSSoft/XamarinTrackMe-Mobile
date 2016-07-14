using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMe.Core.Services.Interfaces;
using Xamarin.Forms.Maps;

namespace TrackMe.Services
{
    public class GeoCoderService : IGeoCoderService
    {
        private readonly Geocoder _geocoder = new Geocoder();

        public async Task<IEnumerable<string>> GetAddress(double lat, double lng)
        {
            return await _geocoder.GetAddressesForPositionAsync(new Position(lat, lng));
        }
    }
}
