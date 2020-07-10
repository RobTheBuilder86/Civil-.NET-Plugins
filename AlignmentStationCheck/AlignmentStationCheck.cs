using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil;
using Autodesk.Civil.DatabaseServices;
using Common;


[assembly: ExtensionApplication(typeof(AlignmentStationTest.Initialization))]
[assembly: CommandClass(typeof(AlignmentStationTest.Command))]

namespace AlignmentStationTest
{
    public class Initialization : IExtensionApplication
    {
        public void Initialize()
        {
            Active.WriteMessage(
                "\nAlignment station check geladen. " + 
                "Befehl starten mit \"AlignmentStationTest\".");
        }

        public void Terminate()
        {
        }
    }

    public class Command
    {
        private Alignment _alignment;

        [CommandMethod("AlignmentStationTest")]
        public void AlignmentStationTest()
        {
            using (Transaction trans = Active.StartTransaction()) {
                try {
                    UserInput();
                    DoTheWork();
                } catch (UserCancelledException ex) {
                    Active.WriteMessage(ex.Message);
                    return;
                }
                trans.Commit();
            }
        }

        private void UserInput()
        {
            string message = "\nSelect alignment: ";
            _alignment =
                Active.Editor.GetEntity<Alignment>(message, OpenMode.ForRead);
        }

        private void DoTheWork()
        {
            while (true) {
                Point3d referencePoint = GetPoint();
                TryGettingStation(referencePoint);
            }
        }

        private Point3d GetPoint()
        {
            var options = new PromptPointOptions("\nSelect reference point: ");
            options.AllowNone = false;
            PromptPointResult result = Active.Editor.GetPoint(options);
            CheckForValidUserInput(result);
            Point3d point = result.Value;
            return point;
        }

        private void CheckForValidUserInput(PromptResult result)
        {
            if (result.Status == PromptStatus.OK || result.Status == PromptStatus.Keyword) {
                return;
            } else {
                throw new UserCancelledException("*Abbruch*");
            }
        }

        private void TryGettingStation(Point3d referencePoint)
        {
            try {
                double east = referencePoint.X;
                double north = referencePoint.Y;
                double station = 0;
                double offset = 0;
                _alignment.StationOffset(east, north, ref station, ref offset);
                double officialstation = _alignment.ToOfficialStation(station);
                Active.WriteMessage($"\nOffset: {offset:0.000}" + 
                                    $"\nStation: {station:0.000}" +
                                    $"\nOfficialStation: {officialstation:0.000}");
            } catch (PointNotOnEntityException) {
                Active.WriteMessage("Point cannot be projected onto alignment.");
            }
        }
    }
}