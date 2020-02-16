using System;
using Windows.UI.Xaml.Data;
using JiraGraphThing.Core.Extensions;

namespace JiraGraphThing.Converters
{
    public class CompactTimeFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is TimeSpan ts)
            {
                return ts.FormatCompact();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
