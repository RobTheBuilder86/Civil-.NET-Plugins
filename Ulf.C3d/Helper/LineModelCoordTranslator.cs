using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ulf.Util;

namespace Ulf.C3D.Helper
{
    public static class LineModelCoordTranslator
    {
        /// <summary>
        /// Takes 2 stations and cases and returns a list of appropraite endpoints for a
        /// line connection those stations, respecting jumps between databands. 
        /// </summary>
        /// <param name="csFrom">Station and case of starting point.</param>
        /// <param name="csTo">Station and case of ending point.</param>
        /// <returns></returns>
        public static List<(SimplePoint2d, SimplePoint2d)> TranslateLine(CaseStation csFrom, CaseStation csTo)
        {
            double stationFrom = csFrom.Station;
            double stationTo = csTo.Station;

            List<(double, double)> modelXJumps = 
                StationToXConverter.FindModelXsOfJumps(stationFrom, stationTo);

            double xFrom = csFrom.X;
            double xTo = csTo.X;
            List<(double, double)> modelXOfLineEndPoints = 
                GetModelXOfLineEndPoints(xFrom, modelXJumps, xTo);

            double yFrom = csFrom.Y;
            double yTo = csTo.Y;
            List<(double, double)> modelYOfLineEndPoints = GetModelYOfLineEndPoints(yFrom, modelXOfLineEndPoints, yTo);

            var linePoints = new List<(SimplePoint2d, SimplePoint2d)>();
            int linecount = modelXOfLineEndPoints.Count;
            for(int i = 0; i < linecount; i++) {
                double point1X = modelXOfLineEndPoints[i].Item1;
                double point1Y = modelYOfLineEndPoints[i].Item1;
                SimplePoint2d point1 = new SimplePoint2d(point1X, point1Y);

                double point2X = modelXOfLineEndPoints[i].Item2;
                double point2Y = modelYOfLineEndPoints[i].Item2;
                SimplePoint2d point2 = new SimplePoint2d(point2X, point2Y);

                linePoints.Add((point1, point2));
            }
            return linePoints;
        }

        private static List<(double, double)> GetModelXOfLineEndPoints(double xFrom, 
                                                                       List<(double, double)> modelXJumps, 
                                                                       double xTo)
        {
            var XEndPoints = new List<(double, double)>();
            int jumpCount = modelXJumps.Count;
            // If there are jumps
            if (jumpCount > 0) {
                // Add line from start to first jump.
                XEndPoints.Add((xFrom, modelXJumps[0].Item1));
                // Add lines from end of jump to start of next jump for all jumps.
                for (int i = 1; i < jumpCount; i++) {
                    XEndPoints.Add((modelXJumps[i - 1].Item2, modelXJumps[i].Item1));
                }
                // Add line from end of last jump to end.
                XEndPoints.Add((modelXJumps[jumpCount-1].Item2, xTo));
            } 
            // If there are no jumps return line from start to end
            else {
                XEndPoints.Add((xFrom, xTo));
            }
            return XEndPoints;
        }

        private static List<(double, double)> GetModelYOfLineEndPoints(double yFrom, 
                                                                       List<(double, double)>modelXOfLineEndpoints, 
                                                                       double yTo)
        {
            var YEndPoints = new List<(double, double)>();
            double DeltaXTotal = GetTotalDeltaX(modelXOfLineEndpoints);
            double DeltaYTotal = yTo - yFrom;

            int lineCount = modelXOfLineEndpoints.Count;
            List<double> DeltaYList = GetDeltaYList(DeltaYTotal, DeltaXTotal, modelXOfLineEndpoints);

            if(lineCount == 1) {
                YEndPoints.Add((yFrom, yTo));
            } else {
                double currentY = yFrom + DeltaYList[0];
                YEndPoints.Add((yFrom, currentY));
                double lastY = currentY;
                for (int i = 1; i < lineCount; i++) {
                    currentY = lastY + DeltaYList[i];
                    YEndPoints.Add((lastY, currentY));
                    lastY = currentY;
                }
            }
            return YEndPoints;
        }

        private static double GetTotalDeltaX(List<(double, double)> modelXOfLineEndpoints)
        {
            double totalDeltaX = 0;
            foreach ((double, double) doubleTuple in modelXOfLineEndpoints) {
                totalDeltaX += doubleTuple.Item2 - doubleTuple.Item1;
            }
            return totalDeltaX;
        }

        private static List<double> GetDeltaYList(double DeltaYTotal,
                                                  double DeltaXTotal,
                                                  List<(double, double)>modelXOfLineEndpoints)
        {
            var DeltaYList = new List<double>();
            foreach ((double, double) XPairN in modelXOfLineEndpoints) {
                double deltaXN = Math.Abs(XPairN.Item2 - XPairN.Item1);
                double deltaYN = deltaXN / DeltaXTotal * DeltaYTotal;
                DeltaYList.Add(deltaYN);
            }
            return DeltaYList;
        }
    }
}
