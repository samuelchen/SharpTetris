
//=======================================================================
// <copyright file="WizPageMultiPlayer.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : A wizard page to define game players in local game.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Game;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Controller;
using System.Text.RegularExpressions;

namespace Net.SamuelChen.Tetris {
    public partial class WizPageMultiPlayer : UserControl, IWizardPage {

        protected Skins m_skin;
        protected Setting m_setting;

        private List<string> m_controllerIds;

        public List<Player> Players {
            get {
                List<Player> players = new List<Player>();
                for (int i = 0; i < dgvPlayers.Rows.Count; i++) {
                    Player player = new Player();
                    player.Name = dgvPlayers.Rows[i].Cells[0].Value as string;
                    if (string.IsNullOrEmpty(player.Name))
                        continue;
                    string controllerId = dgvPlayers.Rows[i].Cells[1].Value as string;
                    player.Controller = ControllerFactory.Instance.GetController(controllerId);
                    if (null == player.Controller)
                        continue;
                    player.Controller.KeyMap  = ControllerKeyMap.FromStringDictionary(m_setting.GetKeymap(controllerId));
                    players.Add(player);
                }
                return players;
            }
        }

        public WizPageMultiPlayer() {
            InitializeComponent();
            m_skin = Skins.Instance;
            m_setting = Setting.Instance;
            m_controllerIds = m_setting.ControllerIds;

            DataGridViewComboBoxColumn colController = dgvPlayers.Columns[1] as DataGridViewComboBoxColumn;
            foreach (string id in m_controllerIds) {
                colController.Items.Add(id);
            }
            dgvPlayers.Rows.Add();
            dgvPlayers.Rows[0].Cells[0].Value = m_setting.DefaultPlayerName;
            if (m_controllerIds.Contains(m_setting.DefaultPlayerControllerId))
                dgvPlayers.Rows[0].Cells[1].Value = m_setting.DefaultPlayerControllerId;

            dgvPlayers.CausesValidation = true;
            dgvPlayers.CellContentClick += new DataGridViewCellEventHandler(dgvPlayers_CellContentClick);
            dgvPlayers.Validating += new CancelEventHandler(dgvPlayers_Validating);
            
            LoadSkin();
        }

        void dgvPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (dgvPlayers.Rows[e.RowIndex].IsNewRow)
                return;
            
            if (2 == e.ColumnIndex && e.RowIndex < dgvPlayers.Rows.Count)
                dgvPlayers.Rows.RemoveAt(e.RowIndex);
            if (3 == e.ColumnIndex)
                dgvPlayers.Rows.Insert(e.RowIndex + 1, new DataGridViewRow());

        }

        void dgvPlayers_Validating(object sender, CancelEventArgs e) {
            Regex r = new Regex(@"^\w*$");
            for (int i = 0; i < dgvPlayers.Rows.Count; i++) {
                for (int j = i + 1; j < dgvPlayers.Rows.Count; j++) {
                    if (!r.IsMatch(Convert.ToString(dgvPlayers.Rows[i].Cells[0].Value))) {
                        e.Cancel = true;
                        MessageBox.Show(
                            string.Format(m_skin.GetString("err_invalid_player_name"), dgvPlayers.Rows[i].Cells[0].Value),
                            m_skin.GetString("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.Compare(Convert.ToString(dgvPlayers.Rows[i].Cells[0].Value), Convert.ToString(dgvPlayers.Rows[j].Cells[0].Value), true) == 0
                        || string.Compare(Convert.ToString(dgvPlayers.Rows[i].Cells[1].Value), Convert.ToString(dgvPlayers.Rows[j].Cells[1].Value), true) == 0) {
                        e.Cancel = true;
                        MessageBox.Show(m_skin.GetString("err_duplicate_value"), m_skin.GetString("error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

        }

        private void LoadSkin() {
            dgvPlayers.Columns[0].HeaderText = m_skin.GetString("wiz_col_player");
            dgvPlayers.Columns[1].HeaderText = m_skin.GetString("wiz_col_controller");
            dgvPlayers.Columns[2].ToolTipText = m_skin.GetString("wiz_remove_selected_player");
        }

        #region IWizardPage Members

        public bool Show(IList<object> options) {
            if (null == options || options.Count < 1)
                return false;
            EnumGameType type = (EnumGameType)options[0];
            if (type != EnumGameType.Multiple)
                return false;
            this.Dock = DockStyle.Fill;
            this.Visible = true;
            this.BringToFront();
            return true;
        }

        public object GetValue() {
            return this.Players;
        }

        public new void Hide() {
            base.Hide();
            this.Visible = false;
        }

        #endregion
    }
}
