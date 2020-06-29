using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.DatabaseServices;
using Common;
using CreatePolyFromAlignment.C3D.exc;
using CreatePolyFromAlignment.C3D.imp;
using CreatePolyFromAlignment.Engine;
using System;

[assembly: ExtensionApplication(typeof(CreatePolyFromAlignment.C3D.Initialization))]
[assembly: CommandClass(typeof(CreatePolyFromAlignment.C3D.Command))]

namespace CreatePolyFromAlignment.C3D
{
    public class Initialization : IExtensionApplication
    {
        public void Initialize()
        {
            Active.WriteMessage("\nCreatePolyFromAlignment geladen. Befehl starten mit \"CREATEPOLYFROMALIGNMENT\".");
        }

        public void Terminate()
        {
        }
    }

    public class Command
    {
        private AlignmentWrapper _wrappedAlignment;
        private ProfileWrapper _wrappedProfile;
        private PolylineWrapper _wrappedPolyline;

        private PolyFitter _polyFitter;

        private Transaction _transaction;

        [CommandMethod("CreatePolyFromAlignment")]
        public void CreatePolyFromAlignment()
        {
            using (_transaction = Active.StartTransaction()) {
                try {
                    UserInput();
                } catch (UserCancelledException ex) {
                    Active.WriteMessage(ex.Message);
                    return;
                } catch (AlignmentAttachedProfileException ex) {
                    Active.WriteMessage(ex.Message);
                    return;
                }

            DoTheWork();

            UpdateDrawing();
            _transaction.Commit();
            }
        }

        private void UserInput()
        {
            GetAlignmentAndProfile();
            CreateNewPoly();
            _polyFitter = new PolyFitter(_wrappedAlignment, _wrappedProfile, _wrappedPolyline);
            GetStartEndStationAndIncrement();
        }

        private void GetAlignmentAndProfile()
        {
            string message = "\nSelect alignment: ";
            _wrappedAlignment = new AlignmentWrapper(
                Active.Editor.GetEntity<Alignment>(message, OpenMode.ForRead));
            _wrappedProfile = _wrappedAlignment.GetWrappedProfile();
        }

        private void CreateNewPoly()
        {
            Polyline3d polyline = new Polyline3d();
            _transaction.AddObjectToActiveModelSpace(polyline);
            _wrappedPolyline = new PolylineWrapper(polyline);
        }

        private void GetStartEndStationAndIncrement()
        {
            GetStartStation();
            GetEndStation();
            GetIncrement();
        }

        private void GetStartStation()
        {
            string message = ("\nStart station for polyline creation: ");
            double defaultValue = _polyFitter.StartStation;
            double userinput = GetDouble(message, defaultValue);
            try {
                _polyFitter.StartStation = userinput;
            } catch (ArgumentException e) {
                Active.WriteMessage(e.Message);
                GetStartStation();
            }
        }

        private void GetEndStation()
        {
            string message = ("\nEnd station for polyline creation: ");
            double defaultValue = _polyFitter.EndStation;
            double userinput = GetDouble(message, defaultValue);
            try {
                _polyFitter.EndStation = userinput;
            } catch (ArgumentException e) {
                Active.WriteMessage(e.Message);
                GetEndStation();
            }
        }

        private void GetIncrement()
        {
            string message = ("\nMaximum vertex distance for non-straight segments: ");
            double defaultValue = _polyFitter.MaxIncrement;
            double userinput = GetDouble(message, defaultValue);
            try {
                _polyFitter.MaxIncrement = userinput;
            } catch (ArgumentException e) {
                Active.WriteMessage(e.Message);
                GetIncrement();
            }
        }

        private double GetDouble(string message, double defaultValue)
        {
            PromptDoubleOptions options = new PromptDoubleOptions(message) {
                AllowNone = false,
                AllowNegative = false,
                UseDefaultValue = true,
                DefaultValue = defaultValue
            };

            PromptDoubleResult result = Active.Editor.GetDouble(options);
            if (result.Status != PromptStatus.OK) {
                throw new UserCancelledException("\nUser cancelled.");
            }
            return result.Value;
        }

        private void DoTheWork()
        {
            _polyFitter.FitPoly();
        }

        private void UpdateDrawing()
        {
            Active.Database.TransactionManager.QueueForGraphicsFlush();
        }
    }
}