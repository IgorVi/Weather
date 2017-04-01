using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NumberFormatInfo numberFormatInfo = CultureInfo.GetCultureInfo("en-US").NumberFormat;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetLocaleFromAppSettings();
            forecast.Latitude = map.Latitude;
            forecast.Longitude = map.Longitude;
            forecast.LoadForecast();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            getMenu.Visibility = Visibility.Collapsed;
            menu.Visibility = Visibility.Visible;
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            menu.Visibility = Visibility.Collapsed;
            getMenu.Visibility = Visibility.Visible;

        }

        private void btnMap_Click(object sender, RoutedEventArgs e)
        {
            forecast.Visibility = Visibility.Collapsed;
            map.Visibility = Visibility.Visible;
        }

        private void butnForecast_Click(object sender, RoutedEventArgs e)
        {
            map.Visibility = Visibility.Collapsed;
            forecast.Visibility = Visibility.Visible;
            if (forecast.Latitude != map.Latitude || forecast.Longitude != map.Longitude)
            {
                forecast.Latitude = map.Latitude;
                forecast.Longitude = map.Longitude;
                forecast.LoadForecast();
                UpdateLocationAppSettings();
            }
        }

        public void UpdateLocationAppSettings()
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                settings["latitude"].Value = string.Format("{0:F4}", map.Latitude.ToString(numberFormatInfo));
                settings["longitude"].Value = string.Format("{0:F4}", map.Longitude.ToString(numberFormatInfo));
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }


        private void GetLocaleFromAppSettings()
        {
            try
            {
                map.Latitude = double.Parse(ConfigurationManager.AppSettings["latitude"], numberFormatInfo);

                map.Longitude = double.Parse(ConfigurationManager.AppSettings["longitude"], numberFormatInfo);
            }
            catch
            { }
        }
    }
}
