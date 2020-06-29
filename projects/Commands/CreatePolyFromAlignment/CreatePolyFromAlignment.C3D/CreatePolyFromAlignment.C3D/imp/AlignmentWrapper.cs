using Autodesk.Civil.DatabaseServices;
using CreatePolyFromAlignment.Engine.Interfaces;
using CreatePolyFromAlignment.Engine.HelperObjects;
using System.Collections.Generic;
using System.Linq;

namespace CreatePolyFromAlignment.C3D.imp
{
    internal class AlignmentWrapper : IAlignment
    {
        private Alignment _alignment;
        public AlignmentWrapper(Alignment alignment) => _alignment = alignment;

        public ProfileWrapper GetWrappedProfile()
        {
            return new ProfileWrapper(_alignment.GetProfileFg());
        }

        public (double x, double y) GetCoordsAt(double Station)
        {
            double x = 0;
            double y = 0;
            _alignment.PointLocation(Station, 0, ref x, ref y);
            return (x, y);
        }

        public List<StationSegment> GetSegments()
        {
            var segments = new List<StationSegment>();
            foreach(AlignmentEntity entity in _alignment.Entities) {
                foreach(AlignmentSubEntity subEntity in entity.GetSubEntities()) {
                    segments.Add(subEntity.ToStationSegment());
                }
            }
            segments = segments.OrderBy(s => s.StartStation).ToList();
            return segments;
        }
    }
}