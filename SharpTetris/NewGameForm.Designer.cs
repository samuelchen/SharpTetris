namespace Net.SamuelChen.Tetris {
    partial class NewGameForm {
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
            this.layout = new System.Windows.Forms.TableLayoutPanel();
            this.layoutButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.wizardNewGame = new Net.SamuelChen.Tetris.NewGameWizard();
            this.layout.SuspendLayout();
            this.layoutButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.ColumnCount = 1;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layout.Controls.Add(this.layoutButtons, 0, 1);
            this.layout.Controls.Add(this.wizardNewGame, 0, 0);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Name = "layout";
            this.layout.RowCount = 2;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.96561F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.03438F));
            this.layout.Size = new System.Drawing.Size(498, 349);
            this.layout.TabIndex = 1;
            // 
            // layoutButtons
            // 
            this.layoutButtons.Controls.Add(this.btnPrev);
            this.layoutButtons.Controls.Add(this.btnNext);
            this.layoutButtons.Controls.Add(this.btnFinish);
            this.layoutButtons.Controls.Add(this.btnCancel);
            this.layoutButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.layoutButtons.Location = new System.Drawing.Point(161, 309);
            this.layoutButtons.Name = "layoutButtons";
            this.layoutButtons.Size = new System.Drawing.Size(334, 37);
            this.layoutButtons.TabIndex = 3;
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(3, 3);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 28);
            this.btnPrev.TabIndex = 3;
            this.btnPrev.Text = "Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(84, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 28);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(165, 3);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 28);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(246, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // wizardNewGame
            // 
            this.wizardNewGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardNewGame.Location = new System.Drawing.Point(3, 3);
            this.wizardNewGame.Name = "wizardNewGame";
            this.wizardNewGame.Size = new System.Drawing.Size(492, 300);
            this.wizardNewGame.TabIndex = 0;
            // 
            // NewGameForm
            // 
            this.AcceptButton = this.btnFinish;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(498, 349);
            this.ControlBox = false;
            this.Controls.Add(this.layout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewGameForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.layout.ResumeLayout(false);
            this.layoutButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NewGameWizard wizardNewGame;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.FlowLayoutPanel layoutButtons;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnCancel;

    }
}