
//=======================================================================
// <copyright file="MainForm.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : The main window of the game.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Game;

namespace Net.SamuelChen.Tetris {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            LoadSkins();
        }

        #region events

        private void MainForm_Load(object sender, EventArgs e) {

        }

        private void miNew_Click(object sender, EventArgs e) {

            if (null != m_game)
                m_game.Pause();

            NewGameForm wizardNewGame = new NewGameForm();
            if (wizardNewGame.ShowDialog(this) == DialogResult.OK) {
                IList<object> options = wizardNewGame.Options;
                EnumGameType type = (EnumGameType)options[0];
                List<Player> players = options[1] as List<Player>;

                TetrisGame game = GameFactory.CreateGame(type) as TetrisGame;
                game.Container = this;
                foreach (Player player in players) {
                    player.PlayFiled = new PlayPanel();
                    game.AddPlayer(player);
                }

                wizardNewGame.Close();

                if (null != m_game) 
                    m_game.Dispose();

                m_game = game;
                game.New();
                game.Refresh();
                game.Start();
            }
        }

        private void miOption_Click(object sender, EventArgs e) {
            OptionForm dlg = new OptionForm();
            DialogResult result = dlg.ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (null != m_game)
                m_game.Dispose();
        }

        private void OnAllMenu_Click(object sender, EventArgs e) {
            if (null != m_game)
                m_game.Pause();
        }

        #endregion

        #region private methods

        private void LoadSkins() {
            this.Text = m_skin.GetString("app_name");
            miGame.Text = m_skin.GetShortCutString("mn_game");
            miNew.Text = m_skin.GetShortCutString("mn_new");
            miExit.Text = m_skin.GetShortCutString("mn_exit");
            miSetting.Text = m_skin.GetShortCutString("mn_setting");
            miHelp.Text = m_skin.GetShortCutString("mn_help");
            miAbout.Text = m_skin.GetShortCutString("mn_about");
            if (miHelp.Text.Length <= 0)
                miHelp.Text = "&Help";
            if (miAbout.Text.Length <= 0)
                miAbout.Text = "&About";
        }

        #endregion

        #region fields

        protected Setting m_setting = Setting.Instance;
        protected Skins m_skin = Skins.Instance;
        private TetrisGame m_game = null;

        #endregion

    }
}