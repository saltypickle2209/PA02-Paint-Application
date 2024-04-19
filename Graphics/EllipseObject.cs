using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics
{
    public class EllipseObject : ShapeObject
    {
        public EllipseObject(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape) 
            : base(startingPoint, endingPoint, strokeColor, strokeThickness, strokeType, rotateAngle, isHorizontallyFlipped, isVerticallyFlipped, isPerfectShape)
        {
        }

        public override GraphicObject Clone()
        {
            GraphicObject graphicObject = (GraphicObject)this.MemberwiseClone();

            ((ShapeObject)graphicObject).StartingPoint = new Point(StartingPoint.X + 10, StartingPoint.Y + 10);
            ((ShapeObject)graphicObject).EndingPoint = new Point(EndingPoint.X + 10, EndingPoint.Y + 10);

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

            UIElement ellipse = new Ellipse
            {
                Width = width,
                Height = height,
                Stroke = _strokeColor,
                StrokeThickness = _strokeThickness,
                StrokeDashArray = _strokeType.GetStrokeDashArray(),
                StrokeDashCap = _strokeType.GetPenLineCap(),
                Tag = _id
            };

            ellipse.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
            ellipse.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform(_rotateAngle, width / 2, height / 2));
            transformGroup.Children.Add(new ScaleTransform(
                _isHorizontallyFlipped ? -1 : 1,
                _isVerticallyFlipped ? -1 : 1,
                width / 2,
                height / 2
            ));

            ellipse.RenderTransform = transformGroup;

            return ellipse;
        }

        public override void UpdateUIElement(UIElement element)
        {
            if (element is Ellipse ellipse)
            {
                if ((string)ellipse.Tag != _id)
                    return;

                double width = Math.Abs(_startingPoint.X - _endingPoint.X);
                double height = Math.Abs(_startingPoint.Y - _endingPoint.Y);

                if (_isPerfectShape)
                    width = height = Math.Min(width, height);

                ellipse.Width = width;
                ellipse.Height = height;

                ellipse.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
                ellipse.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(_rotateAngle, width / 2, height / 2));
                transformGroup.Children.Add(new ScaleTransform(
                    _isHorizontallyFlipped ? -1 : 1,
                    _isVerticallyFlipped ? -1 : 1,
                    width / 2,
                    height / 2
                ));

                ellipse.RenderTransform = transformGroup;
            }
        }
    }
}
