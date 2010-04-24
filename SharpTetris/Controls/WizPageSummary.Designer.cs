namespace Net.SamuelChen.Tetris {
    partial class WizPageSummary {
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
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtSummary
            // 
            this.txtSummary.AcceptsTab = true;
            this.txtSummary.BackColor = System.Drawing.SystemColors.Control;
            this.txtSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSummary.Location = new System.Drawing.Point(0, 0);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSummary.Size = new System.Drawing.Size(400, 200);
            this.txtSummary.TabIndex = 0;
            // 
            // WizPageSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSummary);
            this.Name = "WizPageSummary";
            this.Size = new System.Drawing.Size(400, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSummary;
    }
}
