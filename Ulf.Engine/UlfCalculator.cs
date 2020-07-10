using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Ulf.Util;

namespace Ulf.Engine
{
    public static class UlfCalculator
    {

        private static BasePointPair Case1and2Pair()
        {
            return InitializeBasePointPair(400, 34.98, 500, 43.73);
        }

        private static BasePointPair Case3aand4aPair()
        {
            return InitializeBasePointPair(400, 55.52, 500, 69.40);
        }

        private static BasePointPair Case3bPair()
        {
            return InitializeBasePointPair(400, 72.90, 500, 91.12);
        }

        private static BasePointPair Case4bPair()
        {
            return InitializeBasePointPair(400, 24.22, 500, 30.28);
        }

        private static BasePointPair InitializeBasePointPair
            (double vv1, double ulf1, double vv2, double ulf2)
        {
            return new BasePointPair(new BasePoint(vv1, ulf1), new BasePoint(vv2, ulf2));
        }

        public static double ToUlF(double VV, CalculationCase calcCase)
        {
            switch (calcCase) {
                case CalculationCase.Case1:
                case CalculationCase.Case2:
                    return Case1and2Pair().ToUlf(VV);
                case CalculationCase.Case3a:
                case CalculationCase.Case4a:
                    return Case3aand4aPair().ToUlf(VV);
                case CalculationCase.Case3b:
                    return Case3bPair().ToUlf(VV);
                case CalculationCase.Case4b:
                    return Case4bPair().ToUlf(VV);
                default:
                    return 0;
            }
        }

        private class BasePoint
        {
            public double VV;
            public double ULF;
            public BasePoint(double vv, double ulf) => (VV, ULF) = (vv, ulf);
        }

        private class BasePointPair
        {
            BasePoint Point1;
            BasePoint Point2;
            double deltaVV;
            double deltaULF;
            double UnitUlfPerUnitVV;
            public BasePointPair(BasePoint point1, BasePoint point2)
            {
                (Point1, Point2) = (point1, point2);
                deltaVV = Point2.VV - Point1.VV;
                deltaULF = Point2.ULF - Point1.ULF;
                UnitUlfPerUnitVV = deltaULF / deltaVV;
            }
                
            public double ToUlf(double vv)
            {
                double deltaVV = vv - Point1.VV;
                double deltaUlf = deltaVV * UnitUlfPerUnitVV;
                double ulf = Point1.ULF + deltaUlf;
                if(ulf < 0) {
                    return 0;
                }
                return ulf;
            }
        }
    }
}
