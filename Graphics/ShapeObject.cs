using System.Windows;
using System.Windows.Media;

namespace Graphics
{
    /// <summary>
    /// A ShapeObject contains every attribute needed to create a Shape (System.Windows.Shape).
    /// </summary>
    public abstract class ShapeObject : GraphicObject
    {
        // Base attributes of a ShapeObject
        protected Point _startingPoint;

        public Point StartingPoint { 
            get { return _startingPoint; }
            set { _startingPoint = value;  } 
        }

        protected Point _endingPoint;

        public Point EndingPoint
        {
            get { return _endingPoint; }
            set { _endingPoint = value; }
        }

        protected SolidColorBrush _strokeColor;

        public SolidColorBrush StrokeColor
        {
            get { return _strokeColor; }
            set { _strokeColor = value; }
        }

        protected int _strokeThickness;

        public int StrokeThickness
        {
            get { return _strokeThickness; }
            set { _strokeThickness = value; }
        }

        protected IStrokeType _strokeType;

        public IStrokeType StrokeType
        {
            get { return _strokeType; }
            set { _strokeType = value; }
        }

        protected double _rotateAngle;

        public double RotateAngle
        {
            get { return _rotateAngle; }
            set { _rotateAngle = value; }
        }

        protected bool _isHorizontallyFlipped;

        public bool IsHorizontallyFlipped
        {
            get { return _isHorizontallyFlipped; }
            set { _isHorizontallyFlipped = value; }
        }

        protected bool _isVerticallyFlipped;
        public bool IsVerticallyFlipped
        {
            get { return _isVerticallyFlipped; }
            set { _isVerticallyFlipped = value; }
        }

        protected bool _isPerfectShape;

        public bool IsPerfectShape
        {
            get { return _isPerfectShape;  }
            set { _isPerfectShape = value; }
        }

        public ShapeObject(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape) : base()
        {
            _startingPoint = startingPoint;
            _endingPoint = endingPoint;
            _strokeColor = strokeColor;
            _strokeThickness = strokeThickness;
            _strokeType = strokeType;
            _rotateAngle = rotateAngle;
            _isHorizontallyFlipped = isHorizontallyFlipped;
            _isVerticallyFlipped = isVerticallyFlipped;
            _isPerfectShape = isPerfectShape;
        }
    }
}
