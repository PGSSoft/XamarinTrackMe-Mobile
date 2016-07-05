using MvvmCross.Platform;
using System;
using Xamarin.Forms;
using MvvmCross.Plugins.Messenger;
using TrackMe.Core.Messages;
using Xamarin.Forms.Maps;

namespace TrackMe.Views
{
    public partial class HomePage : BaseContentPage
    {
        bool isFirstTime = true;
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);

			float width = 0.01F ;
			MyMap.MoveToRegion(new Xamarin.Forms.Maps.MapSpan( new Position( 50.0493764, 22.00924015),width,width));

            var messenger = Mvx.Resolve<IMvxMessenger>();
            var _locationToken = messenger.Subscribe<LocationMessage>(message =>{
				if (isFirstTime && message.Lat!=(-1) && message.Lng !=(-1) ) {
					MyMap.MoveToRegion(new Xamarin.Forms.Maps.MapSpan( new Position( message.Lat, message.Lng),width,width));
					isFirstTime = false;
                }
            });
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            
            await button.ScaleTo(0.95, 50, Easing.CubicInOut);
            await button.ScaleTo(1, 50, Easing.CubicIn);
        }

        protected override void OnAppearing()
        {
            //HomeViewModel vm = (HomeViewModel)this.BindingContext;
            //todo 
            //Map.MoveToRegion
        }

    }
}
