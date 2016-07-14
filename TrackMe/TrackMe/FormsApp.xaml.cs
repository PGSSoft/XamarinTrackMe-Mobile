using System;
using Xamarin.Forms;

namespace TrackMe
{
    public partial class FormsApp : Application
    {
        public event EventHandler Start;
        public event EventHandler Sleep;
        public event EventHandler Resume;

        public FormsApp()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            var handler = Start;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnSleep()
        {
            var handler = Sleep;
            handler?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnResume()
        {
            var handler = Resume;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
