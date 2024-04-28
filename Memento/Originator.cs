using Graphics;
using LayerManager;
using System.Windows;

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
        public IMemento SaveMoveAction(List<GraphicObject> movedGraphicObjects, int layerIdx, Point afterStartPoint, Point beforeEndPoint, Point afterEndPoint, Point beforeStartPoint)

        {
            return new MoveActionMemento(movedGraphicObjects, layerIdx, afterStartPoint, afterEndPoint, beforeEndPoint, beforeStartPoint);
        }
        public IMemento SaveRotateAction(List<GraphicObject> movedGraphicObjects, int layerIdx, string TypeRotate)

        {
            return new RotateActionMemento(movedGraphicObjects, layerIdx, TypeRotate);
        }

        /// <summary>
        /// Restore the state based on the given IMemento object
        /// </summary>
        /// <param name="memento">The memento used to restore the state</param>
        public void Restore(IMemento memento)
        {
            switch (memento.GetName())
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
                case "MoveAction":
                    {
                        (List<GraphicObject> MovedGraphicObjects, int LayerIdx, Point startBefore, Point endBefore) state = ((List<GraphicObject> MovedGraphicObjects, int LayerIdx, Point startBefore, Point endBefore))((MoveActionMemento)memento).GetStateBefore();
                        _layerList.GetLayerAtIndex(state.LayerIdx)?.UpdateItem(state.MovedGraphicObjects, state.startBefore, state.endBefore);
                    }
                    break;
                case "RotateAction":
                    {
                        (List<GraphicObject> MovedGraphicObjects, int LayerIdx, string type) state = ((List<GraphicObject> MovedGraphicObjects, int LayerIdx, string type))((RotateActionMemento)memento).GetState();
                        if (state.type == "LeftRotate")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            updateGraphicObject.RotateAngle += 90;
                        }
                        else if (state.type == "RightRotate")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            updateGraphicObject.RotateAngle -= 90;
                        }
                        else if (state.type == "Horizontal")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            bool currentState = updateGraphicObject.IsHorizontallyFlipped;
                            updateGraphicObject.IsHorizontallyFlipped = !currentState;
                        }
                        else if (state.type == "Vertical")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            bool currentState = updateGraphicObject.IsVerticallyFlipped;
                            updateGraphicObject.IsVerticallyFlipped = !currentState;
                        }
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
                case "MoveAction":
                    {
                        (List<GraphicObject> MovedGraphicObjects, int LayerIdx, Point startAfter, Point endAfter) state = ((List<GraphicObject> MovedGraphicObjects, int LayerIdx, Point startAfter, Point endAfter))((MoveActionMemento)memento).GetStateAfter();
                        _layerList.GetLayerAtIndex(state.LayerIdx)?.UpdateItem(state.MovedGraphicObjects, state.startAfter, state.endAfter);
                    }
                    break;
                case "RotateAction":
                    {
                        (List<GraphicObject> MovedGraphicObjects, int LayerIdx, string type) state = ((List<GraphicObject> MovedGraphicObjects, int LayerIdx, string type))((RotateActionMemento)memento).GetState();
                        if (state.type == "LeftRotate")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            updateGraphicObject.RotateAngle -= 90;
                        }
                        else if (state.type == "RightRotate")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            updateGraphicObject.RotateAngle += 90;
                        }
                        else if (state.type == "Horizontal")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            bool currentState = updateGraphicObject.IsHorizontallyFlipped;
                            updateGraphicObject.IsHorizontallyFlipped = !currentState;
                        }
                        else if (state.type == "Vertical")
                        {
                            ShapeObject updateGraphicObject = (ShapeObject)state.MovedGraphicObjects[0];
                            bool currentState = updateGraphicObject.IsVerticallyFlipped;
                            updateGraphicObject.IsVerticallyFlipped = !currentState;
                        }
                    }
                    break;
                default:
                    throw new Exception("Unknown memento: " + memento.GetName());
            }
        }
    }
}
