using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;

namespace Common
{
    public static class Active
    {
        public static Document Document {
            get { return Application.DocumentManager.MdiActiveDocument; }
        }

        public static Database Database {
            get { return Document.Database; }
        }

        public static CivilDocument CivilDocument {
            get { return CivilApplication.ActiveDocument; }
        }

        public static Editor Editor {
            get { return Document.Editor; }
        }

        public static void WriteMessage(string message) {
            Editor.WriteMessage(message);
        }

        public static Transaction StartTransaction() {
            return Database.TransactionManager.StartTransaction();
        }
    }
}
