using System.Collections.Generic;
using System.Linq;
using TrackMe.Core.Models;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Core.Services
{
    public class PossibleTimeProvider : IPossibleTimeProvider
    {
        public IEnumerable<PossibleTime> GetPossibleTimes()
        {
#if DEBUG
            int[] times = {1, 5, 10, 30, 60, 120};
#else
            int[] times = {5, 10, 30, 60, 120};
#endif
            return times.Select(time => new PossibleTime(time)).ToList();
        }
    }
}