namespace Net.SamuelChen.Tetris {
    partial class WizPageMultiPlayer {
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
            this.lblPlayer = new System.Windows.Forms.Label();
            this.lblController = new System.Windows.Forms.Label();
            this.dgvPlayers = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.colPlayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colController = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colRemove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colAdd = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlayers)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPlayer
            // 
            this.lblPlayer.AutoSize = true;
            this.lblPlayer.Location = new System.Drawing.Point(15, 15);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new System.Drawing.Size(68, 13);
            this.lblPlayer.TabIndex = 0;
            this.lblPlayer.Text = "Player name:";
            // 
            // lblController
            // 
            this.lblController.AutoSize = true;
            this.lblController.Location = new System.Drawing.Point(170, 15);
            this.lblController.Name = "lblController";
            this.lblController.Size = new System.Drawing.Size(51, 13);
            this.lblController.TabIndex = 1;
            this.lblController.Text = "Controller";
            // 
            // dgvPlayers
            // 
            this.dgvPlayers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlayers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvPlayers.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvPlayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPlayers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPlayer,
            this.colController,
            this.colRemove,
            this.colAdd});
            this.dgvPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPlayers.Location = new System.Drawing.Point(12, 3);
            this.dgvPlayers.MultiSelect = false;
            this.dgvPlayers.Name = "dgvPlayers";
            this.dgvPlayers.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPlayers.RowHeadersVisible = false;
            this.dgvPlayers.ShowCellErrors = false;
            this.dgvPlayers.ShowRowErrors = false;
            this.dgvPlayers.Size = new System.Drawing.Size(385, 114);
            this.dgvPlayers.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 97.75F));
            this.tableLayoutPanel1.Controls.Add(this.dgvPlayers, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 120);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // colPlayer
            // 
            this.colPlayer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPlayer.FillWeight = 162.2434F;
            this.colPlayer.HeaderText = "player name";
            this.colPlayer.Name = "colPlayer";
            // 
            // colController
            // 
            this.colController.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colController.FillWeight = 162.2434F;
            this.colController.HeaderText = "controller";
            this.colController.Name = "colController";
            // 
            // colRemove
            // 
            this.colRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colRemove.HeaderText = "-";
            this.colRemove.Name = "colRemove";
            this.colRemove.ReadOnly = true;
            this.colRemove.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colRemove.Text = "-";
            this.colRemove.UseColumnTextForButtonValue = true;
            this.colRemove.Width = 20;
            // 
            // colAdd
            // 
            this.colAdd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colAdd.HeaderText = "+";
            this.colAdd.Name = "colAdd";
            this.colAdd.ReadOnly = true;
            this.colAdd.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAdd.Text = "+";
            this.colAdd.UseColumnTextForButtonValue = true;
            this.colAdd.Width = 20;
            // 
            // WizPageMultiPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblController);
            this.Controls.Add(this.lblPlayer);
            this.Name = "WizPageMultiPlayer";
            this.Size = new System.Drawing.Size(400, 120);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlayers)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlayer;
        private System.Windows.Forms.Label lblController;
        private System.Windows.Forms.DataGridView dgvPlayers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlayer;
        private System.Windows.Forms.DataGridViewComboBoxColumn colController;
        private System.Windows.Forms.DataGridViewButtonColumn colRemove;
        private System.Windows.Forms.DataGridViewButtonColumn colAdd;
    }
}
