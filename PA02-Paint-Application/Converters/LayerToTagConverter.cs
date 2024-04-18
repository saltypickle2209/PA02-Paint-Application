using LayerManager;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace PA02_Paint_Application.Converters
{
    public class LayerToTagConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is Layer layer && values[1] is LayerList layerList)
            {
                return layerList.Layers.IndexOf(layer);
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}