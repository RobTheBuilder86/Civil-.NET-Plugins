using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.DatabaseServices;
using Common;
using System;
using System.Windows;
using Ulf.C3D.Imp;
using Ulf.C3D.Helper;
using Ulf.Engine;
using Microsoft.SqlServer.Server;

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
        private Transaction _transaction;
        private SurfaceWrapper _wrappedSurface;
        private string _blockname = "Fallzuordnung-Punkt";
        private string _excelFilePath;

        [CommandMethod("UpdateULF")]
        public void UpdateULF()
        {
            using (_transaction = Active.StartTransaction()) {
                try {
                    UserInput();
                } catch (UserCancelledException ex) {
                    Active.WriteMessage(ex.Message);
                    return;
                }
                var CaseEntCreator = new EntityCreator(_transaction, _blockname, "IAG_BWB_Fallzuordnung");
                var caseHandler = new CaseAssignmentEntityHandler(CaseEntCreator);
                var ulfEntHandler = new UlfEntityHandler(_transaction);
                var ulfHandler = new UlfHandler(_wrappedSurface, caseHandler, ulfEntHandler);

                ulfHandler.HandleTheUlf(_excelFilePath);
                ulfEntHandler.AddVertices();

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
            ObjectIdCollection surfaceIds = Active.CivilDocument.GetSurfaceIds();
            foreach(ObjectId surfaceId in surfaceIds) {
                var surface = (TinSurface)_transaction.GetObject(surfaceId, OpenMode.ForRead);
                if (surface != null && surface.Name == "V_Schwing_m_Tagewerken_normiert_komplett") {
                    _wrappedSurface = new SurfaceWrapper(surface);
                    break;
                }
            }
            if (_wrappedSurface == null) {
                string message = "\nSelect TinSurface: ";
                _wrappedSurface = new SurfaceWrapper(
                    Active.Editor.GetEntity<TinSurface>(message, OpenMode.ForRead));
            }
        }

        private void GetExcelFilePath()
        {
            // Prompt user for output file.
            //OpenFileDialog ofd = new OpenFileDialog(
            //    "Exceldatei mit Fallzuordnung öffnen",
            //    null,
            //    "xlsx",
            //    "Excel datei",
            //    OpenFileDialog.OpenFileDialogFlags.NoFtpSites);
            //System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            //if (dr != System.Windows.Forms.DialogResult.OK) {
            //    throw new UserCancelledException("\nNo valid Excel file selected.");
            //}
            // = ofd.Filename;
            _excelFilePath = "T:\\az\\as\\IAG\\IAG1003_NBS_W-U_22-23-24-25a1_SV\\PFA_23_Albhochflaeche\\Ing\\VE230-XY\\Dyn-Stab\\Plaene_Dyn-VB\\Excel\\20200127_Dyn-GB_Fallunterscheidung.xlsx";
        }
    }
}

