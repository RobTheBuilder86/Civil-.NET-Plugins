using Autodesk.Civil.DatabaseServices;
using DeconstructSurfaceSampleView.Engine.HelperObjects;
using DeconstructSurfaceSampleView.Engine.Interfaces;
using DeconstructSurfaceSampleView.Engine.Exceptions;
using DeconstructSurfaceSampleView.C3D.ext;
using Autodesk.Civil;
using Common;

namespace DeconstructSurfaceSampleView.C3D.imp
{
    internal class AlignmentWrapper : IAlignment
    {
        public readonly Alignment _alignment;

        public AlignmentWrapper(Alignment alignment) =>
            _alignment = alignment;

        public SimplePoint2d PointLocation(double officialStation, double offset)
        {
            try {
                double x = 0;
                double y = 0;
                double rawStation = _alignment.ToRawStation(officialStation);
                _alignment.PointLocation(rawStation, offset, ref x, ref y);
                return new SimplePoint2d(x, y);
            } catch (PointNotOnEntityException) {
                string msg = $"\nStation {officialStation:0.000} and offset {offset:0.000} " +
                             "not defined on alignment.";
                Active.WriteMessage(msg);
                throw new PointNotOnAlignmentException(msg);
            }
        }

        public double StartStation {
            get { return _alignment.StartingStation; }
        }

        public double OfficialEndStation
        {
            get { return _alignment.EndingStationWithEquations; }
        }
    }
}
