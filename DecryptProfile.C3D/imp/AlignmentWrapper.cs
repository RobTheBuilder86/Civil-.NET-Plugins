using Autodesk.Civil.DatabaseServices;
using DeconstructSurfaceSampleView.Engine.HelperObjects;
using DeconstructSurfaceSampleView.Engine.Interfaces;
using DeconstructSurfaceSampleView.C3D.ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeconstructSurfaceSampleView.C3D.imp
{
    internal class AlignmentWrapper : IAlignment
    {
        public readonly Alignment _alignment;

        public AlignmentWrapper(Alignment alignment) =>
            _alignment = alignment;

        public SimplePoint2d PointLocation(double officialStation, double offset)
        {
            double x = 0;
            double y = 0;
            double rawStation = _alignment.ToRawStation(officialStation);
            _alignment.PointLocation(rawStation, offset, ref x, ref y);
            return new SimplePoint2d(x, y);
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
