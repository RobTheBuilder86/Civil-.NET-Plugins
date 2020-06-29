using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using AcadEntity = Autodesk.AutoCAD.DatabaseServices.Entity;
using CivilEntity = Autodesk.Civil.DatabaseServices.Entity;

namespace Common
{
    public static class ExtensionMethods
    {
        public static T GetEntity<T>(this Editor editor, string message, OpenMode openMode)
            where T : Entity
        {
            var options = new PromptEntityOptions(message);
            options.SetRejectMessage($"Selected object must be of type {typeof(T).Name}");
            options.AddAllowedClass(typeof(T), false);

            var result = Active.Editor.GetEntity(options);

            if (result.Status != PromptStatus.OK) {
                throw new UserCancelledException("User cancelled!");
            }

            T entity = result.ObjectId.GetObject(openMode) as T;

            return entity;
        }

        public static double GetDouble(this Editor editor, string message, double defaultValue)
        {
            PromptDoubleOptions options = new PromptDoubleOptions(message);
            options.DefaultValue = defaultValue;
            options.AllowNegative = true;
            options.AllowNone = true;
            options.UseDefaultValue = true;
            PromptDoubleResult result = Active.Editor.GetDouble(options);
            if (result.Status != PromptStatus.OK) {
                throw new UserCancelledException("User cancelled.");
            }
            return result.Value;
        }

        public static void AddObjectToActiveModelSpace<T>(this Transaction trans, T newObject)
            where T : Entity
        {
            T entity = newObject as T;
            BlockTableRecord modelSpace = trans.GetActiveModelSpace();

            modelSpace.AppendEntity(entity);
            trans.AddNewlyCreatedDBObject(entity, true);
        }

        private static BlockTableRecord GetActiveModelSpace(this Transaction trans)
        {
            BlockTable blockTable = trans.GetObject(Active.Database.BlockTableId,
                                                    OpenMode.ForRead) as BlockTable;
            BlockTableRecord modelspace =
                trans.GetObject(blockTable[BlockTableRecord.ModelSpace],
                                OpenMode.ForWrite) as BlockTableRecord;
            return modelspace;
        }
    }
}