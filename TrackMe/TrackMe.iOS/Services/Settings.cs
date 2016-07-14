using TrackMe.Core.Services.Interfaces;

namespace TrackMe.iOS.Services
{
    public class Settings : ISettings
    {
        public bool AddOrUpdate<T>(string key, T value)
        {
            return true;
        }

        public T GetValue<T>(string key, T defualt = default(T))
        {
            return defualt;
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}