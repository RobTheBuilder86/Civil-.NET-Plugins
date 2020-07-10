using Common;
using Ulf.Engine.Interfaces;
using Ulf.Util;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace Ulf.C3D.Imp
{
    class UlfEntityHandler : IUlfEntityHandler
    {
        private string _styleName = "Dreiecksummaschung in Grün";
        private string _surfaceName = "Min-ULF";
        private TinSurface _surface;
        private Transaction _trans;
        private string _layername = "IAG_BWB_Min-Ulf";
        private double _yDataBandInsert = 5350000;
        private double _yToTopOfBand = 397;
        private double _yToBottomOfBand = 421;
        private double _yTopOfBand;
        private double _yBottomOfBand;
        private Point3dCollection _points;

        public UlfEntityHandler(Transaction trans)
        {
            _trans = trans;
            _points = new Point3dCollection();
            _yTopOfBand = _yDataBandInsert - _yToTopOfBand;
            _yBottomOfBand = _yDataBandInsert - _yToBottomOfBand;
            Active.CreateLayerIFNonExisting(_layername);
            CreateSurface();
        }


        private void CreateSurface()
        {
            ObjectId styleId = Active.CivilDocument.Styles.SurfaceStyles[_styleName];
            ObjectId surfaceId = TinSurface.Create(Active.Database, _surfaceName);
            _surface = (TinSurface) _trans.GetObject(surfaceId, OpenMode.ForWrite);
            _surface.StyleId = styleId;
            _surface.BuildOptions.UseMaximumTriangleLength = true;
            _surface.BuildOptions.MaximumTriangleLength = 41;
            _surface.Layer = _layername;
        }

        public void AddUlfEntity(double station, double ulf)
        {
            double x = StationToXConverter.ToX(station);
            _points.Add(new Point3d(x, _yTopOfBand, ulf));
            _points.Add(new Point3d(x, _yBottomOfBand, ulf));
        }

        public void Reset()
        {
            //Active.DeleteAllEntitiesOnLayer(_layername);
        }

        public void AddVertices()
        {
            _surface.AddVertices(_points);
        }
    }
}
