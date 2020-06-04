using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.DatabaseServices;
using Common;
using SurpressSurface.Engine;

[assembly: ExtensionApplication(typeof(SurpressSurface.C3D.Initialization))]
[assembly: CommandClass(typeof(SurpressSurface.C3D.Command))]

namespace SurpressSurface.C3D {
	public class Initialization : IExtensionApplication {

		public void Initialize() {
			Active.WriteMessage("\nSurpressSurface geladen. Befehl starten mit \"SURPRESSSURFACE\".");
		}

		public void Terminate() { 
		}

	}

	public class Command {
		private SurfaceWrapper top;
		private SurfaceWrapper bottom;

		[CommandMethod("SurpressSurface")]
		public void SurpressSurface() {
			using (Transaction trans = Active.StartTransaction()) {
				try {
					UserInput();
				} catch (UserCancelledException ex) {
					Active.WriteMessage(ex.Message);
					return;
				}

				DoTheWork();
				UpdateDrawing();

				trans.Commit();
			}
		}

		private void UserInput() {
			top = GetSurfaceWrapper("\nTop surface: ", OpenMode.ForRead);
			bottom = GetSurfaceWrapper("\nBottom surface: ", OpenMode.ForWrite);
			SurfaceSurpressor.MinimumDistance = 
				Active.Editor.GetDouble(
					"\nSpecify minimum distance between top and and bottom: ",
					SurfaceSurpressor.MinimumDistance);
		}

		private SurfaceWrapper GetSurfaceWrapper(string message, OpenMode openMode) {
			TinSurface surface =
				Active.Editor.GetEntity<TinSurface>(message, openMode);
			return new SurfaceWrapper(surface);
		}

		private void DoTheWork() {
				SurfaceSurpressor surpressor = new SurfaceSurpressor(top, bottom);

				surpressor.SurpressBottomSurface();
		}

		private void UpdateDrawing() {
			top._surface.Rebuild();
			bottom._surface.Rebuild();
			Active.Database.TransactionManager.QueueForGraphicsFlush();
		}
	}
}
