using Xamarin.Forms;

namespace TrackMe.Services
{
    public class PageService : IPageService
    {
        public Page CurrentPage
        {
            get
            {
                var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
               
                var page = masterDetailPage != null
                    ? masterDetailPage.Detail
                    : Application.Current.MainPage;
                
                var navigationPage = page as IPageContainer<Page>;

                return navigationPage != null ? 
                    navigationPage.CurrentPage :
                    page;
            }
        }
    }
}