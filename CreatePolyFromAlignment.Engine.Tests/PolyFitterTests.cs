//using System;
//using System.Collections.Generic;
//using CreatePolyFromAlignment.Engine.Interfaces;
//using CreatePolyFromAlignment.Engine.HelperObjects;

//using NSubstitute;
//using Xunit;
//using IAlignment = CreatePolyFromAlignment.Engine.Interfaces.IAlignment;

//namespace CreatePolyFromAlignment.Engine.Tests
//{
//    public class PolyFitterTests
//    {
//        private IAlignment alignment;
//        private IPolyline polyline;
//        private PolyFitter fitter;

//        public PolyFitterTests()
//        {
//            alignment = Substitute.For<IAlignment>();
//            polyline = Substitute.For<IPolyline>();
//            fitter = new PolyFitter(alignment, polyline);
//        }

//        [Theory]
//        [InlineData(0)]
//        [InlineData(-1.5)]
//        public void SetMaxIncrement_ZeroOrNegativeValue_ThrowsArgumentException
//            (double maxIncValue)
//        {
//            Assert.Throws<ArgumentException>(() => { fitter.MaxIncrement = maxIncValue; });
//        }

//        [Fact]
//        public void FitPoly_CalledWithoutArgs_CallsGetCoordsEveryMeter()
//        {
//            alignment.GetStartStation().Returns(0);
//            alignment.GetEndStation().Returns(2);


//            fitter.FitPoly();

//            alignment.Received().GetCoordsAtStation(0);
//            alignment.Received().GetCoordsAtStation(1);
//            alignment.Received().GetCoordsAtStation(2);
//        }

//        [Fact]
//        public void FitPoly_UnevenIncrement_CallsGetCoordsWithBestFitIncrement()
//        {
//            alignment.GetStartStation().Returns(0);
//            alignment.GetEndStation().Returns(1.8);

//            fitter.FitPoly();

//            alignment.Received().GetCoordsAtStation(0);
//            alignment.Received().GetCoordsAtStation(0.9);
//            alignment.Received().GetCoordsAtStation(1.8);
//        }

//        [Fact]
//        public void FitPoly_GetsCoordinates_CallsAddVertexAtThoseCoords()
//        {
//            alignment.GetStartStation().Returns(0);
//            alignment.GetEndStation().Returns(5);
//            alignment.GetCoordsAtStation(0).Returns((0, 0));
//            alignment.GetCoordsAtStation(5).Returns((3, 4));

//            fitter.FitPoly();

//            polyline.Received().AddVertex(0, 0);
//            polyline.Received().AddVertex(3, 4);
//        }

//        [Theory]
//        [InlineData(0, 5, 1.5, 1.25, 3.75)]
//        [InlineData(0, 10, 2, 4, 6)]
//        public void FitPoly_CustomIncrement_CalculatesCorrectStations(
//            double startStation,
//            double endStation,
//            double maxInc,
//            double expectedStation1,
//            double expectedStation2)
//        {
//            alignment.GetStartStation().Returns(startStation);
//            alignment.GetEndStation().Returns(endStation);

//            fitter.MaxIncrement = maxInc;
//            fitter.FitPoly();

//            alignment.Received().GetCoordsAtStation(startStation);
//            alignment.Received().GetCoordsAtStation(expectedStation1);
//            alignment.Received().GetCoordsAtStation(expectedStation2);
//            alignment.Received().GetCoordsAtStation(endStation);
//        }
//    }
//}
