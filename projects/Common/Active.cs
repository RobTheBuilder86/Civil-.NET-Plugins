using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;

namespace Common
{
    public static class Active
    {
        //static Active()
        //{
        //    Document = Application.DocumentManager.MdiActiveDocument;
        //}

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

        public static BlockTable BlockTableForRead (Transaction trans){
            return (BlockTable)trans.GetObject(Database.BlockTableId, OpenMode.ForRead);
        }

        public static BlockTableRecord ModelSpaceForWrite(Transaction trans)
        {
            return (BlockTableRecord)trans.GetObject(
                SymbolUtilityServices.GetBlockModelSpaceId(Database), 
                OpenMode.ForWrite);
        }

        public static void CreateLayerIFNonExisting(string layername)
        {
            Transaction trans = Active.StartTransaction();
            var layerTable = (LayerTable)trans.GetObject(Database.LayerTableId, OpenMode.ForRead);
            if (!layerTable.Has(layername)) {
                var newLayer = new LayerTableRecord() {
                    Name = layername
                };
                layerTable.UpgradeOpen();
                layerTable.Add(newLayer);
                trans.AddNewlyCreatedDBObject(newLayer, true);
            }
            trans.Commit();
        }

        public static void WriteMessage(string message)
        {
            Editor.WriteMessage(message);
        }

        public static void DeleteAllEntitiesOnLayer(string layername)
        {
            using (Transaction trans = Active.StartTransaction()) {
                TypedValue[] selectionCriteria =
                new TypedValue[] { new TypedValue((int)DxfCode.LayerName, layername) };
                SelectionFilter filter = new SelectionFilter(selectionCriteria);
                PromptSelectionResult result = Active.Editor.SelectAll(filter);
                if (result.Status == PromptStatus.OK) { 
                    SelectionSet set = result.Value;
                    ObjectId[] ids = set.GetObjectIds();
                    foreach (ObjectId id in ids) {
                        id.GetObject(OpenMode.ForWrite).Erase();
                    }
                }
                trans.Commit();
            }
        }

        public static Transaction StartTransaction()
        {
            return Database.TransactionManager.StartTransaction();
        }
    }
}