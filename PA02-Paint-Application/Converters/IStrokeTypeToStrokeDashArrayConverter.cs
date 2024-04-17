using Graphics;
using System.Globalization;
using System.Windows.Data;

namespace PA02_Paint_Application.Converters
{
    public class IStrokeTypeToStrokeDashArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IStrokeType strokeType)
            {
                return strokeType.GetStrokeDashArray();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}