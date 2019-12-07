using System;
using System.ComponentModel;
using System.Windows.Data;

using System.Windows.Markup;

namespace TaskLogger.View
{
    public class ToClassDiscriptionConverter : MarkupExtension, IValueConverter
    {
        private static ToClassDiscriptionConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ToClassDiscriptionConverter());
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var gm = value.GetType().GetMember(value.ToString());
                var discriptions = (DescriptionAttribute[])value.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (discriptions.Length == 0)
                    return "";

                return discriptions[0].Description;
            }
            catch
            {
                return string.Empty;
            }
        }

        // No need to implement converting back on a one-way binding 
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
