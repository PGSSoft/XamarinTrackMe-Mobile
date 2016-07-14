using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TrackMe.Core.Helpers
{
    public static class ObservableExtensions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
