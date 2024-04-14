using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics
{
    public class StarObject : ShapeObject
    {
        // star-shaped SVG path
        private readonly string _pathData = "M 152.176 723.546 L 380.336 603.482 l 228.16 119.936 a 6.4 6.4 0 0 0 9.28 -6.72 l -43.52 -254.08 l 184.512 -179.904 a 6.4 6.4 0 0 0 -3.52 -10.88 l -255.104 -37.12 L 386.096 3.61 a 6.4 6.4 0 0 0 -11.52 0 L 260.528 234.778 l -255.104 37.12 a 6.4 6.4 0 0 0 -3.52 10.88 L 186.416 462.682 l -43.584 254.08 a 6.4 6.4 0 0 0 9.28 6.72 z";

        public StarObject(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape) 
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

            UIElement star = new Path
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

            star.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
            star.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform(_rotateAngle, width / 2, height / 2));
            transformGroup.Children.Add(new ScaleTransform(
                _isHorizontallyFlipped ? -1 : 1,
                _isVerticallyFlipped ? -1 : 1,
                width / 2,
                height / 2
            ));

            star.RenderTransform = transformGroup;

            return star;
        }

        public override void UpdateUIElement(UIElement element)
        {
            if (element is Path star)
            {
                if ((string)star.Tag != _id)
                    return;

                double width = Math.Abs(_startingPoint.X - _endingPoint.X);
                double height = Math.Abs(_startingPoint.Y - _endingPoint.Y);

                if (_isPerfectShape)
                    width = height = Math.Min(width, height);

                star.Width = width;
                star.Height = height;

                star.SetValue(Canvas.LeftProperty, _startingPoint.X < _endingPoint.X ? _startingPoint.X : _startingPoint.X - width);
                star.SetValue(Canvas.TopProperty, _startingPoint.Y < _endingPoint.Y ? _startingPoint.Y : _startingPoint.Y - height);

                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(_rotateAngle, width / 2, height / 2));
                transformGroup.Children.Add(new ScaleTransform(
                    _isHorizontallyFlipped ? -1 : 1,
                    _isVerticallyFlipped ? -1 : 1,
                    width / 2,
                    height / 2
                ));

                star.RenderTransform = transformGroup;
            }
        }
    }
}
