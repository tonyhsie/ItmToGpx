using System;
using System.Collections;

namespace GPXWriter
{
    namespace Utils
    {
        public class Point
        {
            private double _latitude;
            private double _longitude;
            private double _elevation;
            private double _speed;
            private DateTime _time;

            public Point(double langitude, double longitude, double elevation, double speed, DateTime time)
            {
                _latitude = langitude;
                _longitude = longitude;
                _elevation = elevation;
                _speed = speed;
                _time = time;
            }

            public double latitude
            {
                get
                {
                    return _latitude;
                }
                set
                {
                    _latitude = latitude;
                }
            }

            public double longitude
            {
                get
                {
                    return _longitude;
                }
                set
                {
                    _longitude = longitude;
                }
            }

            public double elevation
            {
                get
                {
                    return _elevation;
                }
                set
                {
                    _elevation = elevation;
                }
            }

            public double speed
            {
                get
                {
                    return _speed;
                }
                set
                {
                    _speed = speed;
                }
            }

            public DateTime time
            {
                get
                {
                    return _time;
                }
            }
        }

        public class Track
        {
            /// <summary>
            /// 儲存軌跡片段
            /// </summary>
            
            private string _Name;
            private ArrayList _TrackSegments;

            public Track(string sName)
            {
                _Name = sName;
                _TrackSegments = new ArrayList();
            }

            public string name
            {
                get
                {
                    return _Name;
                }
            }

            public ArrayList TrackSegments
            {
                get
                {
                    return ArrayList.ReadOnly(_TrackSegments);
                }
                set
                {
                    throw new Exception("ArrayList musn't be set. Use Add etc. to manipulate ArrayList");
                }
            }

            public void AddSegment(TrackSegment add)
            {
                _TrackSegments.Add(add);
            }
        }
        
        public class TrackSegment
        {
            /// <summary>
            /// 儲存 _TrackSegments 的航點
            /// </summary>
            private ArrayList _Points;

            public TrackSegment()
            {
                _Points = new ArrayList();
            }

            public ArrayList Points
            {
                get
                {
                    return ArrayList.ReadOnly(_Points);
                }
                set
                {
                    throw new Exception("ArrayList musn't be set. Use Add etc. to manipulate ArrayList");
                }
            }

            public void AddPoint(Point add)
            {
                _Points.Add(add);
            }
        }
    }
}
