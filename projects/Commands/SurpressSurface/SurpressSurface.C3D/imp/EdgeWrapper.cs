using SurpressSurface.Engine;
using Autodesk.Civil.DatabaseServices;
using System.Collections.Generic;
using System;
using Autodesk.AutoCAD.Geometry;

namespace SurpressSurface.C3D {
    public class EdgeWrapper : IEdge, IEquatable<EdgeWrapper> {
        internal TinSurfaceEdge Edge { get; }
        private Point3d vertex1 { get { return Edge.Vertex1.Location; } }
        private Point3d vertex2 { get { return Edge.Vertex2.Location; } }

        public EdgeWrapper(TinSurfaceEdge edge) => this.Edge = edge;

        public MinimalPoint Vertex1MP() {
            return vertex1.ToMinPoint();
        }

        public MinimalPoint Vertex2MP() {
            return vertex2.ToMinPoint();
        }

        public bool IsValid() => Edge.IsValid;

        public bool Equals(EdgeWrapper other) {
            return Edge.Equals(other.Edge);
        }

        public override int GetHashCode() {
            return -539139652 + EqualityComparer<TinSurfaceEdge>.Default.GetHashCode(Edge);
        }
    }
}
