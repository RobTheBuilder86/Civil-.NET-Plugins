using CreatePolyFromAlignment.Engine.Interfaces;
using CreatePolyFromAlignment.Engine.HelperObjects;
using NSubstitute;
using System.Collections.Generic;
using Xunit;
using System;
using Xunit.Abstractions;

namespace CreatePolyFromAlignment.Engine.Tests
{
    public class StationComputerTests
    {
        private readonly ITestOutputHelper output;
        private IAlignment Alignment;
        private IProfile Profile;

        public StationComputerTests(ITestOutputHelper output)
        {
            Alignment = Substitute.For<IAlignment>();
            Profile = Substitute.For<IProfile>();
            this.output = output;
        }

        [Theory]
        [MemberData(nameof(OnCreation_InvalidSegmentsData))]
        public void OnCreation_InvalidSegments_ThrowsArgumentException(List<StationSegment> AlignmentSegments,
                                                                       List<StationSegment> ProfileSegments,
                                                                       string Info)
        {
            Alignment.GetSegments().Returns(AlignmentSegments);
            Profile.GetSegments().Returns(ProfileSegments);
            StationComputer testcomputer;

            ArgumentException ex =
                Assert.Throws<ArgumentException>(() =>
                testcomputer = new StationComputer(Alignment, Profile));
            output.WriteLine(Info);
            output.WriteLine($"Exception Message: {ex.Message}");
        }
        public static IEnumerable<Object[]> OnCreation_InvalidSegmentsData =>
            new List<object[]>{
                new object[] {
                               new List<StationSegment> {
                                   new StationSegment(1.0, 3.0, SegmentType.straight),
                                   new StationSegment(4.0, 5.0, SegmentType.straight),
                               },
                               new List<StationSegment> {
                                   new StationSegment(1.0, 3.0, SegmentType.straight),
                                   new StationSegment(3.0, 5.0, SegmentType.straight),
                               },
                               "Test run with incoherent Alignment segments"},
                new object[] {
                               new List<StationSegment> {
                                   new StationSegment(1.0, 4.0, SegmentType.straight),
                                   new StationSegment(4.0, 5.0, SegmentType.straight),
                               },
                               new List<StationSegment> {
                                   new StationSegment(1.0, 3.0, SegmentType.straight),
                                   new StationSegment(4.0, 5.0, SegmentType.straight), },
                               "Test run with incoherent Profile segments"},
                new object[] {
                               new List<StationSegment> {
                                   new StationSegment(1.0, 4.0, SegmentType.straight),
                                   new StationSegment(4.0, 5.0, SegmentType.straight), },
                               new List<StationSegment> {
                                   new StationSegment(6.0, 9.0, SegmentType.straight),
                                   new StationSegment(9.0, 10.0, SegmentType.straight), },
                               "Test run with profile segments that start behind the end of the alignment segments."},
                new object[] {
                               new List<StationSegment> {
                                   new StationSegment(6.0, 9.0, SegmentType.straight),
                                   new StationSegment(9.0, 10.0, SegmentType.straight), },
                               new List<StationSegment> {
                                   new StationSegment(1.0, 4.0, SegmentType.straight),
                                   new StationSegment(4.0, 5.0, SegmentType.straight), },
                               "Test run with profile segments that end before the start of the alignment segments."},
            };

        [Theory]
        [MemberData(nameof(OnCreation_ValidSegmentData))]
        public void OnCreation_ValidSegments_ComputesCorrectIndizes(List<StationSegment> AlignmentSegments,
                                                                    List<StationSegment> ProfileSegments,
                                                                    int ExpectedAlignmentIndex,
                                                                    int ExpectedProfileIndex)
        {
            Alignment.GetSegments().Returns(AlignmentSegments);
            Profile.GetSegments().Returns(ProfileSegments);
            StationComputer testcomputer = new StationComputer(Alignment, Profile);

            int ActualAlignmentIndex = testcomputer.AlignmentSegmentIndex;
            int ActualProfileIndex = testcomputer.ProfileSegmentIndex;

            Assert.Equal(ExpectedAlignmentIndex, ActualAlignmentIndex);
            Assert.Equal(ExpectedProfileIndex, ActualProfileIndex);
        }
        public static IEnumerable<Object[]> OnCreation_ValidSegmentData =>
        new List<object[]>{
            new object[] {
                            new List<StationSegment> {
                                new StationSegment(1.0, 2.0, SegmentType.straight),
                                new StationSegment(2.0, 5.0, SegmentType.straight),
                            },
                            new List<StationSegment> {
                                new StationSegment(1.0, 3.0, SegmentType.straight),
                                new StationSegment(3.0, 5.0, SegmentType.straight),
                            },
                            0,
                            0,
            },
            new object[] {
                            new List<StationSegment> {
                                new StationSegment(3.0, 5.0, SegmentType.straight),
                                new StationSegment(5.0, 8.0, SegmentType.straight),
                            },
                            new List<StationSegment> {
                                new StationSegment(1.0, 2.0, SegmentType.straight),
                                new StationSegment(2.0, 5.0, SegmentType.straight),
                            },
                            0,
                            1,
            },
            new object[] {
                            new List<StationSegment> {
                                new StationSegment(1.0, 2.0, SegmentType.straight),
                                new StationSegment(2.0, 5.0, SegmentType.straight),
                            },
                            new List<StationSegment> {
                                new StationSegment(3.0, 5.0, SegmentType.straight),
                                new StationSegment(5.0, 8.0, SegmentType.straight),
                            },
                            1,
                            0,
            },
        };

