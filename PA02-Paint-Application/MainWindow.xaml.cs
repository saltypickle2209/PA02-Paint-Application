using Factories;
using Graphics;
using LayerManager;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PA02_Paint_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region PROPERTY_DECLARATION

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _currentTool = "Pen";

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

        private Point _startingPoint;
        private Point _endingPoint;
        private bool _isPerfectShape = false;
        private UIElement? _currentUIElement;
        private GraphicObject? _currentGraphicObject;
        private bool _isDrawing = false;

        #endregion PROPERTY_DECLARATION

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private LayerList _layerList;

        public LayerList LayerList
        {
            get { return _layerList; }
            set
            {
                _layerList = value;
                OnPropertyChanged(nameof(LayerList));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeToolBar();

            LayerList = new LayerList();
            LayerList.AddLayer(new Layer());
            LayerList.AddLayer(new Layer());
            LayerList.AddLayer(new Layer());
        }

        #region UI_INITIALIZATION

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
                    ToolTip = $"Draw a {shapeObjectFactory.GetName()}"
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

        #endregion UI_INITIALIZATION

        private void ToolRadioButton_Checked(object sender, EventArgs e)
        {
            _currentTool = (string)(sender as RadioButton)!.Tag;
            Trace.WriteLine("Current tool: " + _currentTool);
        }

        private void ShapeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _currentFactory = (IShapeObjectFactory)(sender as RadioButton)!.Tag;
            Trace.WriteLine("Current shape: " + _currentFactory.GetName());
        }

        private void LayerRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            LayerList.CurrentLayerIndex = (int)(sender as RadioButton)!.Tag;
            Trace.WriteLine("Current layer: " + LayerList.CurrentLayerIndex.ToString());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void drawCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentTool == "Pen")
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _startingPoint = e.GetPosition(drawCanvas);
                    _endingPoint = _startingPoint;
                    _currentGraphicObject = _currentFactory!.CreateProduct(_startingPoint, _endingPoint, _currentBrushColor, _currentBrushThickness, _currentBrushType, 0, false, false, false);
                    _currentUIElement = _currentGraphicObject.ConvertToUIElement();
                    drawCanvas.Children.Add(_currentUIElement);
                    _isDrawing = true;
                }
            }
        }

        private void drawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentTool == "Pen" && _isDrawing)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _endingPoint = e.GetPosition(drawCanvas);
                    ((ShapeObject)_currentGraphicObject!).EndingPoint = _endingPoint;
                    _currentGraphicObject.UpdateUIElement(_currentUIElement!);
                }
            }
        }

        private void drawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentTool == "Pen" && _isDrawing)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    LayerList.GetCurrentLayer()!.AddItem(_currentGraphicObject!);
                    _currentGraphicObject = null;
                    _currentUIElement = null;
                    _isDrawing = false;
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if (_isDrawing)
                {
                    _isPerfectShape = true;
                    ((ShapeObject)_currentGraphicObject!).IsPerfectShape = _isPerfectShape;
                    _currentGraphicObject.UpdateUIElement(_currentUIElement!);
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if (_isDrawing)
                {
                    _isPerfectShape = false;
                    ((ShapeObject)_currentGraphicObject!).IsPerfectShape = _isPerfectShape;
                    _currentGraphicObject.UpdateUIElement(_currentUIElement!);
                }
            }
        }
    }
}