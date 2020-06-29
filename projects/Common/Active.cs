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

        public static void WriteMessage(string message)
        {
            Editor.WriteMessage(message);
        }

        public static void DeleteAllEntitiesOnLayer(string layername)
        {
            TypedValue[] selectionCriteria =
            new TypedValue[] { new TypedValue((int)DxfCode.LayerName, layername) };
            SelectionFilter filter = new SelectionFilter(selectionCriteria);
            PromptSelectionResult result = Active.Editor.SelectAll(filter);
            SelectionSet set = result.Value;
            ObjectId[] ids = set.GetObjectIds();
            foreach (ObjectId id in ids) {
                id.GetObject(OpenMode.ForWrite).Erase();
            }
        }

        public static Transaction StartTransaction()
        {
            return Database.TransactionManager.StartTransaction();
        }
    }
}