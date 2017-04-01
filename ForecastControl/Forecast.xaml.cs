using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Weather.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Forecast : UserControl
    {
        private Timer timerSelecteItems = new Timer(50);
        private NumberFormatInfo numberFormatInfo = CultureInfo.GetCultureInfo("en-US").NumberFormat;

        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.Register("Latitude", typeof(double), typeof(Forecast), new PropertyMetadata(0D), new ValidateValueCallback(validateLatitude));
        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("Longitude", typeof(double), typeof(Forecast), new PropertyMetadata(0D), new ValidateValueCallback(ValidateLongitude));

        public Forecast()
        {
            InitializeComponent();
            timerSelecteItems.Elapsed += TimerSelecteItems_Elapsed; ;
        }

        public double Longitude
        {
            get { return (double)GetValue(LongitudeProperty); }
            set { SetValue(LongitudeProperty, value); }
        }
        public double Latitude
        {
            get { return (double)GetValue(LatitudeProperty); }
            set { SetValue(LatitudeProperty, value); }
        }

        public void LoadForecast()
        {
            darkBackground.Visibility = Visibility.Visible;
            string http = @"http://api.openweathermap.org/data/2.5/forecast/daily?lat=" + Latitude.ToString(numberFormatInfo) + "&lon=" + Longitude.ToString(numberFormatInfo) + "&cnt=7&appid=8ebc91462ca4ef611d4fe64c7a674709&mode=xml&units=metric";
            XmlDataProvider xdp = (XmlDataProvider)TryFindResource("weatherProvider");
            xdp.Source = new Uri(http);
        }

        private static bool validateLatitude(object value)
        {
            double lat = (double)value;
            return lat >= -90 && lat <= 90;
        }
        private static bool ValidateLongitude(object value)
        {
            double lon = (double)value;
            return lon <= 180 && lon >= -180;
        }
        private void XmlDataProvider_DataChanged(object sender, EventArgs e)
        {
            darkBackground.Visibility = Visibility.Collapsed;
            txtUpdate.Text = string.Format("Updated as of {0:HH:mm:ss}", DateTime.Now);
            timerSelecteItems.Start();
        }
        private void TimerSelecteItems_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerSelecteItems.Stop();
            Dispatcher.BeginInvoke((Action)(() => listBoxForecast.SelectedIndex = 0));
        }
    }
}
