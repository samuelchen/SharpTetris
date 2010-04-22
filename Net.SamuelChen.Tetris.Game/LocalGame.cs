
//=======================================================================
// <copyright file="Game.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To define a game.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Controller;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Skin;

namespace Net.SamuelChen.Tetris.Game {
    /// <summary>
    /// Game playing class
    /// </summary>>
    public class LocalGame : TetrisGame {


        public LocalGame()
            : base() {
            PrivateInit();
        }

        public LocalGame(EnumGameType type)
            : base(type) {
            PrivateInit();
        }

        public LocalGame(Form container)
            : base(container) {
            PrivateInit();
        }

        #region Properties
        #endregion

        private void PrivateInit() {
            m_skin = Skins.Instance;
            m_timer = new System.Timers.Timer();
            m_timer.Elapsed += this.OnTimer_Elapsed;
        }


        #region Game process

        /// <summary>
        /// New game
        /// </summary>
        public override void New() {
            base.New();
            m_timer.Interval = 3000;
        }


        /// <summary>
        /// Start a game
        /// </summary>
        /// <param name="nLevel">start level</param>
        /// <returns></returns>
        public override void Start(int level) {
            base.Start(level);
            m_timer.Start();
            Refresh();
        }

        /// <summary>
        /// Game over
        /// </summary>
        public override void Over() {
            base.Over();
            m_timer.Stop();
        }

        /// <summary>
        /// Pause
        /// </summary>
        public override void Pause() {
            m_timer.Stop();
            base.Pause();
        }

        /// <summary>
        /// Resume
        /// </summary>
        public override void Resume() {
            base.Resume();
            m_timer.Start();
        }

        #endregion


        #region events

        protected void OnTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;

                // move sharp
                if (null != panel)
                    panel.Go();

                //TODO: score and level
            }
        }

        #endregion


        #region IDisposable Members

        public override void Dispose() {
            m_timer.Stop();
            m_timer.Dispose();
            base.Dispose();
        }

        #endregion

        #region Fields

        protected System.Timers.Timer m_timer;
        protected Skins m_skin;

        #endregion

    }
}
