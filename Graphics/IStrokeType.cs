using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graphics
{
    /// <summary>
    /// Defines the stroke type of a ShapeObject (eg. dashed line, dotted line,...)
    /// </summary>
    public interface IStrokeType
    {
        DoubleCollection GetStrokeDashArray();
        PenLineCap GetPenLineCap();
    }
}
