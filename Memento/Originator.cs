using Graphics;
using LayerManager;

namespace Memento
{
    /// <summary>
    /// A class storing state that changes over time (in this context, it's the LayerList object), together with
    /// functions to create snapshots (Memento) and restore/revert the state from a snapshot
    /// </summary>
    public class Originator
    {
        // The state of this Originator
        private LayerList _layerList;

        public LayerList LayerList
        {
            get { return _layerList; }
        }

        public Originator(LayerList layerList)
        {
            _layerList = layerList;
        }

        public IMemento SaveAddAction(List<GraphicObject> addedGraphicObjects, int layerIdx)
        {
            return new AddActionMemento(addedGraphicObjects, layerIdx);
        }

        public IMemento SaveDeleteAction(List<GraphicObject> deletedGraphicObjects, int layerIdx)
        {
            return new DeleteActionMemento(deletedGraphicObjects, layerIdx);
        }

        public IMemento SaveAddLayerAction(Layer addedLayer, int addedIdx)
        {
            return new AddLayerActionMemento(addedLayer, addedIdx);
        }

        public IMemento SaveDeleteLayerAction(Layer deletedLayer, int deletedIdx)
        {
            return new DeleteLayerActionMemento(deletedLayer, deletedIdx);
        }

        /// <summary>
        /// Restore the state based on the given IMemento object
        /// </summary>
        /// <param name="memento">The memento used to restore the state</param>
        public void Restore(IMemento memento)
        {
            switch(memento.GetName())
            {
                case "Add":
                    {
                        (List<GraphicObject> AddedGraphicObjects, int LayerIdx) state = ((List<GraphicObject> AddedGraphicObjects, int LayerIdx))memento.GetState();
                        _layerList.GetLayerAtIndex(state.LayerIdx)?.RemoveItems(state.AddedGraphicObjects);
                    }
                    break;

                case "Delete":
                    {
                        (List<GraphicObject> DeletedGraphicObjects, int LayerIdx) state = ((List<GraphicObject> DeletedGraphicObjects, int LayerIdx))memento.GetState();
                        _layerList.GetLayerAtIndex(state.LayerIdx)?.AddItems(state.DeletedGraphicObjects);
                    }
                    break;

                case "AddLayer":
                    {
                        (Layer AddedLayer, int AddedIdx) state = ((Layer AddedLayer, int AddedIdx))memento.GetState();
                        _layerList.RemoveLayer(state.AddedIdx);
                    }
                    break;

                case "DeleteLayer":
                    {
                        (Layer DeletedLayer, int DeletedIdx) state = ((Layer DeletedLayer, int DeletedIdx))memento.GetState();
                        _layerList.AddLayerAtIndex(state.DeletedLayer, state.DeletedIdx);
                    }
                    break;

                default:
                    throw new Exception("Unknown memento: " + memento.GetName());
            }
        }

        /// <summary>
        /// Revert the state based on the given IMemento object
        /// </summary>
        /// <param name="memento">The memento used to revert the state</param>
        public void Revert(IMemento memento)
        {
            switch (memento.GetName())
            {
                case "Add":
                    {
                        (List<GraphicObject> AddedGraphicObjects, int LayerIdx) state = ((List<GraphicObject> AddedGraphicObjects, int LayerIdx))memento.GetState();
                        _layerList.GetLayerAtIndex(state.LayerIdx)?.AddItems(state.AddedGraphicObjects);
                    }
                    break;

                case "Delete":
                    {
                        (List<GraphicObject> DeletedGraphicObjects, int LayerIdx) state = ((List<GraphicObject> DeletedGraphicObjects, int LayerIdx))memento.GetState();
                        _layerList.GetLayerAtIndex(state.LayerIdx)?.RemoveItems(state.DeletedGraphicObjects);
                    }
                    break;

                case "AddLayer":
                    {
                        (Layer AddedLayer, int AddedIdx) state = ((Layer AddedLayer, int AddedIdx))memento.GetState();
                        _layerList.AddLayerAtIndex(state.AddedLayer, state.AddedIdx);
                    }
                    break;

                case "DeleteLayer":
                    {
                        (Layer DeletedLayer, int DeletedIdx) state = ((Layer DeletedLayer, int DeletedIdx))memento.GetState();
                        _layerList.RemoveLayer(state.DeletedIdx);
                    }
                    break;

                default:
                    throw new Exception("Unknown memento: " + memento.GetName());
            }
        }
    }
}
