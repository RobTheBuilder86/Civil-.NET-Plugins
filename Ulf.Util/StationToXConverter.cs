using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;

namespace Ulf.Util
{
    public static class StationToXConverter
    {
        static double StartStation = 53530;
        static double StartModelX = 3550000;

        static List<double> DatabandLengths = new List<double> {
            1023.874, 1120,  910, 1110, 1070, 1080,  910,  890, 1050,  880,
            1034.382, 990,  1030,  860, 1080, 1120, 1110, 1120, 1120, 1100,
            1120,     605.2665
        };

        static List<StationEquation> StationEquations = new List<StationEquation>{
            new StationEquation(53834.377, 53810.503),
            new StationEquation(64340.460, 64326.078)
        };

        static double LastStationStanding = StartStation + 
                                            DatabandLengths.Sum() -
                                            StationEquations.Sum(s => s.DeltaStation);

        public static double ToX(double station)
        {
            CheckForValidInputStation(station);
            double xOffset = CalculateXOffsetDueToDatabandJumps(station);
            double steqCorrection = CalculateStationEquationCorrection(station);
            return StartModelX + station - StartStation + xOffset + steqCorrection;
        }

        private static void CheckForValidInputStation(double station)
        {
            if (station < StartStation) {
                throw new ArgumentException("Station must be above 53530!");
            } else if (station > LastStationStanding) {
                throw new ArgumentException("Station must be below 75825.27 (end of VE240)!");
            }
        }

        private static double CalculateXOffsetDueToDatabandJumps(double station)
        {
            int i = CountDatabandJumps(station);
            return i * 2000 - DatabandLengths.Take(i).Sum();
        }

        private static int CountDatabandJumps(double station)
        {
            int jumps = 0;
            double iterationStation = StartStation;
            while (iterationStation < station) {
                iterationStation += DatabandLengths[jumps];
                jumps++;
            }
            return --jumps;
        }

        private static double CalculateStationEquationCorrection(double station)
        {
            double steqCorrection = 0;
            foreach(StationEquation steq in StationEquations) {
                if (steq.StationAhead < station) {
                    steqCorrection += steq.DeltaStation;
                }
            }
            return steqCorrection;
        }

        public static List<double> FindStationsOfJumps(double fromStation, double toStation)
        {
            CheckForValidInputStation(fromStation);
            CheckForValidInputStation(toStation);
            var jumps = new List<double>();
            int index = 0;
            double station = StartStation;
            while (station + DatabandLengths[index] < fromStation) {
                station += DatabandLengths[index];
                index++;
            }
            while (station + DatabandLengths[index] < toStation) {
                station += DatabandLengths[index];
                jumps.Add(station - CalculateStationEquationCorrection(station));
                index++;
            }
            return jumps;
        }

        public static List<(double, double)> FindModelXsOfJumps(double fromStation, double toStation)
        {
            List<(double, double)> ModelXsOfJumps = new List<(double, double)>();
            List<double> jumpStations = FindStationsOfJumps(fromStation, toStation);
            foreach (double jumpStation in jumpStations) {
                double ModelXBeforeJump = ToX(jumpStation);
                double ModelXAfterJump = RoundUpToNearest2000(ModelXBeforeJump);
                ModelXsOfJumps.Add((ModelXBeforeJump, ModelXAfterJump));
            }
            return ModelXsOfJumps;
        }

        private static double RoundUpToNearest2000(double x)
        {
            x /= 2000;
            x = Math.Ceiling(x);
            x *= 2000;
            return x;
        }
    }

    class StationEquation
    {
        public readonly double StationBack;
        public readonly double StationAhead;
        public double DeltaStation {
            get { return StationBack - StationAhead; }
        }
        public StationEquation(double stationBack, double stationAhead) =>
            (StationBack, StationAhead) = (stationBack, stationAhead);
        
    }
}
