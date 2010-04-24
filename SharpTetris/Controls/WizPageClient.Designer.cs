namespace Net.SamuelChen.Tetris {
    partial class WizPageClient {
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
            this.btnPaste = new System.Windows.Forms.Button();
            this.mskPort = new System.Windows.Forms.MaskedTextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.mskIP = new System.Windows.Forms.MaskedTextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(285, 130);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(75, 56);
            this.btnPaste.TabIndex = 7;
            this.btnPaste.Text = "paste from clipboard";
            this.btnPaste.UseVisualStyleBackColor = true;
            // 
            // mskPort
            // 
            this.mskPort.Location = new System.Drawing.Point(170, 67);
            this.mskPort.Name = "mskPort";
            this.mskPort.Size = new System.Drawing.Size(100, 20);
            this.mskPort.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(50, 100);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(63, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Your Name:";
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
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(50, 40);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(86, 13);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "Host IP Address:";
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(15, 15);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(39, 13);
            this.lblPrompt.TabIndex = 9;
            this.lblPrompt.Text = "prompt";
            // 
            // mskIP
            // 
            this.mskIP.Location = new System.Drawing.Point(170, 37);
            this.mskIP.Name = "mskIP";
            this.mskIP.Size = new System.Drawing.Size(100, 20);
            this.mskIP.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(170, 97);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 5;
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(53, 130);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(217, 56);
            this.txtInfo.TabIndex = 6;
            // 
            // WizPageClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.mskIP);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.mskPort);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblPrompt);
            this.Name = "WizPageClient";
            this.Size = new System.Drawing.Size(400, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.MaskedTextBox mskPort;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.MaskedTextBox mskIP;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtInfo;
    }
}
