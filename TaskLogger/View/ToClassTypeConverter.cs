using System;
using System.Windows.Data;

using System.Windows.Markup;

namespace TaskLogger.View
{
    public class ToClassTypeConverter: MarkupExtension, IValueConverter
    {
        private static ToClassTypeConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ToClassTypeConverter());
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            return value.GetType();
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
