using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Services;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneNative.Services
{
    public class GeoCoderService : IGeoCoderService
    {
        public Task<IEnumerable<string>> GetAddress(double lat, double lng)
        {
            var taskCompletion = new TaskCompletionSource<IEnumerable<string>>();
            EventHandler<QueryCompletedEventArgs<IList<MapLocation>>> handler = null;
            var reverseGeocode = new ReverseGeocodeQuery();
            reverseGeocode.GeoCoordinate = new GeoCoordinate(lat, lng);

            handler = (s, e) =>
            {
                reverseGeocode.QueryCompleted -= handler;
                if (e.Cancelled)
                    taskCompletion.SetCanceled();
                else if (e.Error != null)
                    taskCompletion.SetException(e.Error);
                else
                {
                    var addresses = e.Result.Select(m => AddressToString(m.Information.Address));
                    taskCompletion.SetResult(addresses);
                }
            };

            reverseGeocode.QueryCompleted += handler;
            reverseGeocode.QueryAsync();

            return taskCompletion.Task;
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
            if (!string.IsNullOrEmpty(address.BuildingZone))
                line1.Add(address.BuildingZone);
            if (line1.Count > 0)
                str1 = string.Join(" ", line1) + Environment.NewLine;

            List<string> line2 = new List<string>();
            if (!string.IsNullOrEmpty(address.HouseNumber))
                line2.Add(address.HouseNumber);
            if (!string.IsNullOrEmpty(address.Street))
                line2.Add(address.Street);
            if (line2.Count > 0)
                str2 = string.Join(" ", line2) + Environment.NewLine;

            List<string> line3 = new List<string>();
            if (!string.IsNullOrEmpty(address.City))
                line3.Add(address.City);
            if (!string.IsNullOrEmpty(address.State))
                line3.Add(address.State);
            else if (!string.IsNullOrEmpty(address.Province))
                line3.Add(address.Province);
            if (!string.IsNullOrEmpty(address.PostalCode))
                line3.Add(address.PostalCode);
            if (line3.Count > 0)
                str3 = string.Join(" ", line3) + Environment.NewLine;

            if (!string.IsNullOrEmpty(address.Country))
                str4 = address.Country;

            return str1 + str2 + str3 + str4;
        }
    }
}