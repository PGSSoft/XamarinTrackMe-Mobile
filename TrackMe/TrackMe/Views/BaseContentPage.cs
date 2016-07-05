using Xamarin.Forms;

namespace TrackMe.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            SetBinding(TitleProperty, new Binding(TitleProperty.PropertyName));
        }
    }

    public class BackgroundDebugColors
    {
        public bool IsEnabled { get; set; }
        private Color GetColor(Color color) => IsEnabled ? color : Color.Default;

        public Color A => GetColor(Color.Aqua);
        public Color B => GetColor(Color.Blue);
        public Color C => GetColor(Color.Gray);
        public Color D => GetColor(Color.Green);
        public Color E => GetColor(Color.Lime);
        public Color F => GetColor(Color.Maroon);
        public Color G => GetColor(Color.Pink);
        public Color H => GetColor(Color.Olive);
    }
}
