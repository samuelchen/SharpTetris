
//=======================================================================
// <copyright file="WizPageGameType.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : A wizard page to select game type when starting a
//                new game.
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

namespace Net.SamuelChen.Tetris {
    public partial class WizPageGameType : UserControl, IWizardPage {

        protected Skins m_skin;

        public WizPageGameType() {
            InitializeComponent();
            m_skin = Skins.Instance;
            LoadSkin();
        }

        public EnumGameType GameType {
            get {
                if (radioSingle.Checked)
                    return EnumGameType.Single;
                else if (radioMultiple.Checked)
                    return EnumGameType.Multiple;
                else if (radioHost.Checked)
                    return EnumGameType.Host;
                else if (radioClient.Checked)
                    return EnumGameType.Client;
                else
                    return EnumGameType.Single;
            }
        }

        private void LoadSkin() {
            lblPrompt.Text = m_skin.GetString("wiz_type_prompt");
            radioSingle.Text = m_skin.GetShortCutString("wiz_type_rdo_single");
            radioMultiple.Text = m_skin.GetShortCutString("wiz_type_rdo_multiple");
            radioHost.Text = m_skin.GetShortCutString("wiz_type_rdo_host");
            radioClient.Text = m_skin.GetShortCutString("wiz_type_rdo_client");
        }

        #region IWizardPage Members

        public bool Show(IList<object> options) {
            // this page does not care the condition object.
            this.Dock = DockStyle.Fill;
            this.Visible = true;
            this.BringToFront();
            return true;
        }

        public object GetValue() {
            return (object)this.GameType;
        }

        public new void Hide() {
            base.Hide();
            this.Visible = false;
        }

        #endregion
    }
}
