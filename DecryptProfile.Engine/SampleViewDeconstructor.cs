using DeconstructSurfaceSampleView.Engine.Exceptions;
using DeconstructSurfaceSampleView.Engine.HelperObjects;
using DeconstructSurfaceSampleView.Engine.Interfaces;
using System.Collections.Generic;

namespace DeconstructSurfaceSampleView.Engine
{
    public class SampleViewDeconstructor
    {
        private readonly IAlignment _alignment;
        private List<SimplePoint3d> _deconstrucedPoints;

        public SampleViewDeconstructor(IAlignment alignment) =>
            (_alignment) = (alignment);

        public List<SimplePoint3d> DeconstructSampleView(SurfaceSampleView sampleView)
        {
            _deconstrucedPoints = new List<SimplePoint3d>();
            foreach (AlignmentPoint alignmentPoint in sampleView.GetAlignmentPoints()) {
                TryAddPoint(alignmentPoint);
            }
            return _deconstrucedPoints;
        }

        private void TryAddPoint(AlignmentPoint alignmentPoint)
        {
            try {
                SimplePoint3d deconstructedPoint = 
                    DeconstructAlignmentPoint(alignmentPoint);
                _deconstrucedPoints.Add(deconstructedPoint);
            } catch (PointNotOnAlignmentException) {}
        }

        private SimplePoint3d DeconstructAlignmentPoint(AlignmentPoint alignmentPoint)
        {
            SimplePoint2d point2d = _alignment.PointLocation(alignmentPoint.Station, alignmentPoint.Offset);
            return new SimplePoint3d(point2d, alignmentPoint.Elevation);
        }
    }
}
