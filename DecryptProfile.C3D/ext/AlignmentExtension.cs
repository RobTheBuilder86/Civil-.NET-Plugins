using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.Settings;

namespace DeconstructSurfaceSampleView.C3D.ext
{
    public static class AlignmentExtension
    {
        public static double ToRawStation(this Alignment alignment, double officialStation)
        {
            double rawStation = officialStation;
            foreach(StationEquation steq in alignment.StationEquations) {
                if (steq.StationBack > officialStation)
                    break;
                rawStation += steq.StationBack - steq.StationAhead;
            }
            return rawStation;
        }

        public static double ToOfficialStation(this Alignment alignment, double rawStation)
        {
            // Does not work as intended if station definition point is not equal to 
            // starting point.
            double officialStation = rawStation;
            foreach (StationEquation steq in alignment.StationEquations) {
                if (steq.StationBack > rawStation)
                    break;
                officialStation += steq.StationAhead - steq.StationBack;
            }
            return officialStation;
        }
    }
}
