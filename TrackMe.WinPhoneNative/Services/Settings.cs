using System.IO.IsolatedStorage;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneNative.Services
{
    class Settings : ISettings
    {
        static IsolatedStorageSettings IsolatedSettings => IsolatedStorageSettings.ApplicationSettings;

        public bool AddOrUpdate<T>(string key, T value)
        {
            var changed = false;

            if (IsolatedSettings.Contains(key))
            {
                if (IsolatedSettings[key] != (object)value)
                {
                    IsolatedSettings[key] = value;
                    changed = true;
                }
            }
            else
            {
                IsolatedSettings.Add(key, value);
                changed = true;
            }

            IsolatedSettings.Save();

            return changed;
        }

        public T GetValue<T>(string key, T defualt = default(T))
        {
            T value = defualt;
            if (IsolatedSettings.TryGetValue(key, out value))
            {
                return value;
            }

            return defualt;
        }

        public void Clear()
        {
            IsolatedSettings.Clear();
            IsolatedSettings.Save();
        }
    }
}
