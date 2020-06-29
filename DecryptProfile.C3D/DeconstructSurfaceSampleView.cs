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
        private AlignmentWrapper _wrappedAlignment;

        private PolylineWrapper _wrappedPolyline;
        private SurfaceSampleType _type;
        private ReferencePointData _referencePoint;

        private SampleViewDeconstructor _deconstructor;

        private Transaction _transaction;

        [CommandMethod("DeconstructSampleView")]
        public void DeconstructSampleView()
        {
            using (_transaction = Active.StartTransaction()) {
                try {
                    InitialSetup();
                    while (true) {
                        UserInput();
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
            GetCommonProfileInfo();
            _deconstructor = new SampleViewDeconstructor(_wrappedAlignment);
        }

        private void GetAlignment()
        {
            string message = "\nSelect alignment: ";
            _wrappedAlignment = new AlignmentWrapper(
                Active.Editor.GetEntity<Alignment>(message, OpenMode.ForRead));
        }

        private void GetCommonProfileInfo()
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

        private void UserInput()
        {
            GetPolyline();
            GetReferencePoint();
        }

        private void GetPolyline()
        {
            string message;
            if(_type == SurfaceSampleType.Along) {
                message = "\nSelect polyline representing profile: ";
            } else {
                message = "\nSelect polyline representing section: ";
            }
            _wrappedPolyline= new PolylineWrapper(
                Active.Editor.GetEntity<Polyline>(message, OpenMode.ForRead));
        }

        private void GetReferencePoint()
        {
            SimplePoint2d point = GetPoint();
            double referenceStation = GetReferenceStation();
            double referenceOffset = GetDouble("\nSpecify reference point offset: ");
            double referenceElevation = GetDouble("\nSpecify reference point elevation: ");
            AlignmentPoint alignmentPoint = new AlignmentPoint(referenceStation, 
                                                               referenceOffset, 
                                                               referenceElevation);
            _referencePoint = new ReferencePointData(point, alignmentPoint);
        }

        private SimplePoint2d GetPoint()
        {
            var options = new PromptPointOptions("\nSelect reference point: ");
            options.AllowNone = false;
            PromptPointResult result = Active.Editor.GetPoint(options);
            CheckForValidUserInput(result);
            Point3d point3d = result.Value;
            return new SimplePoint2d(point3d.X, point3d.Y);
        }

        private double GetReferenceStation()
        {
            while(true) {
                double referenceStation = GetDouble("\nSpecify reference point station: ");
                if(referenceStation > _wrappedAlignment.OfficialEndStation) {
                    Active.WriteMessage("\nReference station cannot be behind alignment " + 
                                        $"ending station ({_wrappedAlignment.OfficialEndStation}).");
                } else if (referenceStation < _wrappedAlignment.StartStation) {
                    Active.WriteMessage("\nReference station cannot be before alignment " +
                                        $"starting station ({_wrappedAlignment.StartStation}).");
                } else {
                    return referenceStation;
                }
            }
        }

        private double GetDouble(string message) {
            var options = new PromptDoubleOptions(message);
            options.AllowNone = false;
            options.AllowZero = true;
            options.AllowNegative = false;
            PromptDoubleResult result = Active.Editor.GetDouble(options);
            CheckForValidUserInput(result);
            return result.Value;
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
            List<SimplePoint3d> Points = _deconstructor.DeconstructSampleView(GetSampleView());
            Polyline3d polyline = new Polyline3d();
            _transaction.AddObjectToActiveModelSpace(polyline);
            foreach (SimplePoint3d point in Points) {
                var point3d = new Point3d(point.X, point.Y, point.Z);
                using (PolylineVertex3d vertex = new PolylineVertex3d(point3d)) {
                    polyline.AppendVertex(vertex);
                }
            }
        }

        private SurfaceSampleView GetSampleView()
        {
            if (_type == SurfaceSampleType.Along) {
                return new ViewAlong(_wrappedPolyline, _referencePoint);
            } else {
                return new ViewAcross(_wrappedPolyline, _referencePoint);
            }
        }
    }

    enum SurfaceSampleType
    {
        Along,
        Across
    }
}