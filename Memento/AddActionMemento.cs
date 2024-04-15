using Graphics;

namespace Memento
{
    public class AddActionMemento : IMemento
    {
        private List<GraphicObject> _addedGraphicObjects;

        private int _layerIdx;

        public AddActionMemento(List<GraphicObject> addedGraphicObjects, int layerIdx)
        {
            _addedGraphicObjects = addedGraphicObjects;
            _layerIdx = layerIdx;
        }

        public string GetName()
        {
            return "Add";
        }

        public object GetState()
        {
            return (_addedGraphicObjects, _layerIdx);
        }
    }
}
