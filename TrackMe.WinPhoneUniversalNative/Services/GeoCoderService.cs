using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using TrackMe.Core.Services.Interfaces;
using TrackMe.WinPhoneUniversalNative.Helpers;

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
            string str1 = String.Empty, str2 = String.Empty, str3 = String.Empty, str4 = String.Empty;

            List<string> line1 = new List<string>();

            if (address.BuildingRoom.IsNotNullOrEmpty())
                line1.Add(address.BuildingRoom);
            if (address.BuildingFloor.IsNotNullOrEmpty())
                line1.Add(address.BuildingFloor);
            if (address.BuildingName.IsNotNullOrEmpty())
                line1.Add(address.BuildingName);
            if (line1.Count > 0)
                str1 = string.Join(" ", line1) + Environment.NewLine;

            List<string> line2 = new List<string>();
            if (address.StreetNumber.IsNotNullOrEmpty())
                line2.Add(address.StreetNumber);
            if (address.Street.IsNotNullOrEmpty())
                line2.Add(address.Street);
            if (line2.Count > 0)
                str2 = string.Join(" ", line2) + Environment.NewLine;

            List<string> line3 = new List<string>();
            if (address.Town.IsNotNullOrEmpty())
                line3.Add(address.Town);
            if (address.Region.IsNotNullOrEmpty())
                line3.Add(address.Region);
            if (address.PostCode.IsNotNullOrEmpty())
                line3.Add(address.PostCode);
            if (line3.Count > 0)
                str3 = string.Join(" ", line3) + Environment.NewLine;

            if (!string.IsNullOrEmpty(address.Country))
                str4 = address.Country;

            return $"{str1}{str2}{str3}{str4}";
        }
    }
}