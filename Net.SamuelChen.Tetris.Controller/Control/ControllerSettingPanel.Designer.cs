namespace Net.SamuelChen.Tetris.Controller {
    partial class ControllerSettingPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControllerSettingPanel));
            this.comboxDevices = new System.Windows.Forms.ComboBox();
            this.lvKeyMap = new System.Windows.Forms.ListView();
            this.colAction = new System.Windows.Forms.ColumnHeader();
            this.colPrimaryKey = new System.Windows.Forms.ColumnHeader();
            this.colSecondaryKey = new System.Windows.Forms.ColumnHeader();
            this.layoutSetting = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboxDevices
            // 
            resources.ApplyResources(this.comboxDevices, "comboxDevices");
            this.comboxDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxDevices.FormattingEnabled = true;
            this.comboxDevices.Name = "comboxDevices";
            this.comboxDevices.SelectedValueChanged += new System.EventHandler(this.comboxDevices_SelectedValueChanged);
            // 
            // lvKeyMap
            // 
            this.lvKeyMap.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvKeyMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAction,
            this.colPrimaryKey,
            this.colSecondaryKey});
            resources.ApplyResources(this.lvKeyMap, "lvKeyMap");
            this.lvKeyMap.FullRowSelect = true;
            this.lvKeyMap.GridLines = true;
            this.lvKeyMap.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvKeyMap.HideSelection = false;
            this.lvKeyMap.MultiSelect = false;
            this.lvKeyMap.Name = "lvKeyMap";
            this.lvKeyMap.UseCompatibleStateImageBehavior = false;
            this.lvKeyMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvKeyMap_MouseClick);
            // 
            // colAction
            // 
            resources.ApplyResources(this.colAction, "colAction");
            // 
            // colPrimaryKey
            // 
            resources.ApplyResources(this.colPrimaryKey, "colPrimaryKey");
            // 
            // colSecondaryKey
            // 
            resources.ApplyResources(this.colSecondaryKey, "colSecondaryKey");
            // 
            // layoutSetting
            // 
            resources.ApplyResources(this.layoutSetting, "layoutSetting");
            this.layoutSetting.Controls.Add(this.comboxDevices, 0, 0);
            this.layoutSetting.Controls.Add(this.lvKeyMap, 0, 1);
            this.layoutSetting.Name = "layoutSetting";
            // 
            // ControllerSettingPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutSetting);
            this.Name = "ControllerSettingPanel";
            this.layoutSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboxDevices;
        private System.Windows.Forms.ListView lvKeyMap;
        private System.Windows.Forms.ColumnHeader colAction;
        private System.Windows.Forms.ColumnHeader colPrimaryKey;
        private System.Windows.Forms.ColumnHeader colSecondaryKey;
        private System.Windows.Forms.TableLayoutPanel layoutSetting;
    }
}
