using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Services
{
    public class GeoCoderService : IGeoCoderService
    {
        public async Task<IEnumerable<string>> GetAddress(double lat, double lng)
        {
            var addr = new List<string>();
            var result = await MapLocationFinder.FindLocationsAtAsync(new Geopoint(new BasicGeoposition() { Latitude = lat, Longitude = lng}));
            if (result.Status == MapLocationFinderStatus.Success)
            {
                addr.Add(AddressToString(result.Locations[0].Address));
            }
            return addr;
        }

        private static string AddressToString(MapAddress address)
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            List<string> line1 = new List<string>();
            if (!string.IsNullOrEmpty(address.BuildingRoom))
                line1.Add(address.BuildingRoom);
            if (!string.IsNullOrEmpty(address.BuildingFloor))
                line1.Add(address.BuildingFloor);
            if (!string.IsNullOrEmpty(address.BuildingName))
                line1.Add(address.BuildingName);
/*            if (!string.IsNullOrEmpty(address.BuildingZone))
                line1.Add(address.BuildingZone);*/
            if (line1.Count > 0)
                str1 = string.Join(" ", line1) + Environment.NewLine;

            List<string> line2 = new List<string>();
            if (!string.IsNullOrEmpty(address.StreetNumber))
                line2.Add(address.StreetNumber);
            if (!string.IsNullOrEmpty(address.Street))
                line2.Add(address.Street);
            if (line2.Count > 0)
                str2 = string.Join(" ", line2) + Environment.NewLine;

            List<string> line3 = new List<string>();
            if (!string.IsNullOrEmpty(address.Town))
                line3.Add(address.Town);
            if (!string.IsNullOrEmpty(address.Region))
                line3.Add(address.Region);
/*            else if (!string.IsNullOrEmpty(address.))
                line3.Add(address.Province);*/
            if (!string.IsNullOrEmpty(address.PostCode))
                line3.Add(address.PostCode);
            if (line3.Count > 0)
                str3 = string.Join(" ", line3) + Environment.NewLine;

            if (!string.IsNullOrEmpty(address.Country))
                str4 = address.Country;

            return $"{str1}{str2}{str3}{str4}";
        }
    }
}