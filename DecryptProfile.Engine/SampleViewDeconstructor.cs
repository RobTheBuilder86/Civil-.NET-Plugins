using DeconstructSurfaceSampleView.Engine.HelperObjects;
using DeconstructSurfaceSampleView.Engine.Interfaces;
using System.Collections.Generic;

namespace DeconstructSurfaceSampleView.Engine
{
    public class SampleViewDeconstructor
    {
        private readonly IAlignment Alignment;

        public SampleViewDeconstructor(IAlignment alignment) =>
            (this.Alignment) = (alignment);

        public List<SimplePoint3d> DeconstructSampleView(SurfaceSampleView sampleView)
        {
            var points = new List<SimplePoint3d>();
            foreach (AlignmentPoint alignmentPoint in sampleView.GetAlignmentPoints()) {
                points.Add(DeconstructAlignmentPoint(alignmentPoint));
            }
            return points;
        }

        private SimplePoint3d DeconstructAlignmentPoint(AlignmentPoint alignmentPoint)
        {
            SimplePoint2d point2d = Alignment.PointLocation(alignmentPoint.Station, alignmentPoint.Offset);
            return new SimplePoint3d(point2d, alignmentPoint.Elevation);
        }
    }
}
