using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml;

namespace Weather.Controls
{
    public class TemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double temperature = 0;
            double.TryParse(((XmlAttribute)value).InnerText, NumberStyles.Any, new CultureInfo("en-US").NumberFormat, out temperature);
            string str = string.Empty;
            if (temperature > 0)
            {
                str = "+";
            }
            return str + temperature.ToString() + "°";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
