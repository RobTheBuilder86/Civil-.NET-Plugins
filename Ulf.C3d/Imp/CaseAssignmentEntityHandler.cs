using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Ulf.Engine.Interfaces;
using Ulf.Util;
using Common;
using Autodesk.AutoCAD.GraphicsSystem;
using Autodesk.Civil.DatabaseServices;
using System;
using Ulf.C3D.Helper;

namespace Ulf.C3D.Imp
{
    internal class CaseAssignmentEntityHandler : ICaseAssignmentEntityHandler
    {

        private double steepness = 2;
        private CaseStation csLast;

        public void Reset()
        {
            Active.DeleteAllEntitiesOnLayer("IAG_BWB_Fallzuordnung");
        }

        public void StartCaseLine(CaseStation cs)
        {
            AddBlockInsert(cs);
            csLast = cs;
        }

        public void AddTransition(CaseStation csTo)
        {
            double deltaStation = DeltaStationCalculator.StationDelta(csLast, csTo);

            CalculationCase startPointCase = csLast.CalcCase;
            double startPointStation = csTo.Station - deltaStation / 2;
            CaseStation TransitionStartPoint = new CaseStation(startPointCase, startPointStation);
            AddBlockInsert(TransitionStartPoint);

            CalculationCase endPointCase = csTo.CalcCase;
            double endPointStation = csTo.Station + deltaStation / 2;
            CaseStation TransitionEndPoint = new CaseStation(endPointCase, endPointStation);
            AddBlockInsert(TransitionEndPoint);
            DrawLine(TransitionStartPoint, TransitionEndPoint);

            csLast = csTo;
        }

        public void EndCaseLine(CaseStation csTo)
        {
            DrawLine(csLast, csTo);
            AddBlockInsert(csTo);
            csLast = null;
        }

        private void AddBlockInsert(CaseStation cs)
        {

        }

        private void DrawLine(CaseStation csFrom, CaseStation csTo)
        {

        }
    }
}
