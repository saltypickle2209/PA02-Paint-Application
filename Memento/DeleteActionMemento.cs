using Graphics;

namespace Memento
{
    public class DeleteActionMemento : IMemento
    {
        private List<GraphicObject> _deletedGraphicObjects;

        private int _layerIdx;

        public DeleteActionMemento(List<GraphicObject> deletedGraphicObjects, int layerIdx)
        {
            _deletedGraphicObjects = deletedGraphicObjects;
            _layerIdx = layerIdx;
        }

        public string GetName()
        {
            return "Delete";
        }

        public object GetState()
        {
            return (_deletedGraphicObjects, _layerIdx);
        }
    }
}
