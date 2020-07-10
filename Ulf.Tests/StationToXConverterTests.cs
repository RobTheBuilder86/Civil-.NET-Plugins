using Ulf.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ulf.Tests
{
    public class StationToXConverterTests
    {
        private readonly ITestOutputHelper output;

        public StationToXConverterTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ToX_Station70790_Returns3584030()
        {
            double expected = 3584030;
            double station = 70790;

            double actual = StationToXConverter.ToX(station);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToX_Station58760_Returns3560020()
        {
            double expected = 3560020;
            double station = 58760;

            double actual = StationToXConverter.ToX(station);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(53529, "Station before 1st databand.")]
        [InlineData(75826, "Station after last databand.")]
        public void ToX_ValueBeyondScope_ThrowsArgumentException(double station, string message)
        {
            output.WriteLine(message);

            Assert.Throws<ArgumentException>(() => StationToXConverter.ToX(station));
        }

        [Theory]
        [InlineData(54000, 3550493.874, "Station within 1st Databand.")]
        [InlineData(54900, 3552370.000, "Station within 2nd Databand.")]
        [InlineData(55900, 3554250.000, "Station within 3rd Databand.")]
        [InlineData(59400, 3560660.000, "Station within 6th Databand.")]
        [InlineData(64500, 3570964.382, "Station within 11th Databand. After 2nd StationEquation.")]
        [InlineData(70200, 3582550, "Station within 17th Databand.")]
        [InlineData(75825.266, 3592605.266, "Last valid station.")]
        public void ToX_GivenRandomValidStations_CalculatesCorrectXValue(double station, 
                                                                         double expectedX,
                                                                         string message)
        {
            output.WriteLine(message);
            double actualX = StationToXConverter.ToX(station);

            Assert.Equal(expectedX, actualX);
        }

        [Theory]
        [MemberData(nameof(FindStationsOfJumps_From1stToLastDB_Data))]
        public void FindStationsOfJumps_From1stToLastDB_FindsAllJumps(double fromStation,
                                                                    double toStation, 
                                                                    List<double>expectedJumps)
        {
            List<double> actualJumps = StationToXConverter.FindStationsOfJumps(fromStation, toStation);

            Assert.Equal(expectedJumps, actualJumps);
        }
        public static IEnumerable<Object[]> FindStationsOfJumps_From1stToLastDB_Data =>
        new List<object[]>{
            new object[] {
                54000, 75500, 
                new List<double>{
                    54530, 55650, 56560, 57670, 58740, 59820, 60730, 61620, 62670, 63550,
                    64570, 65560, 66590, 67450, 68530, 69650, 70760, 71880, 73000, 74100,
                    75220
                }
            }
        };

        [Theory]
        [MemberData(nameof(FindModelXsOfJumps_From1stToLastDB_Data))]
        public void FindModelXsOfJumps_From1stToLastDB_FindsAllJumps(double fromStation,
                                                                     double toStation,
                                                                     List<(double, double)> expectedJumps)
        {
            List<(double, double)> actualJumps = StationToXConverter.FindModelXsOfJumps(fromStation, toStation);
            for (int i = 0; i < actualJumps.Count; i++) {
                output.WriteLine($"{i + 1} -> Expected: {expectedJumps[i].Item1}, {expectedJumps[i].Item2} - Actual: {actualJumps[i].Item1}, {actualJumps[i].Item2}");
            }

            Assert.Equal(expectedJumps, actualJumps);
        }
        public static IEnumerable<Object[]> FindModelXsOfJumps_From1stToLastDB_Data =>
        new List<object[]>{
            new object[] {
                54000, 75500,
                new List<(double,double)>{
                    (3551023.874, 3552000),
                    (3553120.000, 3554000),
                    (3554910.000, 3556000),
                    (3557110.000, 3558000),
                    (3559070.000, 3560000),
                    (3561080.000, 3562000),
                    (3562910.000, 3564000),
                    (3564890.000, 3566000),
                    (3567050.000, 3568000),
                    (3568880.000, 3570000),
                    (3571034.382, 3572000),
                    (3572990.000, 3574000),
                    (3575030.000, 3576000),
                    (3576860.000, 3578000),
                    (3579080.000, 3580000),
                    (3581120.000, 3582000),
                    (3583110.000, 3584000),
                    (3585120.000, 3586000),
                    (3587120.000, 3588000),
                    (3589100.000, 3590000),
                    (3591120.000, 3592000)
                }
            }
        };

        [Fact]
        public void FindModelXOfJumps_58700To58750_Returns3559070And3560000()
        {
            double fromStation = 58700;
            double toStation = 58750;
            var expectedJumps = new List<(double, double)>() {
                (3559070, 3560000)
            };

            List<(double, double)> actualJumps = 
                StationToXConverter.FindModelXsOfJumps(fromStation, toStation);

            Assert.Equal(expectedJumps, actualJumps);
        }
    }
}
