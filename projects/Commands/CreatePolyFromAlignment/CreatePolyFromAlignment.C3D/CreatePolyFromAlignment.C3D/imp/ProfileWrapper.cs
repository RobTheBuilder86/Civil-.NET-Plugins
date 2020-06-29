using Autodesk.Civil.DatabaseServices;
using CreatePolyFromAlignment.Engine.HelperObjects;
using CreatePolyFromAlignment.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePolyFromAlignment.C3D.imp
{
    class ProfileWrapper : IProfile
    {
        private Profile _profile;
        public ProfileWrapper(Profile profile) => _profile = profile;

        public double GetElevationAt(double station)
        {
            return _profile.ElevationAt(station);
        }

        public List<StationSegment> GetSegments()
        {
            var segments = new List<StationSegment>();
            foreach (ProfileEntity entity in _profile.Entities) {
                segments.Add(entity.ToStationSegment());
            }
            segments = segments.OrderBy(s => s.StartStation).ToList();
            return segments;
        }
    }
}
