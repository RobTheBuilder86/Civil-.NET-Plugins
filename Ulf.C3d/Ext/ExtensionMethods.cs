using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ulf.Util;

namespace Ulf.C3D.Ext
{
    internal static class ExtensionMethods
    {
        internal static Point2d ToPoint2d(this SimplePoint2d point)
        {
            return new Point2d(point.X, point.Y);
        }

        internal static Point3d ToPoint3d(this CaseStation cs)
        {
            return new Point3d(cs.X, cs.Y, 0);
        }
    }
}
