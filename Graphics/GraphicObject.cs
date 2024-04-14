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

        // Prototype design pattern
        public abstract GraphicObject Clone();

        // Convert to a UIElement (eg. Line, Rectangle, TextBlock,...)
        public abstract UIElement ConvertToUIElement();

        // Update an existing UIElement in the canvas
        public abstract void UpdateUIElement(UIElement element);
    }

}
