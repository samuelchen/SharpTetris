
//=======================================================================
// <copyright file="WizPageHost.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : A wizard page to start hosting a remote game.
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
using System.Net;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Game;
using System.Text.RegularExpressions;

namespace Net.SamuelChen.Tetris {
    public partial class WizPageHost : UserControl, IWizardPage {
        public WizPageHost() {
            InitializeComponent();

            LoadSkins();
        }

        private void LoadSkins() {
            lblPrompt.Text = m_skin.GetShortCutString("wiz_host_prompt");
            lblName.Text = m_skin.GetShortCutString("wiz_host_name");
            lblPort.Text = m_skin.GetShortCutString("wiz_host_port");
            lblPlayers.Text = m_skin.GetShortCutString("wiz_host_max_players");
            btnCopy.Text = m_skin.GetShortCutString("wiz_host_btn_copy");
  
        }

        private void WizPageHost_Load(object sender, EventArgs e) {
            string hostname = Dns.GetHostName();
            IPHostEntry host = Dns.GetHostEntry(hostname);
            m_ip = host.AddressList[0].ToString();
            lblIP.Text = string.Format("IP: {0}", m_ip);
            txtPort.Text = m_setting.Port; // default port
            txtName.Text = m_setting.DefaultPlayerName;
            numPlayers.Minimum = 2;
            numPlayers.Maximum = m_setting.MaxPlayers;
        }

        protected Skins m_skin = Skins.Instance;
        protected GameSetting m_setting = GameSetting.Instance;
        protected string m_ip = string.Empty;

        #region IWizardPage Members

        public bool Show(IList<object> options) {
            if (null == options || options.Count < 1)
                return false;
            EnumGameType type = (EnumGameType)options[0];
            if (type != EnumGameType.Host)
                return false;
            this.Dock = DockStyle.Fill;
            this.Visible = true;
            this.BringToFront();
            return true;
        }

        public object GetValue() {
            return string.Format("name={0},ip={1},port={2},max_players={3}", txtName.Text, m_ip, txtPort.Text, numPlayers.Value);
        }

        public new void Hide() {
            base.Hide();
            this.Visible = false;
        }

        #endregion

        private void btnCopy_Click(object sender, EventArgs e) {
            string info = string.Format("{0}:{1}", m_ip, txtPort.Text);
            txtInfo.Text = string.Format(m_skin.GetShortCutString("wiz_host_info_copy"), info);
            Clipboard.SetText(info);
            
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            Regex r = new Regex(@"^[^\d]\w+$");
            if (!r.IsMatch(txtName.Text))
                e.Handled = true;
        }

        private void txtPort_Validating(object sender, CancelEventArgs e) {
            int port = Convert.ToInt32(txtPort.Text);
            if (port < 1 || port > 65535) {
                txtInfo.Text = string.Format(m_skin.GetString("err_invalid_port"), txtPort.Text);
                e.Cancel = true;
            } else {
                txtInfo.Text = string.Empty;
            }
        }

    }


}
