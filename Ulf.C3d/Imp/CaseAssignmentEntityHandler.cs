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
using System.Collections.Generic;

namespace Ulf.C3D.Imp
{
    internal class CaseAssignmentEntityHandler : ICaseAssignmentEntityHandler
    {
        private CaseStation csLast;
        private EntityCreator _creator;

        public CaseAssignmentEntityHandler(EntityCreator creator)
        {
            _creator = creator;
        }

        public void Reset()
        {
            _creator.Reset();
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
            DrawLine(csLast, TransitionStartPoint);

            CalculationCase endPointCase = csTo.CalcCase;
            double endPointStation = csTo.Station + deltaStation / 2;
            CaseStation TransitionEndPoint = new CaseStation(endPointCase, endPointStation);
            AddBlockInsert(TransitionEndPoint);
            DrawLine(TransitionStartPoint, TransitionEndPoint);

            csLast = TransitionEndPoint;
        }

        public void EndCaseLine(CaseStation csTo)
        {
            DrawLine(csLast, csTo);
            AddBlockInsert(csTo);
            csLast = null;
        }

        private void AddBlockInsert(CaseStation cs)
        {
            _creator.CreateBlockInsert(cs);
        }

        private void DrawLine(CaseStation csFrom, CaseStation csTo)
        {
            List<(SimplePoint2d, SimplePoint2d)> endPoints = LineModelCoordTranslator.TranslateLine(csFrom, csTo);
            _creator.CreateLines(endPoints);
        }
    }
}
