using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TrackMe.Core.Models;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace TrackMe.WinPhoneUniversalNative.Controls
{
    public sealed class UserPosition : Control
    {
        public UserPosition()
        {
            this.DefaultStyleKey = typeof(UserPosition);
            Resources["PositionOrigin"] = new Point(0.5, 0.5);
            
        }

        public readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Position), typeof(UserPosition), new PropertyMetadata(null));

        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(UserPosition), new PropertyMetadata(null));

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var border = GetTemplateChild("PositionBorder") as Border;
            var grid = GetTemplateChild("ToolTipGrid") as Grid;

            if (border != null && grid != null)
                border.Tapped += (sender, args) => { grid.Visibility = Visibility.Visible; };
            
            SetToolTipVisibility(this);
        }

        public static readonly DependencyProperty ToolTipVisibilityProperty = DependencyProperty.Register("ToolTipVisibility", typeof(Visibility), typeof(UserPosition), new PropertyMetadata(Visibility.Collapsed));

        private static void SetToolTipVisibility(UserPosition userposition)
        {
            var grid = userposition?.GetTemplateChild("ToolTipGrid") as Grid;
            if (userposition != null && grid != null)
            {
                grid.Visibility = userposition.ToolTipVisibility;
            }
        }

        public Visibility ToolTipVisibility
        {
            get { return (Visibility)GetValue(ToolTipVisibilityProperty); }
            set
            {
                SetValue(ToolTipVisibilityProperty, value);
                SetToolTipVisibility(this);
            }
        }
    }
}
