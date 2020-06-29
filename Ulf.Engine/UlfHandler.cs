using System;
using System.Collections.Generic;
using System.Threading;
using Ulf.Engine.Interfaces;
using Ulf.Excel;
using Ulf.Util;

namespace Ulf.Engine
{
    class UlfHandler
    {
        private List<CaseAssignment> _assignments;
        private ISurface _surface;
        private ICaseAssignmentEntityHandler _caseEntHandler;
        private IUlfEntityHandler _ulfEntHandler;

        public UlfHandler(ISurface surface,
                          ICaseAssignmentEntityHandler caseEntHandler,
                          IUlfEntityHandler ulfEntHandler)
        {
            (_surface, _caseEntHandler, _ulfEntHandler) = 
                (surface, caseEntHandler, ulfEntHandler);
        }

        public void HandleTheUlf(string excelFilePath)
        {
            CaseReader reader = new CaseReader(excelFilePath);
            _assignments = reader.CaseAssignments;
            ResetEntities();
            IterateCaseAssignments();
        }

        private void ResetEntities()
        {
            _caseEntHandler.Reset();
            _ulfEntHandler.Reset();
        }

        private void IterateCaseAssignments()
        {
            StartCaseLine(0);
            EndLineOrAddTransitionAccordingToNextAssignment(0);
            for (int i = 1; i < _assignments.Count; i++) {
                // If current assignment has none case, skip current itteration.
                if(IsNoneCase(i)) {
                    continue;
                } else {
                    StartNewLineIfLastAssignmentWasNone(i);
                    EndLineOrAddTransitionAccordingToNextAssignment(i);
                    DrawUlfForAssignment(i);
                }
            }
        }

        private bool IsNoneCase(int assignmentIndex)
        {
            return _assignments[assignmentIndex].CalcCase == CalculationCase.CaseNone;
        }

        private void StartNewLineIfLastAssignmentWasNone(int assignmentIndex)
        {
            if (IsNoneCase(assignmentIndex - 1)) {
                StartCaseLine(assignmentIndex);
            }
        }

        private void EndLineOrAddTransitionAccordingToNextAssignment(int assignmentIndex)
        {
            if (IsNoneCase(assignmentIndex + 1)) {
                EndCaseLine(assignmentIndex);
            } else {
                AddTransition(assignmentIndex + 1);
            }
        }

        private void StartCaseLine(int assignmentIndex)
        {
            double station = _assignments[assignmentIndex].FromStation;
            CalculationCase calcCase = _assignments[assignmentIndex].CalcCase;
            CaseStation cs = new CaseStation(calcCase, station);
            _caseEntHandler.StartCaseLine(cs);
        }

        private void AddTransition(int assignmentIndex)
        {
            double station = _assignments[assignmentIndex].FromStation;
            CalculationCase toCase = _assignments[assignmentIndex ].CalcCase;
            CaseStation csTo = new CaseStation(toCase, station);
            _caseEntHandler.AddTransition(csTo);
        }

        private void EndCaseLine(int assignmentIndex)
        {
            double station = _assignments[assignmentIndex].ToStation;
            CalculationCase toCase = _assignments[assignmentIndex].CalcCase;
            CaseStation csTo = new CaseStation(toCase, station);
            _caseEntHandler.EndCaseLine(csTo);
        }

        private void DrawUlfForAssignment(int assignmentIndex)
        {
            double startStation = _assignments[assignmentIndex].FromStation + 1;
            double endStation = _assignments[assignmentIndex].FromStation;
            double deltaStation = endStation - startStation;
            double increaseFactor = deltaStation / Math.Ceiling(deltaStation);
            CalculationCase calcCase = _assignments[assignmentIndex].CalcCase;
            for (double station = startStation; 
                        station <= endStation; 
                        station += increaseFactor) {
                double maxVV = _surface.getMaxVVAtStation(station);
                double ulf = UlfCalculator.ToUlF(maxVV, calcCase);
                _ulfEntHandler.AddUlfEntity(station, ulf);
            } 
        }
    }
}
