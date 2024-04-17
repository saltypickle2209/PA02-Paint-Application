using Factories;
using Graphics;
using System.Windows;
using System.Windows.Media;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Controls;

namespace PA02_Paint_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<IShapeObjectFactory> _factories = new List<IShapeObjectFactory>();
        private IShapeObjectFactory? _currentFactory = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create ToolBar buttons
            Assembly assembly = Assembly.LoadFrom("Factories.dll");
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if(type.IsClass && !type.IsInterface && typeof(IShapeObjectFactory).IsAssignableFrom(type))
                {
                    try
                    {
                        _factories.Add((IShapeObjectFactory)Activator.CreateInstance(type)!);
                    }
                    catch (Exception ex) 
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }
            }

            _factories = new List<IShapeObjectFactory>(_factories.OrderBy(factory => factory.GetPriority()));

            for(int i = 0; i < _factories.Count; i++)
            {
                IShapeObjectFactory shapeObjectFactory = _factories[i];
                Trace.WriteLine(shapeObjectFactory.GetName());

                RadioButton radioButton = new RadioButton
                {
                    Tag = shapeObjectFactory,
                    IsChecked = i == 0,
                };
                radioButton.Checked += ShapeRadioButton_Checked;

                Canvas canvas = new Canvas
                {
                    Width = 32,
                    Height = 32
                };

                Point startingPoint = new Point(4, 4);
                Point endingPoint = new Point(28, 28);

                UIElement canvasChild = shapeObjectFactory.CreateProduct(startingPoint, endingPoint, Brushes.Black, 1, new NormalStroke(), 0, false, false, false).ConvertToUIElement();
                canvas.Children.Add(canvasChild);
                radioButton.Content = canvas;
                ShapeToolBar.Items.Add(radioButton);
            }

            _currentFactory = _factories[0];


            GraphicObject rectangle = new RectangleObject(new Point(100, 100), new Point(400, 200), Brushes.Red, 1, new NormalStroke(), 0, false, false, false);
            drawCanvas.Children.Add(rectangle.ConvertToUIElement());
            IShapeObjectFactory factory = new EllipseObjectFactory();
            GraphicObject graphicObject = factory.CreateProduct(new Point(100, 100), new Point(400, 200), Brushes.Red, 5, new DashDotDotStroke(), 45, false, false, false);
            drawCanvas.Children.Add(graphicObject.ConvertToUIElement());
            GraphicObject textObject = new TextObject((ShapeObject)graphicObject, "Lorem ipsum dolor bla bla bla so long I love you moah moah", Brushes.Black, 12, "Meo", Brushes.Transparent);
            drawCanvas.Children.Add(textObject.ConvertToUIElement());
        }

        private void ShapeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _currentFactory = (IShapeObjectFactory)(sender as RadioButton)!.Tag;
            Trace.WriteLine("Current tool: " + _currentFactory.GetName());
        }
    }
}