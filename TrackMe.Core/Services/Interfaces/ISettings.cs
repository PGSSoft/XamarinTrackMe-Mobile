namespace TrackMe.Core.Services.Interfaces
{
    public interface ISettings
    {
        bool AddOrUpdate<T>(string key, T value);
        T GetValue<T>(string key, T defualt = default(T));
        void Clear();
    }
}
