namespace Net.SamuelChen.Tetris {
    partial class WizPageGameType {
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
            this.radioSingle = new System.Windows.Forms.RadioButton();
            this.radioMultiple = new System.Windows.Forms.RadioButton();
            this.radioHost = new System.Windows.Forms.RadioButton();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.radioClient = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // radioSingle
            // 
            this.radioSingle.AutoSize = true;
            this.radioSingle.Checked = true;
            this.radioSingle.Location = new System.Drawing.Point(50, 40);
            this.radioSingle.Name = "radioSingle";
            this.radioSingle.Size = new System.Drawing.Size(86, 17);
            this.radioSingle.TabIndex = 0;
            this.radioSingle.TabStop = true;
            this.radioSingle.Tag = "S";
            this.radioSingle.Text = "Single Player";
            this.radioSingle.UseVisualStyleBackColor = true;
            // 
            // radioMultiple
            // 
            this.radioMultiple.AutoSize = true;
            this.radioMultiple.Location = new System.Drawing.Point(205, 40);
            this.radioMultiple.Name = "radioMultiple";
            this.radioMultiple.Size = new System.Drawing.Size(93, 17);
            this.radioMultiple.TabIndex = 1;
            this.radioMultiple.Tag = "M";
            this.radioMultiple.Text = "Multiple Player";
            this.radioMultiple.UseVisualStyleBackColor = true;
            // 
            // radioHost
            // 
            this.radioHost.AutoSize = true;
            this.radioHost.Location = new System.Drawing.Point(50, 75);
            this.radioHost.Name = "radioHost";
            this.radioHost.Size = new System.Drawing.Size(128, 17);
            this.radioHost.TabIndex = 2;
            this.radioHost.Tag = "H";
            this.radioHost.Text = "Host a network Game";
            this.radioHost.UseVisualStyleBackColor = true;
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(15, 15);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(92, 13);
            this.lblPrompt.TabIndex = 3;
            this.lblPrompt.Text = "Select game type:";
            // 
            // radioClient
            // 
            this.radioClient.AutoSize = true;
            this.radioClient.Location = new System.Drawing.Point(205, 75);
            this.radioClient.Name = "radioClient";
            this.radioClient.Size = new System.Drawing.Size(125, 17);
            this.radioClient.TabIndex = 3;
            this.radioClient.TabStop = true;
            this.radioClient.Tag = "C";
            this.radioClient.Text = "Join a network Game";
            this.radioClient.UseVisualStyleBackColor = true;
            // 
            // WizPageGameType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioClient);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.radioHost);
            this.Controls.Add(this.radioMultiple);
            this.Controls.Add(this.radioSingle);
            this.Name = "WizPageGameType";
            this.Size = new System.Drawing.Size(400, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioSingle;
        private System.Windows.Forms.RadioButton radioMultiple;
        private System.Windows.Forms.RadioButton radioHost;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.RadioButton radioClient;
    }
}
