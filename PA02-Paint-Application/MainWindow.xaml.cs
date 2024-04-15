using Factories;
using Graphics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PA02_Paint_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GraphicObject rectangle = new RectangleObject(new Point(100, 100), new Point(400, 200), Brushes.Red, 1, new NormalStroke(), 0, false, false, false);
            drawCanvas.Children.Add(rectangle.ConvertToUIElement());
            IShapeObjectFactory factory = new EllipseObjectFactory();
            GraphicObject graphicObject = factory.CreateProduct(new Point(100, 100), new Point(400, 200), Brushes.Red, 5, new DashDotDotStroke(), 45, false, false, false);
            drawCanvas.Children.Add(graphicObject.ConvertToUIElement());
            GraphicObject textObject = new TextObject((ShapeObject)graphicObject, "Lorem ipsum dolor bla bla bla so long I love you moah moah", Brushes.Black, 12, "Meo", Brushes.Transparent);
            drawCanvas.Children.Add(textObject.ConvertToUIElement());
        }
    }
}