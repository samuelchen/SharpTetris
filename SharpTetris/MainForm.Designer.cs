namespace Net.SamuelChen.Tetris {
    partial class MainForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mnMain = new System.Windows.Forms.MenuStrip();
            this.miGame = new System.Windows.Forms.ToolStripMenuItem();
            this.miNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.miOption = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnMain
            // 
            this.mnMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGame,
            this.miSetting,
            this.miHelp});
            this.mnMain.Location = new System.Drawing.Point(0, 0);
            this.mnMain.Name = "mnMain";
            this.mnMain.Size = new System.Drawing.Size(494, 24);
            this.mnMain.TabIndex = 0;
            this.mnMain.Text = "menuStrip1";
            // 
            // miGame
            // 
            this.miGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNew,
            this.toolStripMenuItem2,
            this.miExit});
            this.miGame.Name = "miGame";
            this.miGame.Size = new System.Drawing.Size(46, 20);
            this.miGame.Text = "Game";
            // 
            // miNew
            // 
            this.miNew.Name = "miNew";
            this.miNew.Size = new System.Drawing.Size(95, 22);
            this.miNew.Text = "New";
            this.miNew.Click += new System.EventHandler(this.miNew_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(92, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(95, 22);
            this.miExit.Text = "Exit";
            // 
            // miSetting
            // 
            this.miSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOption});
            this.miSetting.Name = "miSetting";
            this.miSetting.Size = new System.Drawing.Size(53, 20);
            this.miSetting.Text = "Setting";
            // 
            // miOption
            // 
            this.miOption.Name = "miOption";
            this.miOption.Size = new System.Drawing.Size(111, 22);
            this.miOption.Text = "Options";
            this.miOption.Click += new System.EventHandler(this.miOption_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(40, 20);
            this.miHelp.Text = "Help";
            // 
            // miAbout
            // 
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(103, 22);
            this.miAbout.Text = "About";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 336);
            this.Controls.Add(this.mnMain);
            this.MainMenuStrip = this.mnMain;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.mnMain.ResumeLayout(false);
            this.mnMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnMain;
        private System.Windows.Forms.ToolStripMenuItem miGame;
        private System.Windows.Forms.ToolStripMenuItem miSetting;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miOption;
        private System.Windows.Forms.ToolStripMenuItem miNew;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}