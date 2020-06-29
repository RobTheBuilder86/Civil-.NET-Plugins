using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.DatabaseServices;
using CreatePolyFromAlignment.Engine.HelperObjects;
using CreatePolyFromAlignment.C3D.exc;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SegmentType = CreatePolyFromAlignment.Engine.HelperObjects.SegmentType;

namespace CreatePolyFromAlignment.C3D.imp
{
    public static class ExtensionMethods
    {
        public static Profile GetProfileFg(this Alignment alignment)
        {
            var ProfilesFG = new List<Profile>();

            foreach (ObjectId profileId in alignment.GetProfileIds()) {
                Profile profile = profileId.GetObject(OpenMode.ForRead) as Profile;
                if (profile.ProfileType == ProfileType.FG) {
                    ProfilesFG.Add(profile);
                }
            }

            switch (ProfilesFG.Count) {
                case 0:
                    throw new AlignmentAttachedProfileException("Alignment has no attached FG Profiles");
                case 1:
                    return ProfilesFG[0];
                default:
                    throw new AlignmentAttachedProfileException("Alignment has multiple attached FG Profiles");
            }
        }

        public static List<AlignmentSubEntity> GetSubEntities(this AlignmentEntity entity)
        {
            var subEntities = new List<AlignmentSubEntity>();
            for (int i = 0; i < entity.SubEntityCount; i++) {
                subEntities.Add(entity[i]);
            }
            return subEntities;
        }

        public static StationSegment ToStationSegment(this AlignmentSubEntity subEntity)
        {
            double start = subEntity.StartStation;
            double end = subEntity.EndStation;
            SegmentType type = (subEntity.SubEntityType == AlignmentSubEntityType.Line) ?
                               SegmentType.straight :
                               SegmentType.nonstraight;
            return new StationSegment(start, end, type);
        }

        public static StationSegment ToStationSegment(this ProfileEntity entity)
        {
            double start = entity.StartStation;
            double end = entity.EndStation;
            SegmentType type = (entity.EntityType == ProfileEntityType.Tangent) ? 
                                SegmentType.straight : 
                                SegmentType.nonstraight;
            return new StationSegment(start, end, type);
        }
    }
}
