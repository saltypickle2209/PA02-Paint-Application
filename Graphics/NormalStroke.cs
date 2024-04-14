using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graphics
{
    public class NormalStroke : IStrokeType
    {
        public PenLineCap GetPenLineCap()
        {
            return PenLineCap.Flat;
        }

        public DoubleCollection GetStrokeDashArray()
        {
            return [];
        }
    }
}
