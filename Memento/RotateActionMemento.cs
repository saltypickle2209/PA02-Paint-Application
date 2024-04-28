using Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento
{
    internal class RotateActionMemento : IMemento
    {
        private List<GraphicObject> _rotateGraphicObjects;
        private int _layerIdx;
        private string type;

        public RotateActionMemento(List<GraphicObject> RotateGraphicObjects, int layerIdx, string typeRotate)
        {
            _rotateGraphicObjects = RotateGraphicObjects;
            _layerIdx = layerIdx;
            type = typeRotate;
        }
        public string GetName()
        {
            return "RotateAction";
        }

        public object GetState()
        {
            return (_rotateGraphicObjects, _layerIdx, type);
        }
    }
}
