using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DatabaseServices;
using SurpressSurface.Engine;
using System.Collections.Generic;
using System.Linq;

namespace SurpressSurface.C3D {
    public static class ExtensionMethods {

        public static List<VertexWrapper> ToVertexWrapperList
            (this TinSurfaceVertexCollection collection) {

            var vertexWrapperList = new List<VertexWrapper>();
            foreach (TinSurfaceVertex vertex in collection) {
                vertexWrapperList.Add(new VertexWrapper(vertex));
            }
            return vertexWrapperList;
        }

        public static IEnumerable<EdgeWrapper> GetEdges(this TinSurface surface) {
            List<EdgeWrapper> edges = new List<EdgeWrapper>();

            var validTriangles = surface.Triangles.Where(t => t.IsValid);
            foreach (TinSurfaceTriangle triangle in validTriangles) {
                edges.AddRange(GetTriangleEdges(triangle));
            }

            return edges.Distinct().Where(e => e.IsValid());
        }

        static private IEnumerable<EdgeWrapper> GetTriangleEdges(TinSurfaceTriangle triangle){
            var triangleEdges = new List<EdgeWrapper>();

            triangleEdges.Add(new EdgeWrapper(triangle.Edge1));
            triangleEdges.Add(new EdgeWrapper(triangle.Edge2));
            triangleEdges.Add(new EdgeWrapper(triangle.Edge3));

            return triangleEdges;
        }

        public static MinimalPoint ToMinPoint(this Point3d point) {
            return new MinimalPoint(point.X, point.Y, point.Z);
        }

        public static Point3d ToPoint3d(this MinimalPoint point) {
            return new Point3d(point.X, point.Y, point.Z);
        }
    }
}
