using LayerManager;

namespace Memento
{
    public class AddLayerActionMemento : IMemento
    {
        private Layer _addedLayer;
        private int _addedIdx;

        public AddLayerActionMemento(Layer addedLayer, int addedIdx)
        {
            _addedLayer = addedLayer;
            _addedIdx = addedIdx;
        }

        public string GetName()
        {
            return "AddLayer";
        }

        public object GetState()
        {
            return (_addedLayer, _addedIdx);
        }
    }
}
