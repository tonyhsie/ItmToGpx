using System;
using System.IO;
using GPXWriter.Main;
using GPXWriter.Utils;
using SQLiteWrapper;
using System.Collections;

namespace ITM_To_GPX
{
    namespace Utils
    {
        class SQLiteFile
        {
            private string _filename;
            private SQLiteBase _sqlitebase;

            public SQLiteFile(string filename)
            {
                _filename = Path.GetFullPath(filename);
            }

            public void SQLiteToGPX(GPX gpxFile)
            {
                //----------------------------------------------------------------------------------------------------------------
                // 轉換
                //----------------------------------------------------------------------------------------------------------------                

                // 打開資料庫
                _sqlitebase = new SQLiteBase(_filename);

                // 取得航點資訊
                ArrayList WPInformation = new ArrayList();
                _ReadWaypointInformation(WPInformation);

                // 取得航點
                _GetWP(WPInformation, gpxFile);

                // 讀入軌跡
                ArrayList TrackInformation = new ArrayList();
                _ReadTrackInformation(TrackInformation);

                // 取得軌跡
                _GetTracks(TrackInformation, gpxFile);

                // 最後關閉資料庫
                _sqlitebase.CloseDatabase();

                //----------------------------------------------------------------------------------------------------------------
                // 轉換結束
                //----------------------------------------------------------------------------------------------------------------           
            }
            
            private void _ReadTrackInformation(ArrayList TrackArray)
            {
                // 對資料庫執行查詢
                System.Data.DataTable TmpTable = _sqlitebase.ExecuteQuery("SELECT * FROM \"Line\"");

                // 取得 Line 資料表的記錄總數
                int Counter = TmpTable.Rows.Count;

                // 讀取並儲存每筆資料記錄
                for (int i = 0; i < Counter; i++)
                {
                    // 取得資料列
                    System.Data.DataRow TmpRow = TmpTable.Rows[i];

                    // 檢查是否為軌跡
                    if ((int)TmpRow[1] == 0)
                    {
                        // 建立一個儲存物件
                        SQLiteTracks TmpTrack = new SQLiteTracks();

                        // 讀取名稱
                        TmpTrack.TrackName = (TmpRow[2]).ToString();

                        // 讀取 ID
                        TmpTrack.ID = (int)(TmpRow[0]);

                        // 讀取 FirstWP
                        TmpTrack.FirstWP = (int)TmpRow[9];

                        // 讀取 LastWP
                        TmpTrack.LastWP = (int)TmpRow[10];

                        // 加入至 ArrayList
                        TrackArray.Add(TmpTrack);
                    }
                }
            }
            
            private void _GetTracks(ArrayList TrackInformation, GPX gpxfile)
            {
                // 讀取對應的 SQLite 資料表
                System.Data.DataTable datatableWP = _sqlitebase.ExecuteQuery("SELECT * FROM \"WP\"");
                System.Data.DataTable datatableGPS = _sqlitebase.ExecuteQuery("SELECT * FROM \"GPSLog\"");

                // 針對每個軌跡執行：
                foreach (SQLiteTracks sqlitetrack in TrackInformation)
                {
                    // 建立軌跡片段
                    TrackSegment tracksegment = new TrackSegment();

                    // 宣告變數
                    double latitude, longitude, elevation, speed;
                    DateTime gpxdatetime;

                    // 走訪資料庫中所有的點
                    for (int i = sqlitetrack.FirstWP - 1; i < sqlitetrack.LastWP; i++)
                    {
                        // 取得航點資料
                        latitude = (float)datatableWP.Rows[i][9];
                        longitude = (float)datatableWP.Rows[i][8];
                        elevation = (float)datatableWP.Rows[i][10];
                        speed = (float)datatableGPS.Rows[i][4] / 3.6d;
                        gpxdatetime = DateTime.FromFileTime((((long)(int)datatableGPS.Rows[i][2]) << 32) + (uint)(int)datatableGPS.Rows[i][3]);

                        // 若為夏令時間，則扣除一小時
                        TimeZone localZone = TimeZone.CurrentTimeZone;
                        if (localZone.IsDaylightSavingTime(gpxdatetime))
                        {
                            gpxdatetime = gpxdatetime.AddHours(-1);
                        }

                        // 轉換為 UTC 時間
                        gpxdatetime = gpxdatetime.ToUniversalTime();

                        // 建立航點
                        Point pointtmp = new Point(latitude, longitude, elevation, speed, gpxdatetime);

                        // 將航點加入片段
                        tracksegment.AddPoint(pointtmp);
                    }

                    // 建立軌跡
                    Track track = new Track(sqlitetrack.TrackName);

                    // 連結軌跡、片段與 GPX
                    track.AddSegment(tracksegment);
                    gpxfile.AddTrack(track);
                }
            }
            
