using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrackMe.Core.Services.Interfaces
{
    public interface IGeoCoderService
    {
        Task<IEnumerable<string>> GetAddress(double lat, double lng);
    }
}
