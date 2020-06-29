namespace DeconstructSurfaceSampleView.Engine.HelperObjects
{
    public class ReferencePointData
    {
        private readonly SimplePoint2d Point2d;
        private readonly AlignmentPoint AlignmentPoint;

        public ReferencePointData(SimplePoint2d point2d, AlignmentPoint alignmentPoint) =>
            (this.Point2d, this.AlignmentPoint) = (point2d, alignmentPoint);

        public double X { get { return Point2d.X; } }
        public double Y { get { return Point2d.Y; } }
        public double Station { get { return AlignmentPoint.Station; } }
        public double Offset { get { return AlignmentPoint.Offset; } }
        public double Elevation { get { return AlignmentPoint.Elevation; } }
    }
}