            private void _ReadWaypointInformation(ArrayList WaypointArray)
            {
                // 對資料庫執行查詢
                System.Data.DataTable TmpTable = _sqlitebase.ExecuteQuery("SELECT * FROM \"VP\"");

                // 取得記錄總數
                int Counter = TmpTable.Rows.Count;

                // 讀取並儲存每筆資料記錄
                for (int i = 0; i < Counter; i++)
                {
                    // 取得資料列
                    System.Data.DataRow TmpRow = TmpTable.Rows[i];

                    // 檢查是否為軌跡
                    if ((int)TmpRow[1] == 0)
                    {
                        // 建立一個儲存物件
                        SQLiteWP TmpWP = new SQLiteWP();

                        // 讀取 ID
                        TmpWP.ID = (int)TmpRow[0];

                        // 讀取 WPID
                        TmpWP.WPID = (int)TmpRow[6];

                        // 加入至 ArrayList
                        WaypointArray.Add(TmpWP);
                    }
                }
            }
            
            private void _GetWP(ArrayList WaypointInformation, GPX gpxfile)
            {
                // 讀取對應的 SQLite 資料表
                System.Data.DataTable datatableWP = _sqlitebase.ExecuteQuery("SELECT * FROM \"WP\"");
                System.Data.DataTable datatableGPS = _sqlitebase.ExecuteQuery("SELECT * FROM \"GPSLog\"");

                // 宣告變數
                double latitude, longitude, elevation, speed;
                DateTime gpxdatetime;

                // 針對每個航點執行：
                foreach (SQLiteWP sqliteWP in WaypointInformation)
                {
                    // 取得航點資料
                    latitude = (float)(datatableWP.Rows[sqliteWP.WPID - 1][9]);
                    longitude = (float)datatableWP.Rows[sqliteWP.WPID - 1][8];
                    elevation = (float)datatableWP.Rows[sqliteWP.WPID - 1][10];
                    speed = (float)datatableGPS.Rows[sqliteWP.WPID - 1][4] / 3.6d;

                    gpxdatetime = DateTime.FromFileTime((((long)(int)datatableGPS.Rows[sqliteWP.WPID - 1][2]) << 32) + (int)datatableGPS.Rows[sqliteWP.WPID - 1][3]);

                    // 若為夏令時間，則扣除一小時
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    if (localZone.IsDaylightSavingTime(gpxdatetime))
                    {
                        gpxdatetime = gpxdatetime.AddHours(-1);
                    }

                    // 轉換為 UTC 時間
                    gpxdatetime = gpxdatetime.ToUniversalTime();

                    // 建立航點
                    Point pointtmp = new Point(latitude, longitude, elevation, speed, gpxdatetime);

                    // 將航點加入片段
                    gpxfile.AddPoint(pointtmp);
                }
            }
        }

        public struct SQLiteTracks
        {
            public int ID;
            public string TrackName;
            public int FirstWP;
            public int LastWP;
        }
        
        public struct SQLiteWP
        {
            public int ID;
            public int WPID;
        }
    }
}
