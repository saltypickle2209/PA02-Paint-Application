using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics
{
    public class LineObject : ShapeObject
    {
        public LineObject(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape) 
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
            UIElement line = new Line
            {
                X1 = _startingPoint.X,
                Y1 = _startingPoint.Y,
                X2 = _endingPoint.X,
                Y2 = _endingPoint.Y,
                Stroke = _strokeColor,
                StrokeThickness = _strokeThickness,
                StrokeDashArray = _strokeType.GetStrokeDashArray(),
                StrokeDashCap = _strokeType.GetPenLineCap(),
                Tag = _id
            };

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

            line.RenderTransform = transformGroup;

            return line;
        }

        public override void UpdateUIElement(UIElement element)
        {
            if(element is Line line)
            {
                if ((string)line.Tag != _id)
                    return;
                else
                {
                    line.X1 = _startingPoint.X;
                    line.Y1 = _startingPoint.Y;
                    line.X2 = _endingPoint.X;
                    line.Y2 = _endingPoint.Y;

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

                    line.RenderTransform = transformGroup;
                }
            }
        }
    }
}
