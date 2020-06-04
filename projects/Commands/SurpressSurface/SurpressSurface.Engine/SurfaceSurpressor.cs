using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpressSurface.Engine
{
    public class SurfaceSurpressor {
        private ISurface _top;
        private ISurface _bottom;

        public static double MinimumDistance = 0.01;

        public SurfaceSurpressor(ISurface top, ISurface bottom) =>
            (_top, _bottom) = (top, bottom);

        public void SurpressBottomSurface() {
            SetBottomVerticesBelowTop();
            AddBottomVertices();
            // CheckForIntersectionsAndAddVertices();
        }

        // Ensure all vertices of sub are below top.
        private void SetBottomVerticesBelowTop() {
            foreach (IVertex vertex 
                in _bottom.GetVertexCollection().Where(v => v.IsValid() == true)) {
                try {
                    double maxHeight =
                        _top.FindElevationAtXY(vertex.GetX(), vertex.GetY())
                        - MinimumDistance;
                    if (vertex.GetZ() > maxHeight) {
                        vertex.SetZ(maxHeight);
                    }
                } catch (PointNotOnSurfaceException) {

                }
            }
        }

        // Check if all vertices of top are above sub. If necessary add a new vertex to
        // bottom with minimum vertical distance to top.
        private void AddBottomVertices() {
            foreach (IVertex vertex in 
                _top.GetVertexCollection().Where(v => v.IsValid() == true)) {
                double x = vertex.GetX();
                double y = vertex.GetY();
                double maxHeight = vertex.GetZ() - MinimumDistance;
                try {
                    if (_bottom.FindElevationAtXY(x, y) > maxHeight) {
                        _bottom.AddVertex(x, y, maxHeight);
                    }
                } catch (PointNotOnSurfaceException) { }
            }
        }
    }
}
