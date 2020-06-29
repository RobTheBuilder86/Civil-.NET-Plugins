using CreatePolyFromAlignment.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CreatePolyFromAlignment.Engine.HelperObjects
{
    public class StationComputer
    {
        private readonly IAlignment Alignment;
        private readonly List<StationSegment> AlignmentSegments;
        public int AlignmentSegmentIndex { get; private set; }
        private StationSegment CurrentAlignmentSegment => 
            AlignmentSegments[AlignmentSegmentIndex];
        private double AlignmentStartStation => AlignmentSegments.First().StartStation;
        private double AlignmentEndStation => AlignmentSegments.Last().EndStation;

        private readonly IProfile Profile;
        private readonly List<StationSegment> ProfileSegments;
        public int ProfileSegmentIndex { get; private set; }
        private StationSegment CurrentProfileSegment => 
            ProfileSegments[ProfileSegmentIndex];
        private double ProfileStartStation => ProfileSegments.First().StartStation;
        private double ProfileEndStation => ProfileSegments.Last().EndStation;

        private double _minStartStation;
        private double _maxEndStation;
        private double _startStation;
        private double _endStation;
        public double EndStation {
            get { return _endStation; }
            set {
                if (value <= _startStation) {
                    throw new ArgumentException("Value of end station must be higher than start station.");
                } else if (value > _maxEndStation ) {
                    throw new ArgumentException("Value of end station cannot exceed end station of either the selected alignment or the attached profile.");
                }
                _endStation = value;
            }
        }
        public double StartStation {
            get { return _startStation; }
            set {
                if (value >= _endStation) {
                    throw new ArgumentException("Value of start station must be lesser than end station.");
                } else if (value < _minStartStation) {
                    throw new ArgumentException("Value of start station cannot be less than the start station of either the selected alignment or the attached profile.");
                }
                _startStation = value;
            }
        }
        public double CurrentlyComputedUntilStation { get; private set; }
        public double MaxIncrement {
            get { return _maxIncrement; }
            set {
                if (value <= 0) {
                    throw new ArgumentException("Increment must have positive, nonzero value.");
                }
                _maxIncrement = value;
            } 
        }
        private double _maxIncrement = 1;


        private List<double> StationsList;
        public List<double> Stations {
            get {
                if (StationsList == null) {
                    ComputeStationsList();
                }
                return StationsList;
            }
        }

        public StationComputer(IAlignment alignment, IProfile profile)
        {
            (this.Profile, this.Alignment) = (profile, alignment);
            ProfileSegments = Profile.GetSegments();
            AlignmentSegments = Alignment.GetSegments();
            GetStartAndEndStations();
            ValidateStationSegments();
        }

        private void GetStartAndEndStations()
        {
            _minStartStation = Math.Max(AlignmentStartStation,
                                        ProfileStartStation);
            _maxEndStation = Math.Min(AlignmentEndStation,
                                      ProfileEndStation);
            _startStation = _minStartStation;
            _endStation = _maxEndStation;
        }

        private void ValidateStationSegments()
        {
            CheckForCoherence(AlignmentSegments, "Alignment");
            CheckForCoherence(ProfileSegments, "Profile");
            CheckForOverlap();
        }

        private void CheckForOverlap()
        {
            if ( _minStartStation > _maxEndStation ) {
                throw new ArgumentException("Station range of alignment and profile do not overlap.");
            }
        }

        private void CheckForCoherence(List<StationSegment> segments, string objectName)
        {
            double lastSegmentEndStation = segments.First().StartStation;
            int counter = 1;
            foreach(StationSegment segment in segments) {
                if (segment.StartStation != lastSegmentEndStation) {
                    throw new ArgumentException(
                        $"Segments of {objectName} are incoherent: " +
                        $"StartStation of segment #{counter} does not equal " +
                        $"EndStation of segment #{counter-1}.");
                }
                lastSegmentEndStation = segment.EndStation;
                counter++;
            }
        }

        private void ComputeStationsList()
        {
            InitializeSegmentIndizes();
            StationsList = new List<double>();
            StationsList.Add(_startStation);
            CurrentlyComputedUntilStation = _startStation;
            while (CurrentlyComputedUntilStation < _endStation) {
                ComputeStationsOfCurrentSegmentPair();
            }
        }

        private void InitializeSegmentIndizes()
        {
            AlignmentSegmentIndex = 0;
            ProfileSegmentIndex = 0;
            while (!CurrentAlignmentSegment.IsInside(_startStation)) {
                AlignmentSegmentIndex++;
            }
            while (!CurrentProfileSegment.IsInside(_startStation)) {
                ProfileSegmentIndex++;
            }
        }

        private void ComputeStationsOfCurrentSegmentPair()
        {
            double start = CurrentlyComputedUntilStation;
            SegmentType currentSegmentPairType = GetCurrentSegmentPairType();
            CurrentlyComputedUntilStation = GetEndOfCurrentSegmentPair();
            if (currentSegmentPairType == SegmentType.straight) {
                Stations.Add(CurrentlyComputedUntilStation);
            } else {
                ComputeStationsOfNonStraightSegmentPair(start, CurrentlyComputedUntilStation);
            }
        }

        private double GetEndOfCurrentSegmentPair()
        {
            double end = 0;
            if (CurrentAlignmentSegment.EndStation == CurrentProfileSegment.EndStation) {
                end = CurrentAlignmentSegment.EndStation;
                AlignmentSegmentIndex++;
                ProfileSegmentIndex++;
            } else if (CurrentAlignmentSegment.EndStation < CurrentProfileSegment.EndStation) {
                end = CurrentAlignmentSegment.EndStation;
                AlignmentSegmentIndex++;
            } else if (CurrentAlignmentSegment.EndStation > CurrentProfileSegment.EndStation) {
                end = CurrentProfileSegment.EndStation;
                ProfileSegmentIndex++;
            } else {
                throw new Exception("Something went wrong while getting end of current segment pair.");
            }
            if (_endStation < end) {
                end = _endStation;
            }
            return end;
        }

        private SegmentType GetCurrentSegmentPairType()
        {
            if (CurrentAlignmentSegment.Type != SegmentType.straight ||
                CurrentProfileSegment.Type != SegmentType.straight) {
                return SegmentType.nonstraight;
            }
            return SegmentType.straight;
        }

        private void ComputeStationsOfNonStraightSegmentPair(double start, double end)
        {
            double steps = Math.Ceiling((end - start - 0.0000001) / _maxIncrement);
            // -0.0000001 is added to make up for double imprecision which could lead to 
            // Math.Ceiling return a value which is 1 above expected value.
            double increment = (end-start) / steps;
            for(int i = 1; i <= steps; i++) {
                double currentStation = start + i * increment;
                Stations.Add(currentStation);
            }
        }
    }
}