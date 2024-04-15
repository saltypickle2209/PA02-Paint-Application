using Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Factories
{
    public class LineObjectFactory : IShapeObjectFactory
    {
        public GraphicObject CreateProduct(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape)
        {
            return new LineObject(startingPoint, endingPoint, strokeColor, strokeThickness, strokeType, rotateAngle, isHorizontallyFlipped, isVerticallyFlipped, isPerfectShape);
        }

        public string GetName()
        {
            return "Line";
        }
    }
}
