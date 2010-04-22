namespace Net.SamuelChen.Tetris {
    partial class ControllerSetting {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControllerSetting));
            this.tpSetting = new System.Windows.Forms.TabControl();
            this.tabPlayer = new System.Windows.Forms.TabPage();
            this.tpSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpSetting
            // 
            this.tpSetting.Controls.Add(this.tabPlayer);
            resources.ApplyResources(this.tpSetting, "tpSetting");
            this.tpSetting.Name = "tpSetting";
            this.tpSetting.SelectedIndex = 0;
            // 
            // tabPlayer
            // 
            resources.ApplyResources(this.tabPlayer, "tabPlayer");
            this.tabPlayer.Name = "tabPlayer";
            this.tabPlayer.UseVisualStyleBackColor = true;
            // 
            // ControllerSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tpSetting);
            this.Name = "ControllerSetting";
            this.Resize += new System.EventHandler(this.ControllerSetting_Resize);
            this.tpSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tpSetting;
        private System.Windows.Forms.TabPage tabPlayer;

    }
}
