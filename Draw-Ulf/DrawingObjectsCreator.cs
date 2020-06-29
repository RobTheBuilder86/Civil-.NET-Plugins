using Autodesk.AutoCAD.Geometry;
using Draw_Ulf.Objects;
using Excel_Ulf;
using System.Collections.Generic;

namespace Draw_Ulf
{
    public class CaseAssignmentPointCreator
    {
        private readonly CaseReader _reader;
        public double TransitionSteepness = 2;

        public CaseAssignmentPointCreator(CaseReader reader) =>
            _reader = reader;

        public List<StationCaseObject> CaseAssignmentPoints()
        {
            var points = new List<StationCaseObject>();

            List<CaseAssignment> caseAssignments = _reader.CaseAssignments;
            CaseAssignmentPoints.Add()
            for (int i = 0; i < caseAssignments.Count; i++) {

            }
        }
        
    }
}
