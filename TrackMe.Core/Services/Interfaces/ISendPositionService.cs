using System.Threading.Tasks;
using TrackMe.Core.Models;

namespace TrackMe.Core.Services.Interfaces
{
    public interface ISendPositionService
    {
        Task<WebResult> SendPosition(string token, Position position, string address, double? accuracy);
        Task<WebResult<TokenPair>> GetToken(Position position, int duration, string address="", double? accuracy=null);
        Task<WebResult> StopSession(string privateToken);
    }
}
