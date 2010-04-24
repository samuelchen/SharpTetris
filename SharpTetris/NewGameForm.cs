using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Game;
using Net.SamuelChen.Tetris.Skin;

namespace Net.SamuelChen.Tetris {
    public partial class NewGameForm : Form {

        private List<IWizardPage> m_pages;
        protected Skins m_skin = Skins.Instance;

        public EnumGameType GameType { get; protected set; }
        public IList<object> Options { get; protected set; }

        public NewGameForm() {
            InitializeComponent();
            this.GameType = EnumGameType.Single;
            m_pages = new List<IWizardPage>();
            Options = new List<object>();

            LoadSkins();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            // Add pages by order
            m_pages.Add(wizardNewGame.AddPage(new WizPageGameType()));
            m_pages.Add(wizardNewGame.AddPage(new WizPageMultiPlayer()));
            m_pages.Add(wizardNewGame.AddPage(new WizPageHost()));
            m_pages.Add(wizardNewGame.AddPage(new WizPageClient()));
            m_pages.Add(wizardNewGame.AddPage(new WizPageSummary()));
            wizardNewGame.ShowFirst();
        }

        protected void LoadSkins() {
            btnPrev.Text = m_skin.GetShortCutString("wiz_btn_prev");
            btnNext.Text = m_skin.GetShortCutString("wiz_btn_next");
            btnFinish.Text = m_skin.GetShortCutString("wiz_btn_finish");
            btnCancel.Text = m_skin.GetShortCutString("wiz_btn_cancel");
        }

        protected void UpdateOptions() {
            Options.Clear();
            this.GameType = (EnumGameType)m_pages[0].GetValue();
            for (int i = 0; i < m_pages.Count; i++) {
                Options.Add(m_pages[i].GetValue());
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) {
            UpdateOptions();
            wizardNewGame.Prev(Options);
            btnNext.Enabled = wizardNewGame.CanNext();
            btnPrev.Enabled = wizardNewGame.CanPrev();
        }

        private void btnNext_Click(object sender, EventArgs e) {
            UpdateOptions();
            wizardNewGame.Next(Options);
            btnNext.Enabled = wizardNewGame.CanNext();
            btnPrev.Enabled = wizardNewGame.CanPrev();
        }

        private void btnFinish_Click(object sender, EventArgs e) {
            wizardNewGame.Finish();
            UpdateOptions();
            this.DialogResult = DialogResult.OK;
            //this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            //this.Close();
        }
    }
}
