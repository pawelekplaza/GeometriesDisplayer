using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Linq;
using System.Xml.Linq;

namespace PathDataDisplayer
{
    public class XElementToAttributyKeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is XElement element))
                return DependencyProperty.UnsetValue;

            return element.Attributes().FirstOrDefault(v => v.Name.LocalName.Equals("Key"))?.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
