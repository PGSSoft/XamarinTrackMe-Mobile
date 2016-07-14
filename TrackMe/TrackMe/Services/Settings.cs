using System;
using System.Collections.Generic;
using MvvmCross.Core.Platform;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.Settings;
using TrackMe.Core.Messages;
using TrackMe.Core.Services.Interfaces;
using Xamarin.Forms;

namespace TrackMe.Services
{
    public class Settings : ISettings
    {
        private Plugin.Settings.Abstractions.ISettings CrossProperties => CrossSettings.Current;
        private IDictionary<string, object> Properties => Application.Current.Properties;
        public bool AddOrUpdate<T>(string key, T value) => CrossProperties.AddOrUpdateValue(key, value);
        public T GetValue<T>(string key, T defualt = default(T)) => CrossProperties.GetValueOrDefault(key, defualt);
        public void Clear() => Properties.Clear();
    }
}
