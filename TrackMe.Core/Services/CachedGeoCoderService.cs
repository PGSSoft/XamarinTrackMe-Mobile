using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Core.Services
{
    public interface ICachedGeoCoderService : IGeoCoderService
    {
     
    }

    public class CachedGeoCoderService : ICachedGeoCoderService
    {
        private readonly IGeoCoderService _geoCoderService;

        public CachedGeoCoderService(IGeoCoderService geoCoderService)
        {
            _geoCoderService = geoCoderService;
            CacheLifetime = TimeSpan.FromSeconds(50);
        }

        public TimeSpan CacheLifetime { get; set; }
        private DateTime _lastTime = DateTime.MinValue;
        private IEnumerable<string> _cachedResult;

        public async Task<IEnumerable<string>> GetAddress(double lat, double lng)
        {
            var now = DateTime.Now;
            if (now > _lastTime + CacheLifetime)
            {
                _cachedResult = await _geoCoderService.GetAddress(lat, lng);
                _lastTime = now;
            }

            return _cachedResult;
        }
    }
}
