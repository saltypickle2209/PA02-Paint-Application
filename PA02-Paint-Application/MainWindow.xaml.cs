using Factories;
using Graphics;
using LayerManager;
using Memento;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private SolidColorBrush _currentTextColor = Brushes.Black;

        public SolidColorBrush CurrentTextColor
        {
            get { return _currentTextColor; }
            set
            {
                _currentTextColor = value;
                Trace.WriteLine("Change text's color to: " + value.ToString());
                OnPropertyChanged(nameof(CurrentTextColor));
            }
        }

        private ObservableCollection<int> _textSizes = new ObservableCollection<int>() { 10, 12, 16, 20, 24, 32 };

        public ObservableCollection<int> TextSizes
        {
            get { return _textSizes; }
        }

        private int _currentTextSize = 10;

        public int CurrentTextSize
        {
            get { return _currentTextSize; }
            set
            {
                _currentTextSize = value;
                Trace.WriteLine("Change text's size to: " + value.ToString());
                OnPropertyChanged(nameof(CurrentTextSize));
            }
        }

        private ObservableCollection<FontFamily> _textFonts = new ObservableCollection<FontFamily>(Fonts.SystemFontFamilies);

        public ObservableCollection<FontFamily> TextFonts
        {
            get { return _textFonts; }
        }

        private FontFamily _currentTextFont = new FontFamily("Arial");

        public FontFamily CurrentTextFont
        {
            get { return _currentTextFont; }
            set
            {
                _currentTextFont = value;
                Trace.WriteLine("Change text's font to: " + value.Source);
                OnPropertyChanged(nameof(CurrentTextFont));
            }
        }


        private SolidColorBrush _currentTextBackgroundColor = Brushes.Transparent;

        public SolidColorBrush CurrentTextBackgroundColor
        {
            get { return _currentTextBackgroundColor; }
            set
            {
                _currentTextBackgroundColor = value;
                Trace.WriteLine("Change text's background color to: " + value.ToString());
                OnPropertyChanged(nameof(CurrentTextBackgroundColor));
            }
        }

        private StackPanel _textToolBar;

        private Point _startingPoint;
        private Point _endingPoint;
        private Point PreviousStartPoint;
        private Point PreviousEndPoint;
        private Point NextStartPoint;
        private Point NextEndPoint;
        private bool _isPerfectShape = false;
        private UIElement? _currentUIElement;
        private GraphicObject? _currentGraphicObject;
        private GraphicObject? selectedGraphicObject;
        private bool _isDrawing = false;
        private bool _isMoving = false;

        private List<GraphicObject> _clipboard = new List<GraphicObject>();
        private List<GraphicObject>? _tempMem = null;
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
        private LayerList _layerListTest;

        public LayerList LayerListTest
        {
            get { return _layerListTest; }
            set
            {
                _layerListTest = value;
                OnPropertyChanged(nameof(LayerListTest));
            }
        }
        private Originator _originator;
        public Originator Originator
        {
            get { return _originator; }
            set
            {
                _originator = value;
                OnPropertyChanged(nameof(Originator));
            }
        }
        private Caretaker _caretaker;
        public Caretaker CareTaker
        {
            get { return _caretaker; }
            set
            {
                _caretaker = value;
                OnPropertyChanged(nameof(CareTaker));
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeToolBar();

            LayerList = new LayerList();
            LayerList.AddLayer(new Layer());

            _textToolBar = TextToolBar;
            _textToolBar.Visibility = Visibility.Hidden;
            LayerListTest = new LayerList();
            LayerListTest.AddLayer(new Layer());

            Originator = new Originator(LayerList);
            CareTaker = new Caretaker(Originator);
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
            if (_currentTool != "Text" && _textToolBar != null)
            {
                _textToolBar.Visibility = Visibility.Hidden;
            }
            if (_currentTool != "MultipleSelection" && _currentGraphicObject != null)
            {
                drawCanvas.Children.Remove(_currentUIElement);
                _currentGraphicObject = null;
                _currentUIElement = null;
                _tempMem = null;
            }
        }

        private void ShapeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _currentFactory = (IShapeObjectFactory)(sender as RadioButton)!.Tag;
            Trace.WriteLine("Current shape: " + _currentFactory.GetName());
        }

        private void LayerRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            LayerList.CurrentLayerIndex = LayerList.Layers.IndexOf((Layer)(sender as RadioButton)!.Tag);
            Trace.WriteLine("Current layer: " + LayerList.CurrentLayerIndex.ToString());
            if (_currentTool == "MultipleSelection" && _currentGraphicObject != null)
            {
                drawCanvas.Children.Remove(_currentUIElement);
                _currentGraphicObject = null;
                _currentUIElement = null;
                _tempMem = null;
            }
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
            else if (_currentTool == "Text")
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _startingPoint = e.GetPosition(drawCanvas);
                    _currentGraphicObject = LayerList.GetCurrentLayer()?.FindItem(_startingPoint);
                    if (_currentGraphicObject != null)
                    {
                        ShapeObject shapeObject = (ShapeObject)_currentGraphicObject;
                        double centerX = (shapeObject.StartingPoint.X + shapeObject.EndingPoint.X) / 2;
                        double centerY = (shapeObject.StartingPoint.Y + shapeObject.EndingPoint.Y) / 2;

                        _textToolBar.SetValue(Canvas.LeftProperty, centerX - _textToolBar.ActualWidth / 2);
                        _textToolBar.SetValue(Canvas.TopProperty, centerY - _textToolBar.ActualHeight);
                        _textToolBar.Visibility = Visibility.Visible;

                        TextToolBarTextBox.Focus();
                    }
                }
            }
            else if (_currentTool == "SingleSelection")
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (_currentGraphicObject != null)
                    {
                        drawCanvas.Children.Remove(_currentUIElement);
                        _currentGraphicObject = null;
                        _currentUIElement = null;
                        _tempMem = null;
                    }
                    _isDrawing = false;
                    _startingPoint = e.GetPosition(drawCanvas);
                    _currentGraphicObject = LayerList.GetCurrentLayer()?.FindItem(_startingPoint);
                    //selectedGraphicObject = _currentGraphicObject;

                    if (selectedGraphicObject != null && selectedGraphicObject == _currentGraphicObject)
                    {
                        //_currentGraphicObject = null;
                        //drawCanvas.Children.Remove(_currentUIElement);
                        _isDrawing = true;

                        foreach (UIElement element in drawCanvas.Children)
                        {
                            // Thực hiện các thao tác trên mỗi phần tử (element) ở đây
                            // Ví dụ: Xoá phần tử khỏi canvas
                            if (element is Shape shapElement)
                            {

                                if (shapElement.Tag == selectedGraphicObject.Id)
                                {
                                    _currentUIElement = element;

                                }
                            }
                        }
                        PreviousStartPoint = new Point(((ShapeObject)selectedGraphicObject).StartingPoint.X, ((ShapeObject)selectedGraphicObject).StartingPoint.Y);
                        PreviousEndPoint = new Point(((ShapeObject)selectedGraphicObject).EndingPoint.X, ((ShapeObject)selectedGraphicObject).EndingPoint.Y);
                    }
                    else if (_currentGraphicObject != null)
                    {
                        selectedGraphicObject = _currentGraphicObject;

                        (Point TopPoint, Point BottomPoint) bounds = ((Point, Point))LayerList.GetABound(_currentGraphicObject);
                        _currentGraphicObject = new RectangleObject(bounds.TopPoint, bounds.BottomPoint, Brushes.Blue, 1, new DashStroke(), 0, false, false, false);
                        _currentUIElement = _currentGraphicObject.ConvertToUIElement();
                        _currentUIElement.Opacity = 0.3f;
                        ((Rectangle)_currentUIElement).Fill = Brushes.LightBlue;
                        drawCanvas.Children.Add(_currentUIElement);
                    }
                    else
                    {
                        selectedGraphicObject = null;
                    }
                }
            }
            else if (_currentTool == "MultipleSelection")
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // Check for pre-existed selection area
                    if (_currentGraphicObject != null)
                    {
                        drawCanvas.Children.Remove(_currentUIElement);
                        _currentGraphicObject = null;
                        _currentUIElement = null;
                        _tempMem = null;
                    }

                    _startingPoint = e.GetPosition(drawCanvas);
                    _endingPoint = _startingPoint;
                    _currentGraphicObject = new RectangleObject(_startingPoint, _endingPoint, Brushes.Blue, 1, new NormalStroke(), 0, false, false, false);
                    _currentUIElement = _currentGraphicObject.ConvertToUIElement();
                    _currentUIElement.Opacity = 0.3f;
                    ((Rectangle)_currentUIElement).Fill = Brushes.LightBlue;
                    drawCanvas.Children.Add(_currentUIElement);
                    _isDrawing = true;
                }
            }
        }

        private void drawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if ((_currentTool == "Pen" || _currentTool == "MultipleSelection") && _isDrawing)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _endingPoint = e.GetPosition(drawCanvas);
                    ((ShapeObject)_currentGraphicObject!).EndingPoint = _endingPoint;
                    if (_currentGraphicObject is StarObject || _currentGraphicObject is ArrowObject)
                    {
                        ((ShapeObject)_currentGraphicObject!).IsHorizontallyFlipped = _startingPoint.X > _endingPoint.X;
                        ((ShapeObject)_currentGraphicObject!).IsVerticallyFlipped = _startingPoint.Y > _endingPoint.Y;
                    }
                    _currentGraphicObject.UpdateUIElement(_currentUIElement!);
                }
            }
            else if (_currentTool == "SingleSelection" && _isDrawing)
            {
                if (e.LeftButton == MouseButtonState.Pressed && selectedGraphicObject != null)
                {
                    double offsetX = e.GetPosition(drawCanvas).X - _startingPoint.X;
                    double offsetY = e.GetPosition(drawCanvas).Y - _startingPoint.Y;
                    _startingPoint = e.GetPosition(drawCanvas);
                    Point StartUpdate = ((ShapeObject)selectedGraphicObject).StartingPoint;
                    Console.WriteLine($"StartingPoint: {StartUpdate}");
                    StartUpdate.X += offsetX;
                    StartUpdate.Y += offsetY;
                    Point EndUpdate = ((ShapeObject)selectedGraphicObject).EndingPoint;
                    Console.WriteLine($"EndPoint: {EndUpdate}");
                    _isMoving = true;
                    EndUpdate.X += offsetX;
                    EndUpdate.Y += offsetY;
                    ((ShapeObject)selectedGraphicObject).StartingPoint = StartUpdate;
                    ((ShapeObject)selectedGraphicObject).EndingPoint = EndUpdate;

                    selectedGraphicObject.UpdateUIElement(_currentUIElement!);
                    foreach (GraphicObject text in LayerList.GetCurrentLayer()!.GraphicObjectList)
                    {
                        if (text is TextObject Graphic)
                        {
                            if (Graphic.Parent == selectedGraphicObject)
                            {

                                foreach (UIElement element in drawCanvas.Children)
                                {
                                    // Thực hiện các thao tác trên mỗi phần tử (element) ở đây
                                    // Ví dụ: Xoá phần tử khỏi canvas
                                    if (element is TextBlock shapElement)
                                    {

                                        if (shapElement.Tag == Graphic.Id)
                                        {
                                            Trace.WriteLine("TextMove");

                                            Graphic.UpdateUIElement(element);

                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }

        private async void drawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_currentTool == "Pen" && _isDrawing)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    LayerList.GetCurrentLayer()!.AddItem(_currentGraphicObject!);
                    List<GraphicObject> listTemp = new List<GraphicObject>();
                    listTemp.Add(_currentGraphicObject!);
                    CareTaker.BackupAddAction(listTemp, LayerList.CurrentLayerIndex);
                    _currentGraphicObject = null;
                    _currentUIElement = null;
                    _isDrawing = false;
                    RedrawAll();
                }
            }
            else if (_currentTool == "MultipleSelection" && _isDrawing)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    drawCanvas.Children.Remove(_currentUIElement!);
                    _currentGraphicObject = null;
                    _currentUIElement = null;
                    _isDrawing = false;

                    CaptureAreaToClipboard(_startingPoint, _endingPoint);

                    _tempMem = LayerList.GetCurrentLayer()!.FindItemInRange(_startingPoint, _endingPoint);
                    if (_tempMem.Count != 0)
                    {
                        (Point TopPoint, Point BottomPoint) bounds = ((Point, Point))LayerList.GetBounds(_tempMem);
                        _currentGraphicObject = new RectangleObject(bounds.TopPoint, bounds.BottomPoint, Brushes.Blue, 1, new DashStroke(), 0, false, false, false);
                        _currentUIElement = _currentGraphicObject.ConvertToUIElement();
                        _currentUIElement.Opacity = 0.3f;
                        ((Rectangle)_currentUIElement).Fill = Brushes.LightBlue;
                        drawCanvas.Children.Add(_currentUIElement);
                    }
                }
            }
            else if (_currentTool == "SingleSelection" && _isDrawing && _isMoving)
            {
                List<GraphicObject> MoveItem = new List<GraphicObject>();
                MoveItem.Add(selectedGraphicObject!);
                NextStartPoint = new Point(((ShapeObject)selectedGraphicObject).StartingPoint.X, ((ShapeObject)selectedGraphicObject).StartingPoint.Y);
                NextEndPoint = new Point(((ShapeObject)selectedGraphicObject).EndingPoint.X, ((ShapeObject)selectedGraphicObject).EndingPoint.Y);
                CareTaker.BackupMoveAction(MoveItem, LayerList.CurrentLayerIndex, NextStartPoint, NextEndPoint, PreviousEndPoint, PreviousStartPoint);
                _currentGraphicObject = null;
                _currentUIElement = null;
                _isDrawing = false;
                _isMoving = false;
                selectedGraphicObject = null;

            }
        }

        private void CaptureAreaToClipboard(Point startingPoint, Point endingPoint)
        {
            double x = Math.Min(startingPoint.X, endingPoint.X);
            double y = Math.Min(startingPoint.Y, endingPoint.Y);
            double width = Math.Abs(startingPoint.X - endingPoint.X);
            double height = Math.Abs(startingPoint.Y - endingPoint.Y);

            if ((int)width == 0 && (int)height == 0)
                return;

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)width,
                (int)height,
                96d, 96d,
                PixelFormats.Pbgra32);

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(drawCanvas);

                drawingContext.DrawRectangle(brush, null, new Rect(new Point(-x, -y), new Size(drawCanvas.ActualWidth, drawCanvas.ActualHeight)));
            }

            renderTargetBitmap.Render(drawingVisual);
            Clipboard.SetImage(renderTargetBitmap);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if (_isDrawing && _currentTool == "Pen")
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
                if (_isDrawing && _currentTool == "Pen")
                {
                    _isPerfectShape = false;
                    ((ShapeObject)_currentGraphicObject!).IsPerfectShape = _isPerfectShape;
                    _currentGraphicObject.UpdateUIElement(_currentUIElement!);
                }
            }
        }

        private void RedrawAll()
        {
            drawCanvas.Children.Clear();
            drawCanvas.Children.Add(_textToolBar);

            foreach (Layer layer in LayerList.Layers)
            {
                foreach (GraphicObject graphicObject in layer.GraphicObjectList)
                {
                    drawCanvas.Children.Add(graphicObject.ConvertToUIElement());
                }
            }
        }

        private void TextToolBarTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string currentText = TextToolBarTextBox.Text;
                if (currentText != String.Empty)
                {
                    TextToolBarTextBox.Text = String.Empty;
                    _textToolBar.Visibility = Visibility.Hidden;

                    GraphicObject textObject = new TextObject((ShapeObject)_currentGraphicObject!, currentText, CurrentTextColor, CurrentTextSize, CurrentTextFont, CurrentTextBackgroundColor);
                    drawCanvas.Children.Add(textObject.ConvertToUIElement());
                    LayerList.GetCurrentLayer()?.AddItem(textObject);
                    List<GraphicObject> textTemp = new List<GraphicObject>();
                    textTemp.Add(textObject);
                    CareTaker.BackupAddAction(textTemp, LayerList.CurrentLayerIndex);
                }
            }
        }

        public static RoutedCommand CopyCommand = new RoutedCommand();

        private void CopyEvent(object sender, RoutedEventArgs e)
        {
            if (_currentTool == "MultipleSelection" && _tempMem != null && _tempMem.Count != 0)
            {
                Trace.WriteLine("Copy");
                _clipboard.Clear();
                _clipboard.AddRange(_tempMem);
            }
        }

        public static RoutedCommand CutCommand = new RoutedCommand();

        private void CutEvent(object sender, RoutedEventArgs e)
        {
            if (_currentTool == "MultipleSelection" && _tempMem != null && _tempMem.Count != 0)
            {
                Trace.WriteLine("Cut");
                _clipboard.Clear();
                _clipboard.AddRange(_tempMem);

                LayerList.GetCurrentLayer()?.RemoveItems(_clipboard);
                CareTaker.BackupDeleteAction(_clipboard, LayerList.CurrentLayerIndex);
                RedrawAll();
            }
        }

        public static RoutedCommand PasteCommand = new RoutedCommand();

        private void PasteEvent(object sender, RoutedEventArgs e)
        {
            if (_clipboard.Count != 0)
            {
                Trace.WriteLine("Paste");
                List<GraphicObject> shapeObjects = new List<GraphicObject>(_clipboard.Where(graphicObject => graphicObject is ShapeObject));
                List<GraphicObject> textObjects = new List<GraphicObject>(_clipboard.Where(graphicObject => graphicObject is TextObject));

                List<GraphicObject> copiedGraphicObjects = new List<GraphicObject>();
                foreach (GraphicObject graphicObject in shapeObjects)
                {
                    GraphicObject copiedGraphicObject = graphicObject.Clone();
                    copiedGraphicObjects.Add(copiedGraphicObject);

                    foreach (GraphicObject textObject in textObjects)
                    {
                        if (((TextObject)textObject).Parent == graphicObject)
                        {
                            GraphicObject copiedTextObject = ((TextObject)textObject).DeepClone((ShapeObject)copiedGraphicObject);
                            copiedGraphicObjects.Add(copiedTextObject);
                        }
                    }
                }

                LayerList.GetCurrentLayer()?.AddItems(copiedGraphicObjects);
                CareTaker.BackupAddAction(copiedGraphicObjects, LayerList.CurrentLayerIndex);
                RedrawAll();
                _clipboard.Clear();
                _clipboard.AddRange(copiedGraphicObjects);
            }
        }

        private void AddLayeBtn_Click(object sender, RoutedEventArgs e)
        {
            Layer tempLayer = new Layer();

            LayerList.AddLayer(tempLayer);
            CareTaker.BackupAddLayerAction(LayerList.GetCurrentLayer()!, LayerList.CurrentLayerIndex);
        }

        private void DeleteLayerBtn_Click(object sender, RoutedEventArgs e)
        {
            CareTaker.BackupDeleteLayerAction(LayerList.GetCurrentLayer()!, LayerList.CurrentLayerIndex);
            LayerList.RemoveLayer(LayerList.CurrentLayerIndex);
            RedrawAll();
        }

        public static RoutedCommand SaveCommand = new RoutedCommand();

        private void SaveEvent(object sender, RoutedEventArgs e)
        {
            if (LayerList.CurrentLayerIndex == -1)
            {
                MessageBox.Show("There is nothing to be saved!", "Nothing to save", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                var dialog = new SaveFileDialog()
                {
                    FileName = "Document",
                    DefaultExt = ".bson",
                    Filter = "BSON files (.bson)|*.bson"
                };

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    string filePath = dialog.FileName;

                    MemoryStream ms = new MemoryStream();
                    using (BsonDataWriter writer = new BsonDataWriter(ms))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.TypeNameHandling = TypeNameHandling.Auto;
                        serializer.Serialize(writer, LayerList);
                    }

                    File.WriteAllBytes(filePath, ms.ToArray());

                    Trace.WriteLine("Saved");
                }
            }
        }

        public static RoutedCommand LoadCommand = new RoutedCommand();

        private void LoadEvent(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "BSON files (.bson)|*.bson"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filePath = dialog.FileName;

                byte[] data = File.ReadAllBytes(filePath);

                MemoryStream ms = new MemoryStream(data);
                using (BsonDataReader reader = new BsonDataReader(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.TypeNameHandling = TypeNameHandling.Auto;
                    LayerList newLayerList = serializer.Deserialize<LayerList>(reader);
                    LayerList = newLayerList;
                    RedrawAll();
                    Trace.WriteLine("Loaded");
                }
            }
        }
        private void LeftRotate_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedGraphicObject != null)
            {
                ((ShapeObject)selectedGraphicObject).RotateAngle -= 90;
                foreach (UIElement element in drawCanvas.Children)
                {
                    // Thực hiện các thao tác trên mỗi phần tử (element) ở đây
                    // Ví dụ: Xoá phần tử khỏi canvas
                    if (element is Shape shapElement)
                    {

                        if (shapElement.Tag == selectedGraphicObject.Id)
                        {
                            ((ShapeObject)selectedGraphicObject).UpdateUIElement(element);
                        }
                    }
                }
                List<GraphicObject> graphicObjects = new List<GraphicObject>();
                graphicObjects.Add(selectedGraphicObject);
                CareTaker.BackupRotateAction(graphicObjects, LayerList.CurrentLayerIndex, "LeftRotate");
            }
        }

        // Hàm xử lý khi RadioButton "rightRotate" được chọn
        private void rightRotate_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedGraphicObject != null)
            {
                ((ShapeObject)selectedGraphicObject).RotateAngle += 90;
                foreach (UIElement element in drawCanvas.Children)
                {
                    // Thực hiện các thao tác trên mỗi phần tử (element) ở đây
                    // Ví dụ: Xoá phần tử khỏi canvas
                    if (element is Shape shapElement)
                    {

                        if (shapElement.Tag == selectedGraphicObject.Id)
                        {
                            ((ShapeObject)selectedGraphicObject).UpdateUIElement(element);
                        }
                    }
                }
                List<GraphicObject> graphicObjects = new List<GraphicObject>();
                graphicObjects.Add(selectedGraphicObject);
                CareTaker.BackupRotateAction(graphicObjects, LayerList.CurrentLayerIndex, "RightRotate");
            }

        }

        // Hàm xử lý khi RadioButton "Horizontal" được chọn
        private void Horizontal_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedGraphicObject != null)
            {
                bool currentHorizontal = ((ShapeObject)selectedGraphicObject).IsHorizontallyFlipped;
                ((ShapeObject)selectedGraphicObject).IsHorizontallyFlipped = !currentHorizontal;
                foreach (UIElement element in drawCanvas.Children)
                {
                    // Thực hiện các thao tác trên mỗi phần tử (element) ở đây
                    // Ví dụ: Xoá phần tử khỏi canvas
                    if (element is Shape shapElement)
                    {

                        if (shapElement.Tag == selectedGraphicObject.Id)
                        {
                            ((ShapeObject)selectedGraphicObject).UpdateUIElement(element);
                        }
                    }
                }
                List<GraphicObject> graphicObjects = new List<GraphicObject>();
                graphicObjects.Add(selectedGraphicObject);
                CareTaker.BackupRotateAction(graphicObjects, LayerList.CurrentLayerIndex, "Horizontal");
            }
        }

        // Hàm xử lý khi RadioButton "Vertical" được chọn
        private void Vertical_Checked(object sender, RoutedEventArgs e)
        {
            if (selectedGraphicObject != null)
            {
                bool currentVertical = ((ShapeObject)selectedGraphicObject).IsVerticallyFlipped;
                ((ShapeObject)selectedGraphicObject).IsVerticallyFlipped = !currentVertical;
                foreach (UIElement element in drawCanvas.Children)
                {
                    // Thực hiện các thao tác trên mỗi phần tử (element) ở đây
                    // Ví dụ: Xoá phần tử khỏi canvas
                    if (element is Shape shapElement)
                    {

                        if (shapElement.Tag == selectedGraphicObject.Id)
                        {
                            ((ShapeObject)selectedGraphicObject).UpdateUIElement(element);
                        }
                    }
                }
                List<GraphicObject> graphicObjects = new List<GraphicObject>();
                graphicObjects.Add(selectedGraphicObject);
                CareTaker.BackupRotateAction(graphicObjects, LayerList.CurrentLayerIndex, "Vertical");
            }
        }
        public static RoutedCommand UndoCommand = new RoutedCommand();

        private void UndoBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý logic khi nút "Undo" được nhấn
            CareTaker.Undo();
            RedrawAll();
        }
        public static RoutedCommand RedoCommand = new RoutedCommand();

        private void RedoBtn_Click(object sender, RoutedEventArgs e)
        {
            // Xử lý logic khi nút "Redo" được nhấn
            CareTaker.Redo();
            RedrawAll();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }
    }
}