using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JiraGraphThing.Converters
{
    public class BooleanToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] choices = (parameter as string).Split('|');

            if (value is bool booleanValue)
            {
                if (booleanValue)
                    return choices[1];
            }

            return choices[0];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
