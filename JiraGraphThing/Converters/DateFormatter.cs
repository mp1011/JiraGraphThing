using System;
using Windows.UI.Xaml.Data;

namespace JiraGraphThing.Converters
{
    public class DateFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is DateTime dt)
            {
                if(parameter is string format)
                {
                    return dt.ToString(format);
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
