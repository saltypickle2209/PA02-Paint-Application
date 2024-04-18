using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics
{
    public class RectangleObject : ShapeObject
    {
        public RectangleObject(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape) 
            : base(startingPoint, endingPoint, strokeColor, strokeThickness, strokeType, rotateAngle, isHorizontallyFlipped, isVerticallyFlipped, isPerfectShape)
        {
        }

        public override GraphicObject Clone()
        {
            GraphicObject graphicObject = (GraphicObject)this.MemberwiseClone();

            // Assign a new ID
            graphicObject.Id = Guid.NewGuid().ToString();

            return graphicObject;
        }

        public override UIElement ConvertToUIElement()
        {
            double width = Math.Abs(_startingPoint.X - _endingPoint.X);
            double height = Math.Abs(_startingPoint.Y - _endingPoint.Y);

            if (_isPerfectShape)
                width = height = Math.Min(width, height);

            UIElement rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                Stroke = _strokeColor,
                StrokeThickness = _strokeThickness,
                StrokeDashArray = _strokeType.GetStrokeDashArray(),
                StrokeDashCap = _strokeType.GetPenLineCap(),
                Tag = _id
            };

            rectangle.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
            rectangle.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform(_rotateAngle, width / 2, height / 2));
            transformGroup.Children.Add(new ScaleTransform(
                _isHorizontallyFlipped ? -1 : 1,
                _isVerticallyFlipped ? -1 : 1,
                width / 2,
                height / 2
            ));

            rectangle.RenderTransform = transformGroup;

            return rectangle;
        }

        public override void UpdateUIElement(UIElement element)
        {
            if(element is Rectangle rectangle)
            {
                if ((string)rectangle.Tag != _id)
                    return;

                double width = Math.Abs(_startingPoint.X - _endingPoint.X);
                double height = Math.Abs(_startingPoint.Y - _endingPoint.Y);

                if (_isPerfectShape)
                    width = height = Math.Min(width, height);

                rectangle.Width = width;
                rectangle.Height = height;

                rectangle.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
                rectangle.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

                double centerX = (_startingPoint.X + _endingPoint.X) / 2;
                double centerY = (_startingPoint.Y + _endingPoint.Y) / 2;

                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(_rotateAngle, centerX, centerY));
                transformGroup.Children.Add(new ScaleTransform(
                    _isHorizontallyFlipped ? -1 : 1,
                    _isVerticallyFlipped ? -1 : 1,
                    centerX,
                    centerY
                ));

                rectangle.RenderTransform = transformGroup;
            }
        }
    }
}
