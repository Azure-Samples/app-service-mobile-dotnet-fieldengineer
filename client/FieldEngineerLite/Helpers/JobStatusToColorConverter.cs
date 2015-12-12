using System;
using FieldEngineerLite.Models;
using Xamarin.Forms;

namespace FieldEngineerLite.Helpers
{
    public class JobStatusToColorConverter : IValueConverter
    {
        private bool useLightTheme;

        public JobStatusToColorConverter(bool useLightTheme = false)
        {
            this.useLightTheme = useLightTheme; 
        }

        private Color GetColorFromStatus(string status)
        {
            switch (status)
            {
                case Job.InProgressStatus:
                    return Color.Green;
                case Job.PendingStatus:
                    return Color.FromRgb(210, 90, 0);
                case Job.CompleteStatus:
                    return Color.Blue;
                default:
                    throw new ArgumentException(string.Format("Unknown status: '{0}'", status), "status");
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var status = (string)value;
            var color = GetColorFromStatus(status);
            if (useLightTheme)
            {
                var luminosityModifier = Device.OnPlatform(
                    iOS: 0.95, 
                    Android: 0.1, 
                    WinPhone: 0.1
                );

                color = color.WithLuminosity(luminosityModifier);
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
