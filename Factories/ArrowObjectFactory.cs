﻿using Graphics;
using System.Windows;
using System.Windows.Media;

namespace Factories
{
    public class ArrowObjectFactory : IShapeObjectFactory
    {
        public GraphicObject CreateProduct(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape)
        {
            return new ArrowObject(startingPoint, endingPoint, strokeColor, strokeThickness, strokeType, rotateAngle, isHorizontallyFlipped, isVerticallyFlipped, isPerfectShape);
        }

        public string GetName()
        {
            return "Arrow";
        }

        public int GetPriority()
        {
            return 2;
        }
    }
}
