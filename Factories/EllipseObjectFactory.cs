using Graphics;
using System.Windows;
using System.Windows.Media;

namespace Factories
{
    public class EllipseObjectFactory : IShapeObjectFactory
    {
        public GraphicObject CreateProduct(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape)
        {
            return new EllipseObject(startingPoint, endingPoint, strokeColor, strokeThickness, strokeType, rotateAngle, isHorizontallyFlipped, isVerticallyFlipped, isPerfectShape);
        }

        public string GetName()
        {
            return "Ellipse";
        }

        public int GetPriority()
        {
            return 1;
        }
    }
}
