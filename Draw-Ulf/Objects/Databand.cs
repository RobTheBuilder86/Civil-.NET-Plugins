using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SurfaceCommands
{
    internal class DataBandInfo
    {
        public Point3d Insert { get; }
        public double Length { get; }
        public double StartStation { get; }
        public double EndStation { get; }

        public DataBandInfo( Point2d insert, double startStation, double length)
        {
            if (length <= 0) {
                throw new ArgumentException(
                    "DataBand cannot have negative or zero length value.",
                    nameof(length));
            }
            if (startStation < 0) {
                throw new ArgumentException(
                    "DataBand cannot have negative starting station.",
                    nameof(startStation));
            }
            this.Insert = new Point3d(insert.X, insert.Y, 0);
            this.Length = length;
            this.StartStation = startStation;
            this.EndStation = StartStation + Length;
        }

        public static List<DataBandInfo> GetNbsDatabands()
        {
            double databandX = 3550000;
            double startStation = 53530;
            const double databandsY = 5350000;
            const double databandsDeltaX = 2000;
            List<double> databandLengths = new List<double>
                {
                    1023.875, 1120,  910, 1110, 1070, 1080,  910,  890, 1050,  880,
                    1034.382,  990, 1030,  860, 1080, 1120, 1110, 1120, 1120, 1100,
                    1120,      605.2665
                };

            List<DataBand> Databands = new List<DataBand>();
            foreach (double length in databandLengths) {
                Point2d currentInsert =
                    new Point2d(databandX, databandsY);

                Databands.Add(
                    new DataBand(currentInsert,
                                 startStation,
                                 length));
                startStation += length;
                databandX += databandsDeltaX;
            }

            return Databands;
        }
    }
}