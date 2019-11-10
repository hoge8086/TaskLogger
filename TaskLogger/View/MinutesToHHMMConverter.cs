using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace TaskLogger.View
{
    class MinutesToHHMMConverter: MarkupExtension, IValueConverter
    {
        private static MinutesToHHMMConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new MinutesToHHMMConverter());
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";

            if (!(value is int)) throw new ArgumentException("not int");
            int minutes = (int)value;
            if (minutes < 60)
                return minutes.ToString();
            return string.Format("{0}:{1:D2}", minutes / 60, minutes % 60);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
