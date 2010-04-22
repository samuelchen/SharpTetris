namespace Net.SamuelChen.Tetris {
    partial class OptionForm {
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
            this.tabPageOption = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabController = new System.Windows.Forms.TabPage();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnAddController = new System.Windows.Forms.Button();
            this.btnRemoveController = new System.Windows.Forms.Button();
            this.grpPlayer = new System.Windows.Forms.GroupBox();
            this.lblPlayer = new System.Windows.Forms.Label();
            this.txtPlayer = new System.Windows.Forms.TextBox();
            this.comboxController = new System.Windows.Forms.ComboBox();
            this.lblController = new System.Windows.Forms.Label();
            this.ControllerSetting = new Net.SamuelChen.Tetris.ControllerSetting();
            this.tabPageOption.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabController.SuspendLayout();
            this.grpPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageOption
            // 
            this.tabPageOption.Controls.Add(this.tabGeneral);
            this.tabPageOption.Controls.Add(this.tabController);
            this.tabPageOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabPageOption.Location = new System.Drawing.Point(0, 0);
            this.tabPageOption.Name = "tabPageOption";
            this.tabPageOption.SelectedIndex = 0;
            this.tabPageOption.Size = new System.Drawing.Size(483, 280);
            this.tabPageOption.TabIndex = 7;
            this.tabPageOption.SelectedIndexChanged += new System.EventHandler(this.tabPageOption_SelectedIndexChanged);
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.grpPlayer);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(475, 254);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "tabPage2";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tabController
            // 
            this.tabController.Controls.Add(this.ControllerSetting);
            this.tabController.Location = new System.Drawing.Point(4, 22);
            this.tabController.Name = "tabController";
            this.tabController.Padding = new System.Windows.Forms.Padding(3);
            this.tabController.Size = new System.Drawing.Size(475, 254);
            this.tabController.TabIndex = 0;
            this.tabController.Text = "tabPage1";
            this.tabController.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(413, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 29);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(344, 286);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(63, 29);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(275, 286);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(63, 29);
            this.btnApply.TabIndex = 10;
            this.btnApply.Text = "apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnAddController
            // 
            this.btnAddController.Location = new System.Drawing.Point(12, 286);
            this.btnAddController.Name = "btnAddController";
            this.btnAddController.Size = new System.Drawing.Size(89, 29);
            this.btnAddController.TabIndex = 11;
            this.btnAddController.Text = "add controller";
            this.btnAddController.UseVisualStyleBackColor = true;
            this.btnAddController.Visible = false;
            this.btnAddController.Click += new System.EventHandler(this.btnAddPlayer_Click);
            // 
            // btnRemoveController
            // 
            this.btnRemoveController.Location = new System.Drawing.Point(107, 286);
            this.btnRemoveController.Name = "btnRemoveController";
            this.btnRemoveController.Size = new System.Drawing.Size(107, 29);
            this.btnRemoveController.TabIndex = 12;
            this.btnRemoveController.Text = "remove controller";
            this.btnRemoveController.UseVisualStyleBackColor = true;
            this.btnRemoveController.Visible = false;
            this.btnRemoveController.Click += new System.EventHandler(this.btnRemovePlayer_Click);
            // 
            // grpPlayer
            // 
            this.grpPlayer.Controls.Add(this.lblController);
            this.grpPlayer.Controls.Add(this.lblPlayer);
            this.grpPlayer.Controls.Add(this.txtPlayer);
            this.grpPlayer.Controls.Add(this.comboxController);
            this.grpPlayer.Location = new System.Drawing.Point(8, 6);
            this.grpPlayer.Name = "grpPlayer";
            this.grpPlayer.Size = new System.Drawing.Size(459, 101);
            this.grpPlayer.TabIndex = 0;
            this.grpPlayer.TabStop = false;
            this.grpPlayer.Text = "groupBox1";
            // 
            // lblPlayer
            // 
            this.lblPlayer.AutoSize = true;
            this.lblPlayer.Location = new System.Drawing.Point(6, 16);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new System.Drawing.Size(64, 13);
            this.lblPlayer.TabIndex = 3;
            this.lblPlayer.Text = "player name";
            // 
            // txtPlayer
            // 
            this.txtPlayer.Location = new System.Drawing.Point(82, 13);
            this.txtPlayer.Name = "txtPlayer";
            this.txtPlayer.Size = new System.Drawing.Size(100, 20);
            this.txtPlayer.TabIndex = 4;
            // 
            // comboxController
            // 
            this.comboxController.FormattingEnabled = true;
            this.comboxController.Location = new System.Drawing.Point(263, 13);
            this.comboxController.Name = "comboxController";
            this.comboxController.Size = new System.Drawing.Size(121, 21);
            this.comboxController.TabIndex = 5;
            // 
            // lblController
            // 
            this.lblController.AutoSize = true;
            this.lblController.Location = new System.Drawing.Point(201, 16);
            this.lblController.Name = "lblController";
            this.lblController.Size = new System.Drawing.Size(50, 13);
            this.lblController.TabIndex = 6;
            this.lblController.Text = "controller";
            // 
            // ControllerSetting
            // 
            this.ControllerSetting.Actions = null;
            this.ControllerSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControllerSetting.Location = new System.Drawing.Point(3, 3);
            this.ControllerSetting.Name = "ControllerSetting";
            this.ControllerSetting.Size = new System.Drawing.Size(469, 248);
            this.ControllerSetting.TabIndex = 6;
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 321);
            this.Controls.Add(this.btnRemoveController);
            this.Controls.Add(this.btnAddController);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabPageOption);
            this.Name = "OptionForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.OptionForm_Load);
            this.Resize += new System.EventHandler(this.OptionForm_Resize);
            this.tabPageOption.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabController.ResumeLayout(false);
            this.grpPlayer.ResumeLayout(false);
            this.grpPlayer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Net.SamuelChen.Tetris.ControllerSetting ControllerSetting;
        private System.Windows.Forms.TabControl tabPageOption;
        private System.Windows.Forms.TabPage tabController;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnAddController;
        private System.Windows.Forms.Button btnRemoveController;
        private System.Windows.Forms.GroupBox grpPlayer;
        private System.Windows.Forms.Label lblPlayer;
        private System.Windows.Forms.TextBox txtPlayer;
        private System.Windows.Forms.ComboBox comboxController;
        private System.Windows.Forms.Label lblController;
    }
}

