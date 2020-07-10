using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.DatabaseServices;
using Common;
using System;
using System.Collections.Generic;
using Ulf.C3D.Ext;
using Ulf.Util;

namespace Ulf.C3D.Helper
{
    class EntityCreator
    {
        private Transaction _trans;
        private BlockTable _blocktable;
        private BlockTableRecord _modelspace;
        private string _blockname;
        private string _layername;

        public EntityCreator(Transaction trans, 
                             string blockname,
                             string layername)
        {
            _trans = trans;
            _blocktable = Active.BlockTableForRead(_trans);
            _modelspace = Active.ModelSpaceForWrite(_trans);
            if(!_blocktable.Has(blockname)) {
                throw new ArgumentException(
                    "Error while constructing new EntityCreator: No Block named " + 
                    $"\"{blockname}\" found in provided blocktable.");
            }
            _blockname = blockname;
            Active.CreateLayerIFNonExisting(layername);
            _layername = layername;

        }

        public void Reset()
        {
            Active.DeleteAllEntitiesOnLayer(_layername);
        }

        public void CreateBlockInsert(CaseStation cs)
        {
            Point3d insertPoint = new Point3d(cs.X, cs.Y, 0);
            using (var blockref = new BlockReference(insertPoint, _blocktable[_blockname])) {
                blockref.Layer = _layername;
                _modelspace.AppendEntity(blockref);
                _trans.AddNewlyCreatedDBObject(blockref, true);
            }
        }

        public void CreateLines(List<(SimplePoint2d, SimplePoint2d)> endPointsList)
        {
            foreach((SimplePoint2d pt1, SimplePoint2d pt2) in endPointsList) {
                using (var line = new Line(pt1.ToPoint3d(), pt2.ToPoint3d())) {
                    line.Layer = _layername;
                    _modelspace.AppendEntity(line);
                    _trans.AddNewlyCreatedDBObject(line, true);
                }
            }
        }
    }
}
