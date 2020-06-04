using SurpressSurface.Engine;
using Autodesk.Civil.DatabaseServices;

namespace SurpressSurface.C3D
{
    public class VertexWrapper : IVertex {
        private readonly TinSurfaceVertex _vertex;
        private TinSurface ParentSurface { 
            get { return _vertex.Surface; } 
        }

        public VertexWrapper(TinSurfaceVertex vertex) => 
            (_vertex) = (vertex);

        public double GetX() => _vertex.Location.X;
        public double GetY() => _vertex.Location.Y;
        public double GetZ() => _vertex.Location.Z;

        public void SetZ(double newZ) {
            ParentSurface.SetVertexElevation(_vertex, newZ);
        }

        public bool IsValid() {
            return _vertex.IsValid;
        }
    }

    
}
