using System;
using System.Collections.Generic;
using TrackMe.Core.Models;

namespace TrackMe.Core.Helpers
{
    public static class GeoHelper
    {
        public static double ToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        public static double EarthRadiusInMeters => 6371000;

        public static IList<Position> CalculatePolygon(double latitude, double langitude, double accuracy, int increamentValue = 4)
        {
            var d = accuracy / EarthRadiusInMeters;
            var lat = ToRadian(latitude);
            var lng = ToRadian(langitude);
            var coords = new List<Position>();
            for (int i = 0; i <= 360; i += increamentValue)
            {
                var brng = ToRadian(i);
                var latRadians = Math.Asin(Math.Sin(lat) * Math.Cos(d) + Math.Cos(lat) * Math.Sin(d) * Math.Cos(brng));
                var lngRadians = lng +
                                 Math.Atan2(Math.Sin(brng) * Math.Sin(d) * Math.Cos(lat),
                                     Math.Cos(d) - Math.Sin(lat) * Math.Sin(latRadians));
                coords.Add(new Position(180 * latRadians / Math.PI, 180 * lngRadians / Math.PI));
            }

            return coords;
        }
    }
}
