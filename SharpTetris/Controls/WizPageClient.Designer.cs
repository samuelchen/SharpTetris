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
            this.txtHostPort = new System.Windows.Forms.MaskedTextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.txtHostIP = new System.Windows.Forms.MaskedTextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(285, 130);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(75, 56);
            this.btnPaste.TabIndex = 3;
            this.btnPaste.Text = "paste from clipboard";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // txtHostPort
            // 
            this.txtHostPort.Location = new System.Drawing.Point(170, 97);
            this.txtHostPort.Mask = "00000";
            this.txtHostPort.Name = "txtHostPort";
            this.txtHostPort.Size = new System.Drawing.Size(100, 20);
            this.txtHostPort.TabIndex = 2;
            this.txtHostPort.Validating += new System.ComponentModel.CancelEventHandler(this.txtHostPort_Validating);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(50, 40);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(63, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Your Name:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(50, 100);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(72, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Network Port:";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(50, 70);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(88, 13);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "Host Name or IP:";
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
            // txtHostIP
            // 
            this.txtHostIP.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.txtHostIP.Location = new System.Drawing.Point(170, 67);
            this.txtHostIP.Name = "txtHostIP";
            this.txtHostIP.Size = new System.Drawing.Size(100, 20);
            this.txtHostIP.TabIndex = 1;
            this.txtHostIP.Validating += new System.ComponentModel.CancelEventHandler(this.txtHostIP_Validating);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(170, 37);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 0;
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(53, 130);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.Size = new System.Drawing.Size(217, 56);
            this.txtInfo.TabIndex = 6;
            // 
            // WizPageClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtHostIP);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.txtHostPort);
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
        private System.Windows.Forms.MaskedTextBox txtHostPort;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.MaskedTextBox txtHostIP;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtInfo;
    }
}
