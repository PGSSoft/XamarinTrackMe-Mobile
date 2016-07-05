using System;
using Windows.Storage;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.WinPhoneUniversalNative.Services
{
    public class Settings : ISettings
    {
        private ApplicationDataContainer AppSettings => ApplicationData.Current.LocalSettings;

        public bool AddOrUpdate<T>(string key, T value)
        {
            if (typeof (T) == typeof (DateTime))
            {
                var dt = ((DateTime)(object)value);

                var dto = (DateTimeOffset)dt;
                return AddOrUpdateImpl(key, dto);
            }

            return AddOrUpdateImpl(key, value);
        }

        private bool AddOrUpdateImpl<T>(string key, T value)
        {
            bool changed = false;
            if (AppSettings.Values.ContainsKey(key))
            {
                if (AppSettings.Values[key] != (object) value)
                {
                    AppSettings.Values[key] = value;
                    changed = true;
                }
            }
            else
            {
                AppSettings.CreateContainer(key, ApplicationDataCreateDisposition.Always);
                AppSettings.Values[key] = value;
                changed = true;
            }

            return changed;
        }

        public T GetValue<T>(string key, T defualt = default(T))
        {
            object value = defualt;

            if (AppSettings.Values.ContainsKey(key))
            {
                value = AppSettings.Values[key];
                if (value is DateTimeOffset)
                    value = ((DateTimeOffset)value).DateTime;
                
            }

            return (T)value;
        }

        public void Clear()
        {
            AppSettings.Values.Clear();
        }
    }
}