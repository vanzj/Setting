using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Setting.Convert
{
    public class RectangeStrokeConverter : IValueConverter
    {
         SolidColorBrush enable = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cccccc")); 
         SolidColorBrush disable = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#654321"));
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (bool)value;
            return isVisible ? Max : Mix;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int currentValue = (int)value;
            return currentValue == Max;
        }
    }
}
