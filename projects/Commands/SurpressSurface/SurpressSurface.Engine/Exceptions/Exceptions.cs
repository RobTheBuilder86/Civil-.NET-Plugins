using System;

namespace SurpressSurface.Engine {
    public class PointNotOnSurfaceException : Exception {
        public PointNotOnSurfaceException(string message) : base(message) { }
    }

    public class NoIntersectionException : Exception {
        public NoIntersectionException(string message) : base(message) { }
    }
}
