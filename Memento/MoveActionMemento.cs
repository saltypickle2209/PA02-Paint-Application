using Graphics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento
{
    public class MoveActionMemento : IMemento
    {
        private List<GraphicObject> _moveGraphicObjects;
        private Point _beforeStartPoint;
        private Point _afterStartPoint;
        private Point _beforeEndPoint;
        private Point _afterEndPoint;
        private int _layerIdx;
        public MoveActionMemento(List<GraphicObject> movedGraphicObjects, int layerIdx, Point afterStartPoint, Point beforeEndPoint, Point afterEndPoint, Point beforeStartPoint)
        {
            _moveGraphicObjects = movedGraphicObjects;
            _layerIdx = layerIdx;
            _afterStartPoint = afterStartPoint;
            _beforeEndPoint = beforeEndPoint;
            _afterEndPoint = afterEndPoint;
            _beforeStartPoint = beforeStartPoint;
        }
        public string GetName()
        {
            return "MoveAction";
        }

        public object GetState()
        {
            return (_moveGraphicObjects, _layerIdx);
        }
        public object GetStateBefore()
        {
            return (_moveGraphicObjects, _layerIdx, _beforeStartPoint, _beforeEndPoint);

        }
        public object GetStateAfter()
        {
            return (_moveGraphicObjects, _layerIdx, _afterStartPoint, _afterEndPoint);

        }
    }
}
