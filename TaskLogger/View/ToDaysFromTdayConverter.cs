using System;
using System.Windows.Data;

using System.Windows.Markup;

namespace TaskLogger.View
{
    public class ToDaysFromTdayConverter : MarkupExtension, IValueConverter
    {
        private static ToDaysFromTdayConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ToDaysFromTdayConverter());
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var date = value as DateTime?;
                if (date == null)
                    return "";
                if (date.Value.Date == DateTime.Today)
                    return "今日";
                if (date.Value.Date < DateTime.Today)
                    return  ((DateTime.Today - date.Value.Date).Days).ToString() + "日前";

                return  ((date.Value.Date - DateTime.Today).Days).ToString() + "日後";

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
