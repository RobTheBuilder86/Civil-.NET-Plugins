using Autodesk.Civil.DatabaseServices;
using Ulf.Engine.Interfaces;
using Ulf.Util;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil;

namespace Ulf.C3D.Imp
{
    class SurfaceWrapper : ISurface
    {
        static readonly double yTopOfSurface = 5349939;
        static readonly double yBottomOfSurface = 5349819;

        private TinSurface _surface;

        public SurfaceWrapper(TinSurface surface) => _surface = surface;

        public double getMinVVAtStation(double station)
        {
            double ModelX = StationToXConverter.ToX(station);
            Point3d p3dStart = new Point3d(ModelX, yTopOfSurface, 0.0);
            Point3d p3dEnd = new Point3d(ModelX, yBottomOfSurface, 0.0);
            try {
                Point3dCollection vvSamples = _surface.SampleElevations(p3dStart, p3dEnd);
                if (vvSamples.Count > 0) {
                    double minVV = vvSamples[0].Z;
                    for (int i = 1; i < vvSamples.Count; i++) {
                        if (vvSamples[i].Z < minVV) {
                            minVV = vvSamples[i].Z;
                        }
                    }
                    return minVV;
                }
                throw new SurfaceNotDefinedException($"DGM ist bei Station {station} nicht definiert.");
            } catch (Autodesk.Civil.SurfaceException) {
                throw new SurfaceNotDefinedException($"DGM ist bei Station {station} nicht definiert.");
            }
        }
    }
}
