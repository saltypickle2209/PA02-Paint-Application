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
        /// <summary>
        /// Return a DoubleCollection representing a stroke's dash array
        /// </summary>
        /// <returns>A DoubleCollection describing the stroke</returns>
        DoubleCollection GetStrokeDashArray();

        /// <summary>
        /// Return a PenLineCap representing the cap between two ends of a stroke
        /// </summary>
        /// <returns>A PenLineCap enum</returns>
        PenLineCap GetPenLineCap();

        /// <summary>
        /// Get the number telling the priority of the implemented class in a list, starting with 0. Used for sorting a list
        /// with multiple objects of type IStrokeType
        /// </summary>
        /// <returns>An interger indicating the priority</returns>
        int GetPriority();
    }
}
