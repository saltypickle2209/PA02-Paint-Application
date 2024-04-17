using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graphics
{
    public class DotStroke : IStrokeType
    {
        public PenLineCap GetPenLineCap()
        {
            return PenLineCap.Round;
        }

        public DoubleCollection GetStrokeDashArray()
        {
            return new DoubleCollection([0, 3]);
        }

        public int GetPriority()
        {
            return 1;
        }
    }
}
