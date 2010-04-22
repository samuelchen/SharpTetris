namespace Net.SamuelChen.Tetris {
    partial class NewGameWizard {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.PictureBanner = new System.Windows.Forms.PictureBox();
            this.panelPages = new System.Windows.Forms.Panel();
            this.pagePlayers = new Net.SamuelChen.Tetris.WizPageMultiPlayer();
            this.pageGameType = new Net.SamuelChen.Tetris.WizPageGameType();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBanner)).BeginInit();
            this.panelPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutMain.Controls.Add(this.PictureBanner, 0, 0);
            this.layoutMain.Controls.Add(this.panelPages, 0, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 3;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.5228F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.4772F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.layoutMain.Size = new System.Drawing.Size(487, 341);
            this.layoutMain.TabIndex = 0;
            // 
            // PictureBanner
            // 
            this.PictureBanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBanner.Location = new System.Drawing.Point(3, 3);
            this.PictureBanner.Name = "PictureBanner";
            this.PictureBanner.Size = new System.Drawing.Size(481, 101);
            this.PictureBanner.TabIndex = 2;
            this.PictureBanner.TabStop = false;
            // 
            // panelPages
            // 
            this.panelPages.Controls.Add(this.pagePlayers);
            this.panelPages.Controls.Add(this.pageGameType);
            this.panelPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPages.Location = new System.Drawing.Point(3, 110);
            this.panelPages.Name = "panelPages";
            this.panelPages.Size = new System.Drawing.Size(481, 216);
            this.panelPages.TabIndex = 3;
            // 
            // pagePlayers
            // 
            this.pagePlayers.Location = new System.Drawing.Point(48, 19);
            this.pagePlayers.Name = "pagePlayers";
            this.pagePlayers.Size = new System.Drawing.Size(400, 120);
            this.pagePlayers.TabIndex = 1;
            this.pagePlayers.Visible = false;
            // 
            // pageGameType
            // 
            this.pageGameType.Location = new System.Drawing.Point(3, 3);
            this.pageGameType.Name = "pageGameType";
            this.pageGameType.Size = new System.Drawing.Size(347, 120);
            this.pageGameType.TabIndex = 0;
            // 
            // NewGameWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutMain);
            this.Name = "NewGameWizard";
            this.Size = new System.Drawing.Size(487, 341);
            this.layoutMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBanner)).EndInit();
            this.panelPages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Panel panelPages;
        private Net.SamuelChen.Tetris.WizPageGameType pageGameType;
        private WizPageMultiPlayer pagePlayers;
        public System.Windows.Forms.PictureBox PictureBanner;
    }
}
