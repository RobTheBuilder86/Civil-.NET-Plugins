using SurpressSurface.Engine;
using System.Collections.Generic;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace SurpressSurface.C3D {
    public class SurfaceWrapper : ISurface {
        public readonly TinSurface _surface;

        public SurfaceWrapper(TinSurface surface) {
            _surface = surface;
        }

        public MinimalPoint GetIntersectionMidpoint(IEdge edge) {
            var comp = new IntersectionMidpointComputer(_surface, edge);
            Point3d midpoint = comp.GetIntersectionsMidpoint();
            return midpoint.ToMinPoint();
        }

        public double FindElevationAtXY(double x, double y) {
            try {
                double elevation = _surface.FindElevationAtXY(x, y);
                return elevation;
            } catch (Autodesk.Civil.PointNotOnEntityException ex) {
                throw (new PointNotOnSurfaceException(ex.Message));
            }
        }

        public void AddVertex(double x, double y, double z) {
            Point3d newVertex = new Point3d(x, y, z);
            _surface.AddVertex(newVertex);
        }

        public IEnumerable<IVertex> GetVertexCollection() {
            return _surface.Vertices.ToVertexWrapperList();
        }

        public IEnumerable<IEdge> GetEdges() {
            return _surface.GetEdges();
        }
    }
}
