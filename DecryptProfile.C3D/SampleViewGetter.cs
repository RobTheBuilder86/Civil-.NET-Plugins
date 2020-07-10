using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Common;
using DeconstructSurfaceSampleView.Engine.HelperObjects;
using DeconstructSurfaceSampleView.C3D.imp;
using Microsoft.SqlServer.Server;

namespace DeconstructSurfaceSampleView.C3D
{
    internal class SampleViewGetter
    {
        private SurfaceSampleType _sampleType;
        private PolylineWrapper _wrappedPolyline;
        private ReferencePointData _referencePoint;
        private AlignmentWrapper _wrappedAlignment {
            get { 
                return DeconstructSurfaceSampleView.WrappedAlignment; 
            }
        }
        private double _verticalExaggeration = 1.0;
        private double _horizontalExaggeration = 1.0;


        internal SampleViewGetter(SurfaceSampleType type) => _sampleType = type;

        internal SurfaceSampleView GetSampleView()
        {
            GetPolylineAndReferencePoint();
            if (_sampleType == SurfaceSampleType.Along) {
                return new ViewAlong(_wrappedPolyline,
                                     _referencePoint,
                                     _verticalExaggeration,
                                     _horizontalExaggeration);
            } else {
                return new ViewAcross(_wrappedPolyline, 
                                      _referencePoint,
                                      _verticalExaggeration,
                                      _horizontalExaggeration);
            }
        }

        private void GetPolylineAndReferencePoint()
        {
            GetPolyline();
            GetReferencePoint();
        }

        private void GetPolyline()
        {
            string message;
            if (_sampleType == SurfaceSampleType.Along) {
                message = "\nSelect polyline representing profile: ";
            } else {
                message = "\nSelect polyline representing section: ";
            }
            _wrappedPolyline = new PolylineWrapper(
                Active.Editor.GetEntity<Polyline>(message, OpenMode.ForRead));
        }

        private void GetReferencePoint()
        {
            SimplePoint2d point = GetPoint();
            double referenceStation = GetReferenceStation();
            double referenceOffset = GetOffset();
            double referenceElevation = GetElevation();
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
            while (true) {
                double referenceStation = GetDouble("\nSpecify reference point station: ");
                if (referenceStation > _wrappedAlignment.OfficialEndStation) {
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

        private double GetOffset()
        {
            var options = new PromptDoubleOptions("\nSpecify reference point offset: ");
            options.AllowNone = false;
            options.AllowZero = true;
            options.AllowNegative = true;
            options.AppendKeywordsToMessage = true;
            options.Keywords.Add("Horizontal exaggeration");
            PromptDoubleResult result = Active.Editor.GetDouble(options);
            CheckForValidUserInput(result);
            if (result.Status == PromptStatus.Keyword) {
                GetExaggeration("horizontal", ref _horizontalExaggeration);
                // Call GetOffset again to have a value to return for offset
                return GetOffset();
            } 
            return result.Value;
        }

        private double GetElevation()
        {
            var options = new PromptDoubleOptions("\nSpecify reference point elevation: ");
            options.AllowNone = false;
            options.AllowZero = true;
            options.AllowNegative = true;
            options.AppendKeywordsToMessage = true;
            options.Keywords.Add("Vertical exaggeration");
            PromptDoubleResult result = Active.Editor.GetDouble(options);
            CheckForValidUserInput(result);
            if (result.Status == PromptStatus.Keyword) {
                GetExaggeration("vertical", ref _verticalExaggeration);
                // Call GetOffset again to have a value to return for offset
                return GetElevation();
            }
            return result.Value;
        }

        private void GetExaggeration(string exaggerationAdjective, ref double exaggeration)
        {
            string msg = "\nSpecify " + exaggerationAdjective + " exaggeration: ";
            var options = new PromptDoubleOptions(msg);
            options.AllowNone = false;
            options.AllowZero = false;
            options.AllowNegative = true;
            options.DefaultValue = 1.0;
            options.UseDefaultValue = true;
            PromptDoubleResult result = Active.Editor.GetDouble(options);
            CheckForValidUserInput(result);
            exaggeration = result.Value;
        }

        private double GetDouble(string message)
        {
            var options = new PromptDoubleOptions(message);
            options.AllowNone = false;
            options.AllowZero = true;
            options.AllowNegative = true;
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

    }
}
