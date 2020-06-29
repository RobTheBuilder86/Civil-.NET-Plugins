using Autodesk.Civil.DatabaseServices;
using Ulf.Engine.Interfaces;
using Ulf.Util;
using System;
using Autodesk.AutoCAD.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil;

namespace Ulf.C3D.Imp
{
    class SurfaceWrapper : ISurface
    {
        static readonly double yTopOfSurface = 5349736;
        static readonly double yBottomOfSurface = 5349616;

        private TinSurface _surface;

        public SurfaceWrapper(TinSurface surface) => _surface = surface;

        public double getMaxVVAtStation(double station)
        {
            double ModelX = StationToXConverter.ToX(station);
            Point3d p3dStart = new Point3d(ModelX, yTopOfSurface, 0.0);
            Point3d p3dEnd = new Point3d(ModelX, yBottomOfSurface, 0.0);
            Point3dCollection curSamples = _surface.SampleElevations(p3dStart, p3dEnd);

            if (curSamples.Count > 0) {
                double dblMinOlf = curSamples[0].Z;
                for (int nSample = 1; nSample < curSamples.Count; nSample++) {
                    double dblCurOlf = curSamples[nSample].Z;
                    if (dblCurOlf < dblMinOlf)
                        dblMinOlf = dblCurOlf;
                }
                return dblMinOlf;
            }
            throw new PointNotOnEntityException($"DGM ist bei Station {station} nicht definiert.");
        }
    }
}
