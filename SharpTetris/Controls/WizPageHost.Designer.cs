namespace Net.SamuelChen.Tetris {
    partial class WizPageHost {
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
            this.lblPrompt = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblPlayers = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.MaskedTextBox();
            this.numPlayers = new System.Windows.Forms.NumericUpDown();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtInfo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPlayers)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(15, 15);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(39, 13);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "prompt";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(50, 40);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(69, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Game Name:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(50, 70);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(72, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Network Port:";
            // 
            // lblPlayers
            // 
            this.lblPlayers.AutoSize = true;
            this.lblPlayers.Location = new System.Drawing.Point(50, 100);
            this.lblPlayers.Name = "lblPlayers";
            this.lblPlayers.Size = new System.Drawing.Size(84, 13);
            this.lblPlayers.TabIndex = 3;
            this.lblPlayers.Text = "Player Numbers:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(170, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 4;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(170, 67);
            this.txtPort.Mask = "0000";
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(100, 20);
            this.txtPort.TabIndex = 5;
            // 
            // numPlayers
            // 
            this.numPlayers.Location = new System.Drawing.Point(170, 98);
            this.numPlayers.Name = "numPlayers";
            this.numPlayers.Size = new System.Drawing.Size(100, 20);
            this.numPlayers.TabIndex = 6;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(285, 130);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 56);
            this.btnCopy.TabIndex = 8;
            this.btnCopy.Text = "copy to clipboard";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(53, 130);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(217, 56);
            this.txtInfo.TabIndex = 9;
            // 
            // WizPageHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.numPlayers);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblPlayers);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblPrompt);
            this.Name = "WizPageHost";
            this.Size = new System.Drawing.Size(400, 200);
            this.Load += new System.EventHandler(this.WizPageHost_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPlayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblPlayers;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.MaskedTextBox txtPort;
        private System.Windows.Forms.NumericUpDown numPlayers;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtInfo;
    }
}
