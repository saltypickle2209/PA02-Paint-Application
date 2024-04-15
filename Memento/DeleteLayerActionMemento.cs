using LayerManager;

namespace Memento
{
    public class DeleteLayerActionMemento : IMemento
    {
        private Layer _deletedLayer;
        private int _deletedIdx;

        public DeleteLayerActionMemento(Layer deletedLayer, int deletedIdx)
        {
            _deletedLayer = deletedLayer;
            _deletedIdx = deletedIdx;
        }

        public string GetName()
        {
            return "DeleteLayer";
        }

        public object GetState()
        {
            return (_deletedLayer, _deletedIdx);
        }
    }
}
