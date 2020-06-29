using System;
using Ulf.Engine;
using Ulf.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Headers;
using System.Collections;

namespace Ulf.Tests
{
    public class UlfCalculatorTests
    {
        private tolerantDoubleEqualityComparer tolerantDoubleComparer;

        public UlfCalculatorTests()
        {
            tolerantDoubleComparer = new tolerantDoubleEqualityComparer(0.006);
        }

        [Fact]
        public void ToULF_NegativeMaxVVValue_ThrowsException()
        {
            CalculationCase calcCase = CalculationCase.Case1;
            Assert.Throws<ArgumentException>(() => { UlfCalculator.ToUlF(-1, calcCase); });
        }

        [Theory]
        [InlineData(CalculationCase.Case1, 400, 34.98)]
        [InlineData(CalculationCase.Case1, 450, 39.36)]
        [InlineData(CalculationCase.Case1, 500, 43.73)]
        [InlineData(CalculationCase.Case2, 400, 34.98)]
        [InlineData(CalculationCase.Case2, 450, 39.36)]
        [InlineData(CalculationCase.Case2, 500, 43.73)]
        [InlineData(CalculationCase.Case3a, 400, 55.52)]
        [InlineData(CalculationCase.Case3a, 450, 62.46)]
        [InlineData(CalculationCase.Case3a, 500, 69.40)]
        [InlineData(CalculationCase.Case3b, 400, 72.90)]
        [InlineData(CalculationCase.Case3b, 450, 82.01)]
        [InlineData(CalculationCase.Case3b, 500, 91.12)]
        [InlineData(CalculationCase.Case4a, 400, 55.52)]
        [InlineData(CalculationCase.Case4a, 450, 62.46)]
        [InlineData(CalculationCase.Case4a, 500, 69.40)]
        [InlineData(CalculationCase.Case4b, 400, 24.22)]
        [InlineData(CalculationCase.Case4b, 450, 27.25)]
        [InlineData(CalculationCase.Case4b, 500, 30.28)]
        public void ToUlf_BasePoints_CalculatesBasePointULF(CalculationCase calcCase, 
                                                                     double VV, 
                                                                     double expectedUlf)
        {
            double actualUlf = UlfCalculator.ToUlF(VV, calcCase);

            Assert.Equal(expectedUlf, actualUlf, tolerantDoubleComparer);
        }


        private class tolerantDoubleEqualityComparer : IEqualityComparer<double>
        {
            private double _tolerance;

            public tolerantDoubleEqualityComparer(double tolerance)
            {
                _tolerance = Math.Abs(tolerance);
            }

            public bool Equals(double value1, double value2)
            {
                if (Math.Abs(value1-value2) < _tolerance) {
                    return true;
                } else {
                    return false;
                }
            }

            public int GetHashCode(double obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
