using Windows.UI.Xaml;

namespace TrackMe.WinPhoneUniversalNative.Converters
{
    public sealed class InvertBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public InvertBooleanToVisibilityConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        { }
    }
}