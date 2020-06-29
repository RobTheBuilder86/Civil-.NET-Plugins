using System.Collections.Generic;
using Ulf.C3D.Helper;
using Ulf.Util;
using Xunit;
using Xunit.Abstractions;

namespace Ulf.Tests
{
    public class ModelCoordTranslatorTests
    {
        private readonly ITestOutputHelper output;

        public ModelCoordTranslatorTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TranslateLine_LineSpanningOneJump_ReturnsCorrectModelCoords()
        {
            CaseStation CSfrom = new CaseStation(CalculationCase.Case1, 55600);
            CaseStation CSto = new CaseStation(CalculationCase.Case4b, 55700);
            List<(SimplePoint2d, SimplePoint2d)> expected = new List<(SimplePoint2d, SimplePoint2d)> {
                (new SimplePoint2d(3553070.0, 5349924.0), new SimplePoint2d(3553120.0, 5349897.5)),
                (new SimplePoint2d(3554000.0, 5349897.5), new SimplePoint2d(3554050.0, 5349871.0))
            };

            List<(SimplePoint2d, SimplePoint2d)> actual = LineModelCoordTranslator.TranslateLine(CSfrom, CSto);
            for (int i = 0; i < actual.Count; i++) {
                output.WriteLine($"{i + 1:00} " + 
                    $"\n -> Exp. - Item1: {expected[i].Item1} --- Item2: {expected[i].Item2}" +
                    $"\n -> Act. - Item1: {actual[i].Item1} --- Item2: {actual[i].Item2}");
            }

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TranslateLine_LineSpanningMultipleJumps_ReturnsCorrectModelCoords()
        {
            CaseStation CSfrom = new CaseStation(CalculationCase.Case1, 55000);
            CaseStation CSto = new CaseStation(CalculationCase.Case4b, 59000);
            List<(SimplePoint2d, SimplePoint2d)> expected = new List<(SimplePoint2d, SimplePoint2d)> {
                (new SimplePoint2d(3552470, 5349924), new SimplePoint2d(3553120.00000003, 5349915.38750001)),
                (new SimplePoint2d(3554000, 5349915.38750001), new SimplePoint2d(3554910.00000002, 5349903.33000001)),
                (new SimplePoint2d(3556000, 5349903.33000001), new SimplePoint2d(3557110.00000003, 5349888.62250001)),
                (new SimplePoint2d(3558000, 5349888.62250001), new SimplePoint2d(3559070.00000003, 5349874.445)),
                (new SimplePoint2d(3560000, 5349874.445), new SimplePoint2d(3560259.9999999, 5349871.00000001)),
            };

            List<(SimplePoint2d, SimplePoint2d)> actual = LineModelCoordTranslator.TranslateLine(CSfrom, CSto);
            for (int i = 0; i < actual.Count; i++) {
                output.WriteLine($"{i + 1:00} " +
                    $"\n -> Exp. - Item1: {expected[i].Item1} --- Item2: {expected[i].Item2}" +
                    $"\n -> Act. - Item1: {actual[i].Item1} --- Item2: {actual[i].Item2}");
            }

            Assert.Equal(expected, actual);
        }

    }
}
