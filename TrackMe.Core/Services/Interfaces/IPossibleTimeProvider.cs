using System.Collections.Generic;
using TrackMe.Core.Models;

namespace TrackMe.Core.Services.Interfaces
{
    public interface IPossibleTimeProvider
    {
        IEnumerable<PossibleTime> GetPossibleTimes();
    }
}