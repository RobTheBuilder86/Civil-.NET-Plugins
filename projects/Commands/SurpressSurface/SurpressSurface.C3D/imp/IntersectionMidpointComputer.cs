using Autodesk.Aec.Modeler;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using SurpressSurface.Engine;

namespace SurpressSurface.C3D {
    // Works under the assumption, that _point1 and _point2 both either lay below or above
    // the surface. So that there are two intersection points between the surfaec and the
    // connecting line from _point1 to _point2.
    internal class IntersectionMidpointComputer {
        private TinSurface _surface;
        private Point3d _point1;
        private Point3d _point2;
        public Point3d FirstIntersection { get; private set; }
        public Point3d SecondIntersection { get; private set; }

        public IntersectionMidpointComputer(TinSurface surface, IEdge edge) {
            _surface = surface;
            _point1 = edge.Vertex1MP().ToPoint3d();
            _point2 = edge.Vertex2MP().ToPoint3d();
        }

        public IntersectionMidpointComputer(TinSurface surface,
                                            Point3d point1,
                                            Point3d point2) 
            => (_surface, _point1, _point2) = (surface, point1, point2);
        
        public Point3d GetIntersectionsMidpoint() {
            GetFirstIntersectionPoint();
            GetSecondIntersectionPoint();

            Point3d Midpoint = CalculateMidpoint(FirstIntersection, SecondIntersection);
            return Midpoint;
        }

        private void GetFirstIntersectionPoint() {
            Vector3d vector = _point1.GetVectorTo(_point2);
            FirstIntersection = GetIntersectionPoint(_point1, vector);
        }

        private void GetSecondIntersectionPoint() {
            Vector3d vector = _point2.GetVectorTo(_point1);
            SecondIntersection = GetIntersectionPoint(_point2, vector);
        }

        private Point3d GetIntersectionPoint(Point3d point, Vector3d vector) {
            try {
                return _surface.GetIntersectionPoint(point, vector);
            } catch (System.ArgumentException ex){
                throw new NoIntersectionException(ex.Message);
            }
        }

        public Point3d CalculateMidpoint(Point3d a, Point3d b) {
            double midX = (a.X + b.X) / 2;
            double midY = (a.Y + b.Y) / 2;
            double midZ = (a.Z + b.Z) / 2;
            return new Point3d(midX, midY, midZ);
        }
    }
}
