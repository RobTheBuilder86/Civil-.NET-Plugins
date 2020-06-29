using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.DatabaseServices;
using Common;


[assembly: ExtensionApplication(typeof(C3D_QuickProfileTest.Initialization))]
[assembly: CommandClass(typeof(C3D_QuickProfileTest.Command))]

namespace C3D_QuickProfileTest
{
    public class Initialization : IExtensionApplication
    {
        public void Initialize()
        {
            Active.WriteMessage("\nQuick alignment test geladen. Befehl starten mit \"QUICKALIGNMENTTEST\".");
        }

        public void Terminate()
        {
        }
    }

    public class Command
    {
        private Alignment _alignment;

        [CommandMethod("QuickAlignmentTest")]
        public void QuickAlignmentTest()
        {
            using (Transaction transaction = Active.StartTransaction()) {
                try {
                    UserInput();
                } catch (UserCancelledException ex) {
                    Active.WriteMessage(ex.Message);
                    return;
                }
                DoTheWork();

                transaction.Commit();
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
            int count = 1;
            foreach (AlignmentEntity entity in _alignment.Entities) {
                string message = "\n";
                message += $"Entity {count++}:   " +
                        $"Type = {entity.EntityType},   " +
                        $"NumberOfSubentities = {entity.SubEntityCount}";
                Active.WriteMessage(message);
                PrintSubEntities(entity);
            }
        }

        private void PrintSubEntities(AlignmentEntity entity)
        {
            for (int i = 0; i < entity.SubEntityCount; i++) {
                string message = "\n";
                message += $"    Subentity {i + 1}:   " +
                    $"\n        Type         = {entity[i].GetType()}" +
                    $"\n        StartStation = {entity[i].StartStation}" +
                    $"\n        EndStation   = {entity[i].EndStation}";
                Active.WriteMessage(message);
            }
        }
    }
}