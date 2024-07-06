using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Setting.Convert
{
    public class RectangeThicknessConverter : IValueConverter
    {
        const int Max = 10;
        const int Mix = 8;
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
