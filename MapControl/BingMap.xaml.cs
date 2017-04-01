using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Globalization;
using System.Timers;

namespace Weather.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BingMap : UserControl
    {
        private Point mousePosition;
        private bool isMouseDown;
        private Timer timerMouse = new Timer();

        public static readonly DependencyProperty MapZoomGrowthProperty =
            DependencyProperty.Register("MapZoomGrowth", typeof(double), typeof(BingMap), new PropertyMetadata(0.5));

        public BingMap()
        {
            InitializeComponent();
            pin.Location = new Location();
        }

        public double MapZoomGrowth
        {
            get { return (double)GetValue(MapZoomGrowthProperty); }
            set { SetValue(MapZoomGrowthProperty, value); }
        }
        public double Longitude
        {
            get { return pin.Location.Longitude; }
            set { pin.Location.Longitude = value; }
        }
        public double Latitude
        {
            get { return pin.Location.Latitude; }
            set { pin.Location.Latitude = value; }
        }

        private void buttonZoomMore_Click(object sender, RoutedEventArgs e)
        {
            map.ZoomLevel += MapZoomGrowth;
        }
        private void buttonZoomLess_Click(object sender, RoutedEventArgs e)
        {
            map.ZoomLevel -= MapZoomGrowth;
        }

        private void mapMod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBoxItem)((ComboBox)sender).SelectedItem).Tag.ToString())
            {
                case "Aerial":
                    map.Mode = new AerialMode();
                    break;
                case "Road":
                    map.Mode = new RoadMode();
                    break;
                case "AerialWithLabels":
                    map.Mode = new AerialMode(true);
                    break;
            }
        }

        private void map_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            timerMouse.Start();
            e.Handled = true;
            mousePosition = e.GetPosition(this);
        }
        private void map_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            map.Cursor = Cursors.Arrow;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            timerMouse.Elapsed += TimerMouse_Elapsed;
            timerMouse.Interval = 200;
            map.Center = pin.Location;
            textBoxLatitude.Text = "Latitude: " + pin.Location.Latitude.ToString("F4");
            textBoxLongitude.Text = "Longitude: " + pin.Location.Longitude.ToString("F4");
        }

        private void TimerMouse_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerMouse.Stop();
            if (!isMouseDown)
            {
                Location pinLocation = map.ViewportPointToLocation(mousePosition);
                Dispatcher.BeginInvoke((Action)(() => pin.Location = pinLocation));
            }
            else
            {
                Dispatcher.BeginInvoke((Action)(() => map.Cursor = Cursors.SizeAll));
            }
        }
    }
}
