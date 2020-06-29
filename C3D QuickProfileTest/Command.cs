using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.DatabaseServices;
using Common;


[assembly: ExtensionApplication(typeof(QuickProfileTest.Initialization))]
[assembly: CommandClass(typeof(QuickProfileTest.Command))]

namespace QuickProfileTest
{
    public class Initialization : IExtensionApplication
    {
        public void Initialize()
        {
            Active.WriteMessage("\nQuickProfileTest geladen. Befehl starten mit \"QuickProfileTest\".");
        }

        public void Terminate()
        {
        }
    }

    public class Command
    {
        private Profile _profile;

        [CommandMethod("QuickProfileTest")]
        public void QuickProfileTest()
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
            string message = "\nSelect profile: ";
            _profile =
                Active.Editor.GetEntity<Profile>(message, OpenMode.ForRead);
        }

        private void DoTheWork()
        {
            int count = 1;
            foreach (ProfileEntity entity in _profile.Entities) {
                string message = "\n";
                message += $"Entity {count++}:   " +
                        $"Type = {entity.EntityType},   ";
                Active.WriteMessage(message);
            }
        }
    }
}