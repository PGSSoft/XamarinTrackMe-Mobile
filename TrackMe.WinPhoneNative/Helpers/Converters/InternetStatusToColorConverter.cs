using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TrackMe.Core.Services.Interfaces;



namespace TrackMe.WinPhoneNative.Helpers.Converters
{
    class InternetStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target must be a Color");

            var val = (LocationStatus)value;

            if (val == LocationStatus.Started)
                return new SolidColorBrush(StartedColor);
            else if (val == LocationStatus.Initializing)
                return new SolidColorBrush(InitializingdColor);

            return new SolidColorBrush(StoppedColor);
        }


        public Color StartedColor { get; set; } = ColorHelper.ConvertStringToColor("00a561");
        public Color InitializingdColor { get; set; } = Colors.Yellow;
        public Color StoppedColor { get; set; } = ColorHelper.ConvertStringToColor("ce2323");
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}