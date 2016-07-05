using System;
using TrackMe.Core.Models;

namespace TrackMe.Core
{
    public static class Constants
    {
        public static string BaseUrl = @"http://trackme.pgs-soft.com/";
        public static string TokenSubUrl = @"api/Token";
        public static string SendPositionSubUrl = @"api/Coordinates";

        public static string BrowseUrl = BaseUrl + @"#/token/{0}";

        public static Position DefaultPosition = new Position(50,20);
        public static TimeSpan TimeBetweenUpdates = TimeSpan.FromSeconds(5);
    }

    public static class SettingKeys
    {
        public static readonly string Token = "Token";
        public static readonly string PrivateToken = "PrivateToken";
        public static readonly string EndTime = "EndTime";
        public static readonly string Duration = "Duration";
    }
}