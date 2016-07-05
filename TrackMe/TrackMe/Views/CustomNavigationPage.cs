using Xamarin.Forms;

namespace TrackMe.Views
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage():base()
        {
            BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
        }

        public CustomNavigationPage(Page page):base(page)
        {     
        }
    }
}
