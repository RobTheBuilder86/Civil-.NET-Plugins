using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using CreatePolyFromAlignment.Engine.Interfaces;
using System;

namespace CreatePolyFromAlignment.C3D.imp
{
    internal class PolylineWrapper : IPolyline
    {
        public Polyline3d AcadPolyline { get; }
        public PolylineWrapper(Polyline3d polyline) => AcadPolyline = polyline;

        public void AddVertexAt(double x, double y, double z)
        {
            var newVertexPoint = new Point3d(x, y, z);
            using (var newVertex = new PolylineVertex3d(newVertexPoint)) {
                AcadPolyline.AppendVertex(newVertex);
            }
        }
    }
}
