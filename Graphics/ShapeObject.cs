using System.Windows;
using System.Windows.Media;

namespace Graphics
{
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
        protected int _strokeThickness;
        protected IStrokeType _strokeType;
        protected double _rotateAngle;
        protected bool _isHorizontallyFlipped;
        protected bool _isVerticallyFlipped;
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
