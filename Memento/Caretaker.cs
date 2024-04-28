using Graphics;
using LayerManager;
using System.Windows;

namespace Memento
{
    /// <summary>
    /// Caretaker isn't in charge of changing the state of originator but of storing the snapshots created
    /// by originator. Undo()/Redo() is used to retrieve the snapshot and request for a change from the
    /// originator.
    /// </summary>
    public class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();
        private int _currentMementoIdx = -1;
        private Originator _originator;

        public Caretaker(Originator originator)
        {
            _originator = originator;
        }

        public void BackupAddAction(List<GraphicObject> addedGraphicObjects, int layerIdx)
        {
            _mementos = _mementos.Take(_currentMementoIdx + 1).ToList();
            _mementos.Add(_originator.SaveAddAction(addedGraphicObjects, layerIdx));
            _currentMementoIdx = _mementos.Count - 1;
        }

        public void BackupDeleteAction(List<GraphicObject> deletedGraphicObjects, int layerIdx)
        {
            _mementos = _mementos.Take(_currentMementoIdx + 1).ToList();
            _mementos.Add(_originator.SaveDeleteAction(deletedGraphicObjects, layerIdx));
            _currentMementoIdx = _mementos.Count - 1;
        }
        public void BackupMoveAction(List<GraphicObject> deletedGraphicObjects, int layerIdx, Point afterStartPoint, Point beforeEndPoint, Point afterEndPoint, Point beforeStartPoint)
        {
            _mementos = _mementos.Take(_currentMementoIdx + 1).ToList();
            _mementos.Add(_originator.SaveMoveAction(deletedGraphicObjects, layerIdx, afterStartPoint, beforeEndPoint, afterEndPoint, beforeStartPoint));
            _currentMementoIdx = _mementos.Count - 1;
        }
        public void BackupRotateAction(List<GraphicObject> deletedGraphicObjects, int layerIdx, string type)
        {
            _mementos = _mementos.Take(_currentMementoIdx + 1).ToList();
            _mementos.Add(_originator.SaveRotateAction(deletedGraphicObjects, layerIdx, type));
            _currentMementoIdx = _mementos.Count - 1;
        }

        public void BackupAddLayerAction(Layer addedLayer, int addedIdx)
        {
            _mementos = _mementos.Take(_currentMementoIdx + 1).ToList();
            _mementos.Add(_originator.SaveAddLayerAction(addedLayer, addedIdx));
            _currentMementoIdx = _mementos.Count - 1;
        }

        public void BackupDeleteLayerAction(Layer deletedLayer, int deletedIdx)
        {
            _mementos = _mementos.Take(_currentMementoIdx + 1).ToList();
            _mementos.Add(_originator.SaveDeleteLayerAction(deletedLayer, deletedIdx));
            _currentMementoIdx = _mementos.Count - 1;
        }

        public void Undo()
        {
            if (_currentMementoIdx >= 0)
            {
                IMemento memento = _mementos[_currentMementoIdx];
                _currentMementoIdx--;

                try
                {
                    _originator.Restore(memento);
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Redo()
        {
            if (_currentMementoIdx < _mementos.Count - 1)
            {
                IMemento memento = _mementos[++_currentMementoIdx];

                try
                {
                    _originator.Revert(memento);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
