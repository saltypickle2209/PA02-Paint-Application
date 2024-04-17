using Graphics;
using System.Windows;
using System.Windows.Media;

namespace Factories
{
    public class RectangleObjectFactory : IShapeObjectFactory
    {
        public GraphicObject CreateProduct(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape)
        {
            return new RectangleObject(startingPoint, endingPoint, strokeColor, strokeThickness, strokeType, rotateAngle, isHorizontallyFlipped, isVerticallyFlipped, isPerfectShape);
        }

        public string GetName()
        {
            return "Rectangle";
        }

        public int GetPriority()
        {
            return 1;
        }
    }
}
