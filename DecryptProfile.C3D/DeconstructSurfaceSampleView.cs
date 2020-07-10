using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.DatabaseServices;
using Common;
using DeconstructSurfaceSampleView.C3D.imp;
using DeconstructSurfaceSampleView.Engine;
using DeconstructSurfaceSampleView.Engine.HelperObjects;
using System.Collections.Generic;

[assembly: ExtensionApplication(typeof(DeconstructSurfaceSampleView.C3D.Initialization))]
[assembly: CommandClass(typeof(DeconstructSurfaceSampleView.C3D.DeconstructSurfaceSampleView))]


namespace DeconstructSurfaceSampleView.C3D
{
    public class Initialization : IExtensionApplication
    {
        public void Initialize()
        {
            Active.WriteMessage("\nDeconstructSurfaceSampleView geladen. Befehl mit \"DECONSTRUCTSAMPLEVIEW\" starten.");
        }

        public void Terminate()
        {
        }
    }

    public class DeconstructSurfaceSampleView
    {
        internal static AlignmentWrapper WrappedAlignment;

        private PolylineWrapper _wrappedPolyline;
        private SurfaceSampleType _type;
        private ReferencePointData _referencePoint;
        private SampleViewGetter _viewGetter;

        private SampleViewDeconstructor _deconstructor;

        private Transaction _transaction;

        [CommandMethod("DeconstructSampleView")]
        public void DeconstructSampleView()
        {
            using (_transaction = Active.StartTransaction()) {
                try {
                    InitialSetup();
                    while (true) {
                        Deconstruct();
                        Active.Database.TransactionManager.QueueForGraphicsFlush();
                    }
                } catch (UserCancelledException ex) {
                    Active.WriteMessage(ex.Message);
                    return;
                } finally {
                    _transaction.Commit();
                }
            }
        }

        private void InitialSetup()
        {
            GetAlignment();
            GetCommonViewType();
            _deconstructor = new SampleViewDeconstructor(WrappedAlignment);
            _viewGetter = new SampleViewGetter(_type);
        }

        private void GetAlignment()
        {
            string message = "\nSelect alignment: ";
            WrappedAlignment = new AlignmentWrapper(
                Active.Editor.GetEntity<Alignment>(message, OpenMode.ForRead));
        }

        private void GetCommonViewType()
        {
            PromptKeywordOptions options = new PromptKeywordOptions("\nSpecify kind of profile to be deconstructed.");
            options.AllowNone = false;
            options.AppendKeywordsToMessage = true;
            options.Keywords.Add("Profile");
            options.Keywords.Add("Section");
            PromptResult result = Active.Editor.GetKeywords(options);
            CheckForValidUserInput(result);
            if (result.StringResult == "Profile") {
                _type = SurfaceSampleType.Along;
            } else {
                _type = SurfaceSampleType.Across;
            }
        }

        private void CheckForValidUserInput(PromptResult result)
        {
            if (result.Status == PromptStatus.OK || result.Status == PromptStatus.Keyword) {
                return;
            } else {
                throw new UserCancelledException("\nUser cancelled.");
            }
        }

        private void Deconstruct()
        {
            SurfaceSampleView view = _viewGetter.GetSampleView();
            List<SimplePoint3d> points = _deconstructor.DeconstructSampleView(view);
            CreatePolyline(points);
        }

        private void CreatePolyline(List<SimplePoint3d> points)
        {
            Polyline3d polyline = new Polyline3d();
            _transaction.AddObjectToActiveModelSpace(polyline);
            foreach (SimplePoint3d point in points) {
                var point3d = new Point3d(point.X, point.Y, point.Z);
                using (PolylineVertex3d vertex = new PolylineVertex3d(point3d)) {
                    polyline.AppendVertex(vertex);
                }
            }
        }
    }
}