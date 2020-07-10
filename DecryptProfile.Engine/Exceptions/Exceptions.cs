using System;

namespace DeconstructSurfaceSampleView.Engine.Exceptions {
    public class PointNotOnAlignmentException : Exception {
        public PointNotOnAlignmentException(string message) : base(message) { }
    }
}
