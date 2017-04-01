using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Weather.Controls
{
    public class DataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateValue = DateTime.Parse(value.ToString());
            StringBuilder strDate = new StringBuilder(dateValue.ToString("ddd"));
            strDate.Append(" ").Append(dateValue.Day.ToString()).Append(" ");
            return strDate.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
