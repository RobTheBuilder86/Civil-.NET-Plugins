using CreatePolyFromAlignment.Engine.HelperObjects;
using CreatePolyFromAlignment.Engine.Interfaces;

namespace CreatePolyFromAlignment.Engine
{
    public class PolyFitter
    {
        private IAlignment _alignment;
        private IProfile _profile;
        private IPolyline _polyline;
        private StationComputer _computer;

        public double StartStation{
            get { return _computer.StartStation; }
            set { _computer.StartStation = value; }
        }
        public double EndStation { 
            get { return _computer.EndStation; }
            set { _computer.EndStation = value; }
        }
        public double MaxIncrement { 
            get { return _computer.MaxIncrement; }
            set { _computer.MaxIncrement = value; }
        }

        public PolyFitter(IAlignment alignment, IProfile profile, IPolyline polyline) {
            (_alignment, _profile, _polyline) = (alignment, profile, polyline);
            _computer = new StationComputer(_alignment, _profile);
        }

        public void FitPoly() {
            foreach (double station in _computer.Stations) {
                double x, y, z;
                (x, y) = _alignment.GetCoordsAt(station);
                z = _profile.GetElevationAt(station);
                _polyline.AddVertexAt(x, y, z);
            }
        }
    }
}
