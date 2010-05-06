using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Game;
using System.Text.RegularExpressions;
using System.Net;

namespace Net.SamuelChen.Tetris {
    public partial class WizPageClient : UserControl, IWizardPage {
        public WizPageClient() {
            InitializeComponent();

            txtName.Text = m_setting.DefaultPlayerName;
        }

        private void LoadSkins() {
            lblPrompt.Text = m_skin.GetShortCutString("wiz_client_prompt");
            lblIP.Text = m_skin.GetShortCutString("wiz_client_ip");
            lblName.Text = m_skin.GetShortCutString("wiz_client_name");
            lblPort.Text = m_skin.GetShortCutString("wiz_client_port");
            btnPaste.Text = m_skin.GetShortCutString("wiz_client_btn_paste");

        }

        protected Skins m_skin = Skins.Instance;

        #region IWizardPage Members

        public bool Show(IList<object> options) {
            if (null == options || options.Count < 1)
                return false;
            EnumGameType type = (EnumGameType)options[0];
            if (type != EnumGameType.Client)
                return false;
            this.Dock = DockStyle.Fill;
            this.Visible = true;
            this.BringToFront();
            return true;
        }

        public object GetValue() {
            string info = string.Format("name={0},server_ip={1},server_port={2}", txtName.Text, txtHostIP.Text, txtHostPort.Text);
            return info;
        }

        public new void Hide() {
            base.Hide();
            this.Visible = false;
        }

        #endregion

        private void btnPaste_Click(object sender, EventArgs e) {
            string info = Clipboard.GetText();
            txtInfo.Text = info;
            Regex r = new Regex(@"\b(((2[0-5][0-5])|([0-1]?[0-9]?[0-9]))\.){3}(((2[0-5][0-5])|([0-1]?[0-9]?[0-9]))):\d{1,5}\b");
            Regex r1 = new Regex(@"^[^\d]\w+:\d{1,5}$");
            if (r.IsMatch(info) || r1.IsMatch(info)) {
                string[] tmp = info.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                txtHostIP.Text = tmp[0];
                txtHostPort.Text = tmp[1];
            }
        }

        private void txtHostIP_Validating(object sender, CancelEventArgs e) {
            //Regex r = new Regex(@"\b((((2[0-5][0-5])|([0-1]?[0-9]?[0-9]))\.){3}(2[0-5][0-5])|([0-1]?[0-9]?[0-9]))\b");
            //Regex r1 = new Regex(@"^[^\d]\w+:\d{1,5}$");
            //if (!r.IsMatch(txtHostIP.Text) && !r1.IsMatch(txtHostIP.Text)) {
            //    txtInfo.Text = string.Format(m_skin.GetString("err_invalid_ip"), txtHostPort.Text);
            //    e.Cancel = true;
            //}
        }

        private void txtHostPort_Validating(object sender, CancelEventArgs e) {
            int port = Convert.ToInt32(txtHostPort.Text);
            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort) {
                txtInfo.Text = string.Format(m_skin.GetString("err_invalid_port"), txtHostPort.Text);
                e.Cancel = true;
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e) {
            Regex r = new Regex(@"^[^\d]\w+$");
            if (!r.IsMatch(txtName.Text))
                e.Handled = true;
        }

        private GameSetting m_setting = GameSetting.Instance;
    }
}
