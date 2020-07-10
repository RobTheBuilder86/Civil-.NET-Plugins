using DeconstructSurfaceSampleView.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeconstructSurfaceSampleView.Engine.HelperObjects
{
    public abstract class SurfaceSampleView
    {
        public IPolyline SurfaceSampleLine;
        public ReferencePointData ReferencePoint;

        public readonly double VerticalExageration;
        public readonly double HorizontalExageration;

        public SurfaceSampleView(IPolyline surfaceSampleLine,
                                 ReferencePointData referencePoint, 
                                 double verticalExageration, 
                                 double horizontalExageration)
        {
            if (surfaceSampleLine.GetPoints().Count() == 0) {
                throw new ArgumentException("Surface sample line must have at least on vertex.");
            }
            this.SurfaceSampleLine = surfaceSampleLine;
            this.ReferencePoint = referencePoint;
            this.VerticalExageration = verticalExageration;
            this.HorizontalExageration = horizontalExageration;
        }

        public SurfaceSampleView(IPolyline surfaceSampleLine, ReferencePointData referencePoint)
            : this(surfaceSampleLine, referencePoint, 1, 1) { }

        public List<AlignmentPoint> GetAlignmentPoints()
        {
            var AlignmentPoints = new List<AlignmentPoint>();
            foreach(SimplePoint2d point in SurfaceSampleLine.GetPoints()) {
                AlignmentPoints.Add(ToStationOffset(point));
            }
            return AlignmentPoints;
        }

        public abstract AlignmentPoint ToStationOffset(SimplePoint2d point);
    }

    public class ViewAlong : SurfaceSampleView
    {
        public ViewAlong(IPolyline surfaceSampleLine,
                         ReferencePointData referencePoint) 
            : base(surfaceSampleLine, referencePoint) { }

        public ViewAlong(IPolyline surfaceSampleLine,
                         ReferencePointData referencePoint,
                         double verticalExageration,
                         double horizontalExageration) 
            : base(surfaceSampleLine, 
                   referencePoint, 
                   verticalExageration, 
                   horizontalExageration) { }

        public override AlignmentPoint ToStationOffset(SimplePoint2d point)
        {
            double station = ReferencePoint.Station + (point.X - ReferencePoint.X) / HorizontalExageration;
            double offset = ReferencePoint.Offset;
            double elevation = ReferencePoint.Elevation + (point.Y - ReferencePoint.Y) / VerticalExageration;
            return new AlignmentPoint(station, offset, elevation);
        }
    }

    public class ViewAcross : SurfaceSampleView
    {
        public ViewAcross(IPolyline surfaceSampleLine,
                         ReferencePointData referencePoint)
            : base(surfaceSampleLine, referencePoint) { }

        public ViewAcross(IPolyline surfaceSampleLine,
                         ReferencePointData referencePoint,
                         double verticalExageration,
                         double horizontalExageration)
            : base(surfaceSampleLine,
                   referencePoint,
                   verticalExageration,
                   horizontalExageration) { }

        public override AlignmentPoint ToStationOffset(SimplePoint2d point)
        {
            double station = ReferencePoint.Station;
            double offset = ReferencePoint.Offset + (point.X - ReferencePoint.X) / HorizontalExageration;
            double elevation = ReferencePoint.Elevation + (point.Y - ReferencePoint.Y) / VerticalExageration;
            return new AlignmentPoint(station, offset, elevation);
        }
    }
}