        [Theory]
        [MemberData(nameof(Stations_OnlyStraightSegmentsData))]
        public void Stations_OnlyStraightSegments_ComputesCorrectStations(List<StationSegment> AlignmentSegments,
                                                                           List<StationSegment> ProfileSegments,
                                                                           List<double> ExpectedStationList,
                                                                           string Info)
        {
            Alignment.GetSegments().Returns(AlignmentSegments);
            Profile.GetSegments().Returns(ProfileSegments);
            StationComputer testcomputer = new StationComputer(Alignment, Profile);

            var ActualStationList = testcomputer.Stations;

            Assert.Equal(ExpectedStationList, ActualStationList);
            output.WriteLine(Info);
        }
        public static IEnumerable<Object[]> Stations_OnlyStraightSegmentsData =>
        new List<object[]>{
            new object[] {
                            new List<StationSegment> {
                                new StationSegment(1.0, 2.0, SegmentType.straight),
                                new StationSegment(2.0, 5.0, SegmentType.straight),
                            },
                            new List<StationSegment> {
                                new StationSegment(1.0, 3.0, SegmentType.straight),
                                new StationSegment(3.0, 5.0, SegmentType.straight),
                            },
                            new List<double> { 1.0, 2.0, 3.0, 5.0},
                            "Start and end station of profile and alignment are equal. But individual segment ends vary."
            },
            new object[] {
                            new List<StationSegment> {
                                new StationSegment(4.0, 10.0, SegmentType.straight),
                                new StationSegment(10.0, 27.0, SegmentType.straight),
                            },
                            new List<StationSegment> {
                                new StationSegment(14, 15, SegmentType.straight),
                                new StationSegment(15, 16, SegmentType.straight),
                                new StationSegment(16, 17, SegmentType.straight),
                            },
                            new List<double> { 14,15,16,17},
                            "All profile segments inside one alignment segment."
            },
        };


        [Theory]
        [MemberData(nameof(Stations_WithNonStraightSegmentData))]
        public void Stations_WithNonStraightSegment_ComputesCorrectStations(List<StationSegment> AlignmentSegments,
                                                                   List<StationSegment> ProfileSegments,
                                                                   List<double> ExpectedStationList,
                                                                   string Info)
        {
            Alignment.GetSegments().Returns(AlignmentSegments);
            Profile.GetSegments().Returns(ProfileSegments);
            StationComputer testcomputer = new StationComputer(Alignment, Profile);

            var ActualStationList = testcomputer.Stations;

            output.WriteLine(Info);
            Assert.Equal(ExpectedStationList, ActualStationList);
        }
        public static IEnumerable<Object[]> Stations_WithNonStraightSegmentData =>
        new List<object[]>{
            new object[] {
                            new List<StationSegment> {
                                new StationSegment(1.0, 2.0, SegmentType.straight),
                                new StationSegment(2.0, 4.0, SegmentType.straight),
                                new StationSegment(4.0, 4.5, SegmentType.straight),
                                new StationSegment(4.5, 7.0, SegmentType.straight),
                            },
                            new List<StationSegment> {
                                new StationSegment(1.0, 3.0, SegmentType.straight),
                                new StationSegment(3.0, 5.0, SegmentType.nonstraight),
                                new StationSegment(5.0, 10.0, SegmentType.nonstraight),
                            },
                            new List<double> { 1.0, 2.0, 3.0, 4.0, 4.5, 5.0, 6.0, 7.0},
                            "One straight segment in profile overlapping one whole alignment segment."
            },
        };

        [Theory]
        [MemberData(nameof(Stations_CustomStartEndStationAndIncrement))]
        public void Stations_CustomStartEndStationAndIncrement_ComputesCorrectStations(List<StationSegment> AlignmentSegments,
                                                                                       List<StationSegment> ProfileSegments,
                                                                                       double startStation,
                                                                                       double endStation,
                                                                                       double increment,
                                                                                       List<double> ExpectedStationList)
        {
            Alignment.GetSegments().Returns(AlignmentSegments);
            Profile.GetSegments().Returns(ProfileSegments);
            StationComputer testcomputer = new StationComputer(Alignment, Profile);
            testcomputer.StartStation = startStation;
            testcomputer.EndStation = endStation;
            testcomputer.MaxIncrement = increment;

            var ActualStationList = testcomputer.Stations;
            var comparer = new TolerantDoubleEqualityComparer(0.000000001);
            Assert.Equal(ExpectedStationList, ActualStationList, comparer);
        }
        public static IEnumerable<Object[]> Stations_CustomStartEndStationAndIncrement =>
        new List<object[]>{
            new object[] {
                            new List<StationSegment> {
                                new StationSegment(2.0, 5.0, SegmentType.straight),
                            },
                            new List<StationSegment> {
                                new StationSegment(2.0, 5.0, SegmentType.nonstraight),
                            },
                            2.3,
                            3.2,
                            0.3,
                            new List<double> { 2.3, 2.6, 2.9, 3.2},
            },
        };
    }

    class TolerantDoubleEqualityComparer : IEqualityComparer<double>
    {
        double tolerance = 0.0001;

        public TolerantDoubleEqualityComparer(double tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(double x, double y)
        {
            if (x < y + tolerance || x > y - tolerance) {
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
