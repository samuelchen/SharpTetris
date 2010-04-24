
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
using Net.SamuelChen.Tetris.Service;

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
            lblPlayers.Text = m_skin.GetShortCutString("wiz_host_player_number");
            btnCopy.Text = m_skin.GetShortCutString("wiz_host_btn_copy");
  
        }

        private void WizPageHost_Load(object sender, EventArgs e) {
            string hostname = Dns.GetHostName();
            IPHostEntry host = Dns.GetHostEntry(hostname);
            StringBuilder sb = new StringBuilder();
            foreach (IPAddress ip in host.AddressList)
                sb.AppendLine(ip.ToString());
            
            m_ip = sb.ToString();
            txtPort.Text = m_port = m_setting.Port; // default port
            txtName.Text = m_setting.DefaultPlayerName;
        }

        protected Skins m_skin = Skins.Instance;
        protected Setting m_setting = Setting.Instance;
        protected string m_ip = string.Empty;
        protected string m_port = string.Empty;

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
            return m_ip;
        }

        public new void Hide() {
            base.Hide();
            this.Visible = false;
        }

        #endregion
    }
}
