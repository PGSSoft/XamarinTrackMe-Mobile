using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace TrackMe.WinPhoneNative.Helpers
{
    // http://codecoding.com/fast-app-switching-and-mvvmcross-how-to-make-it-work/
    public class FASSupport
    {
        private bool _isPreserved;
        private bool _isReset;
        private static FASSupport _instance;

        private FASSupport()
        {
            //http://msdn.microsoft.com/en-us/library/windowsphone/develop/jj735579(v=vs.105).aspx
            App.RootFrame.Navigating += RootFrame_Navigating;
            App.RootFrame.Navigated += RootFrame_Navigated;
            PhoneApplicationService.Current.Activated += Current_Activated;
        }

        public static void Initialize()
        {
            if (_instance == null)
                _instance = new FASSupport();
        }

        public void Reset()
        {
            _isPreserved = false;
            _isReset = false;
        }

        void Current_Activated(object sender, ActivatedEventArgs e)
        {
            _isPreserved = e.IsApplicationInstancePreserved;
        }

        void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (_isPreserved && _isReset)
            {
                e.Cancel = true;
                Reset();
            }
        }

        void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            _isReset = e.NavigationMode == NavigationMode.Reset;
        }
    }
}