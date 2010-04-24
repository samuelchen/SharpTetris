using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Game;

namespace Net.SamuelChen.Tetris {
    public partial class WizPageClient : UserControl, IWizardPage {
        public WizPageClient() {
            InitializeComponent();
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
            return null;
        }

        public new void Hide() {
            base.Hide();
            this.Visible = false;
        }

        #endregion
    }
}
