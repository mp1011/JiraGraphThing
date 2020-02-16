using System;
using Windows.UI.Xaml.Data;

namespace JiraGraphThing.Converters
{
    public class DoubleToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double dbl)
                return (int)dbl;
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
