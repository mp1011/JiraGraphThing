using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JiraGraphThing.Converters
{
    public class BooleanToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool booleanValue)
            {
                if (!booleanValue)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
