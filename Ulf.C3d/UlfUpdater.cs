using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.DatabaseServices;
using Common;
using System;
using System.Windows;
using Ulf.C3D.Imp;

[assembly: ExtensionApplication(typeof(Ulf.C3D.Initialization))]
[assembly: CommandClass(typeof(Ulf.C3D.UlfUpdater))]

namespace Ulf.C3D
{
    public class Initialization : IExtensionApplication
    {
        public void Initialize()
        {
            Active.WriteMessage("\nÜberlastungsfaktor updater geladen. Befehl starten mit \"UPDATEULF\".");
        }

        public void Terminate()
        {
        }
    }

    public class UlfUpdater
    {
        private SurfaceWrapper _wrappedSurface;
        private Transaction _transaction;
        private string _excelFilePath;

        [CommandMethod("DeconstructSampleView")]
        public void DeconstructSampleView()
        {
            using (_transaction = Active.StartTransaction()) {
                try {
                    UserInput();
                } catch (UserCancelledException ex) {
                    Active.WriteMessage(ex.Message);
                    return;
                }


                _transaction.Commit();
            }
        }

        private void UserInput()
        {
            GetSurface();
            GetExcelFilePath();
        }

        private void GetSurface()
        {
            string message = "\nSelect TinSurface: ";
            _wrappedSurface = new SurfaceWrapper(
                Active.Editor.GetEntity<TinSurface>(message, OpenMode.ForRead));
        }

        private void GetExcelFilePath()
        {
            // Prompt user for output file.
            OpenFileDialog ofd = new OpenFileDialog(
                "Exceldatei mit Fallzuordnung öffnen",
                null,
                "xlsx",
                "Excel datei",
                OpenFileDialog.OpenFileDialogFlags.NoFtpSites);
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK) {
                throw new UserCancelledException("\nNo valid Excel file selected.");
            }
            _excelFilePath = ofd.Filename;
        }
    }
}

