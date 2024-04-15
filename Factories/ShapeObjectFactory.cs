using Graphics;
using System.Windows;
using System.Windows.Media;

namespace Factories
{
    /// <summary>
    /// An interface for a factory that produces GraphicObject products, but all of the products is of the 
    /// ShapeObject subclass.
    /// </summary>
    public interface IShapeObjectFactory
    {
        /// <summary>
        /// Create a ShapeObject and return it as a GraphicObject. Need to provide some info of the shape to be drawn.
        /// </summary>
        /// <param name="startingPoint">The starting point of the shape</param>
        /// <param name="endingPoint">The ending point of the shape</param>
        /// <param name="strokeColor">The stroke's color</param>
        /// <param name="strokeThickness">The stroke's thickness</param>
        /// <param name="strokeType">The stroke type (eg. for normal stroke, use new NormalStroke())</param>
        /// <param name="rotateAngle">The rotate angle of the shape</param>
        /// <param name="isHorizontallyFlipped">Decides whether the shape should be horizontally flipped or not</param>
        /// <param name="isVerticallyFlipped">Decides whether the shape should be vertically flipped or not</param>
        /// <param name="isPerfectShape">Decides whether the shape is a perfect shape (eg. Square, Circle) or not</param>
        /// <returns>A GraphicObject</returns>
        GraphicObject CreateProduct(Point startingPoint, Point endingPoint, SolidColorBrush strokeColor, int strokeThickness, IStrokeType strokeType, double rotateAngle, bool isHorizontallyFlipped, bool isVerticallyFlipped, bool isPerfectShape);

        /// <summary>
        /// Get the name of the factory.
        /// </summary>
        /// <returns>The string representing the factory's name</returns>
        string GetName();
    }
}
