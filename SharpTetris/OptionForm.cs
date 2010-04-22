
//=======================================================================
// <copyright file="OptionForm.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : A dialog to set options.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Controller;
using Microsoft.DirectX.DirectInput;
using System.Diagnostics;
using System.Threading;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Game;

namespace Net.SamuelChen.Tetris {
    public partial class OptionForm : Form {
        #region Fields

        protected Setting m_setting = Setting.Instance;
        protected Skins m_skin = Skins.Instance;
        private int m_autoIdPlayer = 1;
        private int m_autoIdController = 1;
        #endregion

        public OptionForm() {
            InitializeComponent();
            LoadSkins();
        }

        #region events

        private void OptionForm_Load(object sender, EventArgs e) {
            LoadSettings();
        }

        private void tabPageOption_SelectedIndexChanged(object sender, EventArgs e) {
            btnRemoveController.Visible = btnAddController.Visible = (tabPageOption.SelectedTab == tabController);

            if (tabPageOption.SelectedTab == tabGeneral) {
                LoadPlayers();
            } else if (tabPageOption.SelectedTab == tabController) {

            }
        }

        private void btnAddPlayer_Click(object sender, EventArgs e) {
            ControllerSetting.AddController(CreateControllerName());
        }

        private void btnRemovePlayer_Click(object sender, EventArgs e) {
            ControllerSetting.RemoveController();
        }

        private void btnApply_Click(object sender, EventArgs e) {
            SaveSettings();
#if DEBUG
            m_setting.Save(@"C:\Documents and Settings\Samuel\My Documents\Visual Studio 2008\Projects\Tetris.Net\SharpTetris\setting.xml");
#else
            m_setting.Save();
#endif
        }

        private void btnOK_Click(object sender, EventArgs e) {
            SaveSettings();
#if DEBUG
            m_setting.Save(@"C:\Documents and Settings\Samuel\My Documents\Visual Studio 2008\Projects\Tetris.Net\SharpTetris\setting.xml");
#else
            m_setting.Save();
#endif
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void OptionForm_Resize(object sender, EventArgs e) {
            int h = btnApply.Height, w = btnApply.Width;
            if (this.Height < 300)
                this.Height = 300;
            if (this.Width < 400)
                this.Width = 400;
            btnApply.Top = btnOK.Top = btnCancel.Top = this.Height - h - 40;
            btnCancel.Left = this.Width - w - 30;
            btnOK.Left = btnCancel.Left - w - 10;
            btnApply.Left = btnOK.Left - w - 10;
            tabPageOption.Height = btnApply.Top - 10;

            btnRemoveController.Top = btnAddController.Top = btnApply.Top;
            btnRemoveController.Left = btnAddController.Right + 10;
        }

        #endregion

        #region private methods

        protected void LoadSettings() {
            LoadControllers();
            LoadPlayers();
        }

        protected void SaveSettings() {
            SaveControllers();
            SavePlayers();
        }

        private void LoadSkins() {
            this.Text = m_skin.GetString("op_title");
            btnApply.Text = m_skin.GetShortCutString("op_btn_apply");
            btnOK.Text = m_skin.GetShortCutString("op_btn_ok");
            btnCancel.Text = m_skin.GetShortCutString("op_btn_cancel");

            // general setting
            tabGeneral.Text = m_skin.GetString("op_tab_general");

            // controller setting
            tabController.Text = m_skin.GetString("op_tab_controller");
            btnAddController.Text = m_skin.GetShortCutString("op_btn_addcontroller");
            btnRemoveController.Text = m_skin.GetShortCutString("op_btn_removecontroller");
        }

        private void LoadControllers() {
            ControllerSetting.Actions = new List<string>(TetrisGame.ActionMapping.Keys);
            if (m_setting.ControllerIds.Count > 0) {
                ControllerSetting.Clear();
                foreach (string controllerId in m_setting.ControllerIds) {
                    Dictionary<string, string> keymap = m_setting.GetKeymap(controllerId);
                    string name = CreateControllerName();
                    ControllerSetting.AddController(name);
                    ControllerSetting.SetKeyMap(name, controllerId, ControllerKeyMap.FromStringDictionary(keymap));
                }
            } else {
                ControllerSetting.AddControllers(new string[] { CreateControllerName() });
            }
        }

        private void SaveControllers() {
            List<string> players = ControllerSetting.Controllers;
            if (null == players)
                return;

            m_setting.ClearKeymaps();
            for (int i = 0; i < players.Count; i++) {
                string controllerId;
                ControllerKeyMap keymap = ControllerSetting.GetKeyMap(players[i], out controllerId);
                m_setting.SetKeymap(controllerId, keymap.ToStringDictionary());
            }
        }

        private void LoadPlayers() {
            txtPlayer.Text = m_setting.DefaultPlayerName;

            List<string> controllerNames = ControllerSetting.Controllers;
            if (null == controllerNames || 0 == controllerNames.Count)
                return;

            string curName = comboxController.SelectedText;
            if (string.IsNullOrEmpty(curName)) {
                string id = m_setting.DefaultPlayerControllerId;
                foreach (string name in controllerNames){
                    if (ControllerSetting.GetControllerId(name) == id) {
                        curName = name;
                        break;
                    }
                }
            }
                
            comboxController.Items.Clear();
            foreach (string name in controllerNames) {
                comboxController.Items.Add(name);
            }
            comboxController.SelectedItem = curName;
        }

        private void SavePlayers() {
            m_setting.DefaultPlayerName = txtPlayer.Text;
            if (null != comboxController.SelectedItem) {
                string id = ControllerSetting.GetControllerId(comboxController.SelectedItem.ToString());
                m_setting.DefaultPlayerControllerId = id;
            }
            
        }

        private string CreateControllerName() {
            string str = m_skin.GetString("controller");
            return string.Format("{0}{1}", str, m_autoIdController++);
        }

        private string CreatePlayerName() {
            string str = m_skin.GetString("player");
            return string.Format("{0}{1}", str, m_autoIdPlayer++);
        }

        #endregion

    }
}
