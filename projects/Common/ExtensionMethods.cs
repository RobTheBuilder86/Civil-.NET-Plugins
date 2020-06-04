using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace Common {
	public static class ExtensionMethods {

		public static T GetEntity<T>(this Editor editor, string message, OpenMode openMode)
			where T : Entity {
			var rxClass = RXObject.GetClass(typeof(T));

			var options = new PromptEntityOptions(message);
			options.SetRejectMessage(string.Format("Selected object is not an {0}", typeof(T).Name))
			;
			options.AddAllowedClass(typeof(T), false);

			var result = Active.Editor.GetEntity(options);

			if (result.Status != PromptStatus.OK) {
				throw new UserCancelledException("User cancelled!");
			}

			T entity = result.ObjectId.GetObject(openMode) as T;

			return entity;
		}

		public static double GetDouble(this Editor editor, string message, double defaultValue) {
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
	}
}
