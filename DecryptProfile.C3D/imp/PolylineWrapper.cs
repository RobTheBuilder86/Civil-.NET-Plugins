using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Publishing;
using DeconstructSurfaceSampleView.Engine.HelperObjects;
using DeconstructSurfaceSampleView.Engine.Interfaces;
using System.Collections.Generic;

namespace DeconstructSurfaceSampleView.C3D.imp
{
    class PolylineWrapper : IPolyline
    {
        private Polyline _polyline;

        public PolylineWrapper(Polyline polyline) =>
            _polyline = polyline;

        public List<SimplePoint2d> GetPoints()
        {
            var points = new List<SimplePoint2d>();
            for (int i = 0; i < _polyline.NumberOfVertices; i++) {
                Point2d vertex = _polyline.GetPoint2dAt(i);
                SimplePoint2d newPoint = new SimplePoint2d(vertex.X, vertex.Y);
                points.Add(newPoint);
            }
            return points;
        }
    }
}
