using Factories;
using Graphics;
using System.Windows;
using System.Windows.Media;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PA02_Paint_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region PROPERTY_DECLARATION

        public event PropertyChangedEventHandler? PropertyChanged;

        private List<IShapeObjectFactory> _factories = new List<IShapeObjectFactory>();
        private IShapeObjectFactory? _currentFactory = null;

        private ObservableCollection<SolidColorBrush> _brushColors = new ObservableCollection<SolidColorBrush>();

        public ObservableCollection<SolidColorBrush> BrushColors
        {
            get { return _brushColors; }
        }

        private SolidColorBrush _currentBrushColor = Brushes.Black;

        public SolidColorBrush CurrentBrushColor
        {
            get { return _currentBrushColor; }
            set 
            {
                _currentBrushColor = value;
                Trace.WriteLine("Change brush's color to: " + value.ToString());
                OnPropertyChanged(nameof(CurrentBrushColor));
            }
        }

        private ObservableCollection<int> _brushThicknesses = new ObservableCollection<int>() { 1, 2, 3, 4, 5 };

        public ObservableCollection<int> BrushThicknesses
        {
            get { return _brushThicknesses; }
        }

        private int _currentBrushThickness = 1;

        public int CurrentBrushThickness
        {
            get { return _currentBrushThickness; }
            set 
            { 
                _currentBrushThickness = value;
                Trace.WriteLine("Change brush's thickness to: " + value.ToString());
                OnPropertyChanged(nameof(CurrentBrushThickness));
            }
        }

        private ObservableCollection<IStrokeType> _brushTypes = new ObservableCollection<IStrokeType>();

        public ObservableCollection<IStrokeType> BrushTypes
        {
            get { return _brushTypes; }
            set
            {
                _brushTypes = value;
                OnPropertyChanged(nameof(BrushTypes));
            }
        }

        private IStrokeType _currentBrushType;

        public IStrokeType CurrentBrushType
        {
            get { return _currentBrushType; }
            set
            {
                _currentBrushType = value;
                Trace.WriteLine("Change brush's type to: " + value.ToString());
                OnPropertyChanged(nameof(CurrentBrushThickness));
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeToolBar();

            GraphicObject rectangle = new RectangleObject(new Point(100, 100), new Point(400, 200), Brushes.Red, 1, new NormalStroke(), 0, false, false, false);
            drawCanvas.Children.Add(rectangle.ConvertToUIElement());
            IShapeObjectFactory factory = new StarObjectFactory();
            GraphicObject graphicObject = factory.CreateProduct(new Point(100, 100), new Point(200, 200), Brushes.Red, 5, new DashStroke(), 45, false, false, false);
            drawCanvas.Children.Add(graphicObject.ConvertToUIElement());
            GraphicObject textObject = new TextObject((ShapeObject)graphicObject, "Lorem ipsum dolor bla bla bla so long I love you moah moah", Brushes.Black, 12, "Meo", Brushes.Transparent);
            drawCanvas.Children.Add(textObject.ConvertToUIElement());
        }

        private void InitializeToolBar()
        {
            // Create ToolBar buttons
            Assembly assembly = Assembly.LoadFrom("Factories.dll");
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsInterface && typeof(IShapeObjectFactory).IsAssignableFrom(type))
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

            for (int i = 0; i < _factories.Count; i++)
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

            // Get all brush colors
            PropertyInfo[] properties = typeof(Brushes).GetProperties(BindingFlags.Static | BindingFlags.Public);

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(SolidColorBrush))
                {
                    _brushColors.Add((SolidColorBrush)property.GetValue(null)!);
                }
            }

            // Get all brush types
            assembly = Assembly.LoadFrom("Graphics.dll");
            types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsInterface && typeof(IStrokeType).IsAssignableFrom(type))
                {
                    try
                    {
                        _brushTypes.Add((IStrokeType)Activator.CreateInstance(type)!);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }
            }
            BrushTypes = new ObservableCollection<IStrokeType>(_brushTypes.OrderBy(type => type.GetPriority()));
            BrushTypeComboBox.SelectedIndex = 0;
        }

        private void ShapeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _currentFactory = (IShapeObjectFactory)(sender as RadioButton)!.Tag;
            Trace.WriteLine("Current tool: " + _currentFactory.GetName());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}