using System;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using ITM_To_GPX.Utils;
using GPXWriter.Main;

namespace ITM_To_GPX
{
    public partial class WindowMain : Form
    {
        public WindowMain()
        {
            InitializeComponent();
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            // 建立 OpenFileDialog 執行個體並顯示一個「開啟...」對話方塊， 
            // 允許選擇多個檔案，然後將這些檔案加入至
            // ListBoxOpened 之中
            OpenFileDialog TmpFileDialog = new OpenFileDialog();
            TmpFileDialog.Multiselect = true;
            TmpFileDialog.Filter = "GPSPhototagger File (*.itm)|*.itm";

            if (TmpFileDialog.ShowDialog() == DialogResult.OK)
            {
                ListBoxOpened.BeginUpdate();

                foreach (string TmpPfad in TmpFileDialog.FileNames)
                {
                    ListBoxOpened.Items.Add(TmpPfad);
                }

                ListBoxOpened.EndUpdate();
            }
        }

        private void ButtonConvert_Click(object sender, EventArgs e)
        {
            // 建立一個供稍後使用的緩衝區
            byte[] Buffer = new Byte[4096];

            foreach (object TmpObject in ListBoxOpened.Items)
            {
                // 初始化 ZipFile 以進行解壓縮
                ZipFile TmpFile = null;

                try
                {
                    // 開啟清單中的檔案
                    TmpFile = new ZipFile(TmpObject.ToString());

                    // 走訪所有的檔案
                    foreach (ZipEntry TmpEntry in TmpFile)
                    {
                        // 直到找到名稱為 ituser.poi 的檔案
                        if (TmpEntry.IsFile && TmpEntry.Name == "ituser.poi\0")
                        {
                            // 若找到該檔案，則取得該檔案的資料流 (Stream)                           
                            Stream TmpFileStream = TmpFile.GetInputStream(TmpEntry);

                            // 結合原始 itm 檔案的路徑，建立新的檔案名稱
                            string sInputFileDir = Path.GetDirectoryName(TmpObject.ToString());
                            string TmpFullPath = Path.Combine(sInputFileDir, "ituser.poi");

                            // 並建立檔案
                            using (FileStream TmpOutput = File.Create(TmpFullPath))
                            {
                                // 接著將檔案從 zip 中複製過去，然後關閉檔案
                                StreamUtils.Copy(TmpFileStream, TmpOutput, Buffer);
                                TmpOutput.Close();

                                // 現在建立 GPX 與 SQL 物件
                                SQLiteFile sqlitefile = new SQLiteFile(TmpFullPath);
                                GPX gpxfile = new GPX(Path.Combine(sInputFileDir, Path.ChangeExtension(Path.GetFileName(TmpObject.ToString()), ".gpx")));

                                // 建立 Gpx 檔案
                                sqlitefile.SQLiteToGPX(gpxfile);

                                // 並寫入檔案
                                gpxfile.WriteGPX();

                                // 刪除 TmpOutput 暫存檔
                                File.Delete(TmpFullPath);
                            }
                        }
                    }
                }
                finally
                {
                    if (TmpFile != null)
                    {
                        TmpFile.IsStreamOwner = true;
                        TmpFile.Close();
                    }
                }
            }

            MessageBox.Show("Done!", "Converted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            int TmpSelected = ListBoxOpened.SelectedIndex;
            if (TmpSelected >= 0)
            {
                // 刪除 ListBoxOpened 中選取的物件
                ListBoxOpened.Items.Remove(ListBoxOpened.Items[TmpSelected]);

                // 如果下方還有元素，則選取該元素
                // 否則不選取任何元素
                if (ListBoxOpened.Items.Count > TmpSelected)
                {
                    ListBoxOpened.SelectedIndex = TmpSelected;
                }
            }
        }
    }
}
