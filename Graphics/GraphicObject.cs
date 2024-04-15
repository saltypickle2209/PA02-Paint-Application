using System.Windows;

namespace Graphics
{
    public abstract class GraphicObject
    {
        // Each GraphicObject must have a unique ID
        protected string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public GraphicObject()
        {
            _id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Prototype design pattern, used to make a clone of a GraphicObject
        /// </summary>
        /// <returns>A GraphicObject cloned from the original (this). It should have a new id assigned.</returns>
        public abstract GraphicObject Clone();

        /// <summary>
        /// Create a UIElement from a GraphicObject
        /// </summary>
        /// <returns>A UIElement (eg. Line, Rectangle, TextBlock,...) correspond to the type of GraphicObject.</returns>
        public abstract UIElement ConvertToUIElement();

        /// <summary>
        /// Update an existing UIElement in the canvas
        /// </summary>
        /// <param name="element">The UIElement associated to this GraphicObject. Make sure your subclasses match this UIElement's Tag to the GraphicObject's id before updating.</param>
        public abstract void UpdateUIElement(UIElement element);
    }

}
