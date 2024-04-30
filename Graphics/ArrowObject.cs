using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics
{
    public class ArrowObject : ShapeObject
    {
        // arrow-shaped SVG path
        private readonly string _pathData = "M 0 7.8334 v 6.719 c 0 0.813 0.594 1.406 1.438 1.406 h 7.813 v 5.375 c 0 0.5 0.219 0.813 0.688 1.031 c 0.125 0.031 0.281 0.063 0.406 0.063 c 0.313 0 0.563 -0.094 0.781 -0.313 l 10.094 -10.156 c 0.438 -0.375 0.438 -1.125 0 -1.563 l -10.094 -10.063 c -0.625 -0.688 -1.875 -0.25 -1.875 0.781 v 5.344 h -7.813 c -0.844 0 -1.438 0.563 -1.438 1.375 z";

        public ArrowObject(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape) 
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

            UIElement arrow = new Path
            {
                Width = width,
                Height = height,
                Stroke = _strokeColor,
                StrokeThickness = _strokeThickness,
                StrokeDashArray = _strokeType.GetStrokeDashArray(),
                StrokeDashCap = _strokeType.GetPenLineCap(),
                Data = PathGeometry.CreateFromGeometry(Geometry.Parse(_pathData)),
                Stretch = Stretch.Fill,
                Tag = _id
            };

            arrow.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
            arrow.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform(_rotateAngle, width / 2, height / 2));
            transformGroup.Children.Add(new ScaleTransform(
                _isHorizontallyFlipped ? -1 : 1,
                _isVerticallyFlipped ? -1 : 1,
                width / 2,
                height / 2
            ));

            arrow.RenderTransform = transformGroup;

            return arrow;
        }

        public override void UpdateUIElement(UIElement element)
        {
            if (element is Path arrow)
            {
                if ((string)arrow.Tag != _id)
                    return;

                double width = Math.Abs(_startingPoint.X - _endingPoint.X);
                double height = Math.Abs(_startingPoint.Y - _endingPoint.Y);

                if (_isPerfectShape)
                    width = height = Math.Min(width, height);

                arrow.Width = width;
                arrow.Height = height;

                arrow.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
                arrow.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(_rotateAngle, width / 2, height / 2));
                transformGroup.Children.Add(new ScaleTransform(
                    _isHorizontallyFlipped ? -1 : 1,
                    _isVerticallyFlipped ? -1 : 1,
                    width / 2,
                    height / 2
                ));

                arrow.RenderTransform = transformGroup;
            }
        }
    }
}
