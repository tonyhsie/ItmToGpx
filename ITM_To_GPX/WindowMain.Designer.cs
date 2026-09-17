namespace ITM_To_GPX
{
    partial class WindowMain
    {
        /// <summary>
        /// 必要的設計工具變數
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 設計工具支援的必要方法
        /// 請勿使用程式碼編輯器修改這個方法的內容
        /// </summary>
        private void InitializeComponent()
        {
            this.ListBoxOpened = new System.Windows.Forms.ListBox();
            this.ButtonOpen = new System.Windows.Forms.Button();
            this.ButtonConvert = new System.Windows.Forms.Button();
            this.ButtonDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ListBoxOpened
            // 
            this.ListBoxOpened.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBoxOpened.FormattingEnabled = true;
            this.ListBoxOpened.ItemHeight = 12;
            this.ListBoxOpened.Location = new System.Drawing.Point(12, 11);
            this.ListBoxOpened.Name = "ListBoxOpened";
            this.ListBoxOpened.Size = new System.Drawing.Size(221, 220);
            this.ListBoxOpened.TabIndex = 0;
            // 
            // ButtonOpen
            // 
            this.ButtonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOpen.Location = new System.Drawing.Point(239, 11);
            this.ButtonOpen.Name = "ButtonOpen";
            this.ButtonOpen.Size = new System.Drawing.Size(75, 22);
            this.ButtonOpen.TabIndex = 1;
            this.ButtonOpen.Text = "Open File";
            this.ButtonOpen.UseVisualStyleBackColor = true;
            this.ButtonOpen.Click += new System.EventHandler(this.ButtonOpen_Click);
            // 
            // ButtonConvert
            // 
            this.ButtonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonConvert.Location = new System.Drawing.Point(239, 66);
            this.ButtonConvert.Name = "ButtonConvert";
            this.ButtonConvert.Size = new System.Drawing.Size(75, 22);
            this.ButtonConvert.TabIndex = 2;
            this.ButtonConvert.Text = "Convert";
            this.ButtonConvert.UseVisualStyleBackColor = true;
            this.ButtonConvert.Click += new System.EventHandler(this.ButtonConvert_Click);
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonDelete.Location = new System.Drawing.Point(239, 39);
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(75, 22);
            this.ButtonDelete.TabIndex = 3;
            this.ButtonDelete.Text = "Delete";
            this.ButtonDelete.UseVisualStyleBackColor = true;
            this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // WindowMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 242);
            this.Controls.Add(this.ButtonDelete);
            this.Controls.Add(this.ButtonConvert);
            this.Controls.Add(this.ButtonOpen);
            this.Controls.Add(this.ListBoxOpened);
            this.Name = "WindowMain";
            this.Text = "ITM To GPX   (v260917)";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxOpened;
        private System.Windows.Forms.Button ButtonOpen;
        private System.Windows.Forms.Button ButtonConvert;
        private System.Windows.Forms.Button ButtonDelete;
    }
}
