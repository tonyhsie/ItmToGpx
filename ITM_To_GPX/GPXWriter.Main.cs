using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Globalization;

namespace GPXWriter
{
    namespace Main
    {
        public class GPX
        {
            private string _Filename;
            private string _Version;
            private ArrayList _Tracks;
            private ArrayList _WayPoints;

            public string Filename
            {
                get
                {
                    return _Filename;
                }
                set
                {
                    _Filename = Path.GetFullPath(value);
                }
            }

            public string Version
            {
                get
                {
                    return _Version;
                }
            }

            public ArrayList Tracks
            {
                get
                {
                    return ArrayList.ReadOnly(_Tracks);
                }
            }

            public ArrayList WayPoints
            {
                get
                {
                    return ArrayList.ReadOnly(_WayPoints);
                }
            }

            public GPX(string Filename, string Version)
            {
                _Filename = Path.GetFullPath(Filename);
                _Version = Version;
                _Tracks = new ArrayList();
                _WayPoints = new ArrayList();
            }

            public void WriteGPX()
            {
                GPXWrite TmpWriteCache = new GPXWrite(this);
                TmpWriteCache.Write();
            }

            public void AddTrack(Utils.Track track)
            {
                _Tracks.Add(track);
            }

            public void AddPoint(Utils.Point point)
            {
                _WayPoints.Add(point);
            }
        }

        public class GPXWrite
        {
            private GPX _GPX;
            private XmlTextWriter _OutputFile;

            public GPXWrite(GPX gpx)
            {
                if (gpx == null)
                {
                    throw new Exception("GPX gpx musn't be empty");
                }

                _GPX = gpx;
            }

            private void WriteHeader()
            {
                _OutputFile.WriteStartDocument();
                _OutputFile.WriteStartElement("gpx", "http://www.topografix.com/GPX/1/1");
                _OutputFile.WriteAttributeString("version", "1.1");
                _OutputFile.WriteAttributeString("creator", $"ITMtoGPX {_GPX.Version}");
            }

            private void WriteBody()
            {
                CultureInfo ci = new CultureInfo("en-US");

                // 首先寫入航點
                foreach (Utils.Point point in _GPX.WayPoints)
                {
                    _OutputFile.WriteStartElement("wpt");
                    _OutputFile.WriteAttributeString("lat", point.latitude.ToString(ci));
                    _OutputFile.WriteAttributeString("lon", point.longitude.ToString(ci));
                    _OutputFile.WriteElementString("ele", point.elevation.ToString(ci));
                    _OutputFile.WriteElementString("time", point.time.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                    string speed = point.speed.ToString(ci);
                    if (speed != "0")
                        _OutputFile.WriteElementString("speed", speed);
                    _OutputFile.WriteEndElement();
                }

                // 接著寫入軌跡
                foreach (Utils.Track track in _GPX.Tracks)
                {
                    _OutputFile.WriteStartElement("trk");
                    _OutputFile.WriteStartElement("name");
                    _OutputFile.WriteCData(track.name);
                    _OutputFile.WriteEndElement();

                    foreach (Utils.TrackSegment tracksegment in track.TrackSegments)
                    {
                        _OutputFile.WriteStartElement("trkseg");

                        foreach (Utils.Point point in tracksegment.Points)
                        {
                            _OutputFile.WriteStartElement("trkpt");
                            _OutputFile.WriteAttributeString("lat", point.latitude.ToString(ci));
                            _OutputFile.WriteAttributeString("lon", point.longitude.ToString(ci));
                            _OutputFile.WriteElementString("ele", point.elevation.ToString(ci));
                            _OutputFile.WriteElementString("time", point.time.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                            string speed = point.speed.ToString(ci);
                            if (speed != "0")
                                _OutputFile.WriteElementString("speed", speed);
                            _OutputFile.WriteEndElement();
                        }

                        _OutputFile.WriteEndElement();
                    }

                    _OutputFile.WriteEndElement();
                }
            }

            private void WriteFooter()
            {
                _OutputFile.WriteEndDocument();
            }

            public void Write()
            {
                _OutputFile = new XmlTextWriter(_GPX.Filename, System.Text.Encoding.UTF8);
                _OutputFile.Formatting = Formatting.Indented;

                WriteHeader();
                WriteBody();
                WriteFooter();

                _OutputFile.Flush();
                _OutputFile.Close();
            }
        }
    }
}
