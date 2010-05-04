
//=======================================================================
// <copyright file="Game.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To define a game.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System.Windows.Forms;
using Net.SamuelChen.Tetris.Controller;
using System.Collections.Generic;

namespace Net.SamuelChen.Tetris.Game {
    /// <summary>
    /// Game playing class
    /// </summary>>
    public class LocalGame : TetrisGame {


        public LocalGame()
            : base() {
            PrivateInit();
        }

        public LocalGame(EnumGameType type, Form container)
            : base(type) {
            this.Container = container;
            PrivateInit();
        }

        #region Properties

        public Form Container { get; set; }

        #endregion

        private void PrivateInit() {
            m_timer = new System.Timers.Timer();
            m_timer.Elapsed += this.OnTimer_Elapsed;
        }

        //public override void Refresh() {
        //    base.Refresh();

        //    PlayPanel panel = null;
        //    InfoPanel info = null;
        //    int i = 0;
        //    foreach (Player player in this.Players.Values) {
        //        info = player.InfoPanel;
        //        if (null != info) {
        //            info.Show(i * (info.Width + 20) + 20, 40);
        //        }
                
        //        panel = player.PlayFiled;
        //        if (null != panel) {
        //            panel.Show(i * (panel.Width + 20) + 20, info.Bottom + 20);
        //        }

        //        i++;
        //    }

        //    // adjust form size depends on last panel.
        //    if (null != panel) {
        //        this.Container.Height = panel.Bottom + 60;
        //        this.Container.Width = panel.Right + 30;
        //    }
        //}

        public override void AddPlayer(Player player) {
            base.AddPlayer(player);
            this.Container.Controls.Add(player.PlayFiled);
        }

        public override void RemovePlayer(Player player) {
            base.RemovePlayer(player);
            this.Container.Controls.Remove(player.PlayFiled);
        }

        public Player GetPlayer(IController c) {
            foreach (KeyValuePair<string, Player> item in this.Players) {
                Player player = item.Value;
                if (player.Controller == c)
                    return player;
            }
            return null;
        }

        #region Game process

        /// <summary>
        /// New game
        /// </summary>
        public override void New() {
            base.New();
            //this.GetReady();
            if (null != m_timer)
                m_timer.Interval = 3000;
        }


        //public void GetReady() {
        //    foreach (Player player in this.Players.Values) {
        //        PlayPanel panel = player.PlayFiled;
        //        if (null != panel) {
        //            panel.RePaint();
        //        }
        //    }
        //}

        /// <summary>
        /// Start a game
        /// </summary>
        /// <param name="nLevel">start level</param>
        /// <returns></returns>
        public override void Start(int level) {
            base.Start(level);

            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                IController ctrlr = player.Controller;
                if (null == panel || null == ctrlr)
                    continue;

                panel.Status = EnumGameStatus.Running;
                panel.CreateNextShape();
                panel.RePaint();

                ctrlr.Pressed += new ControllerPressHandler(OnController_Pressed);
                ctrlr.Attach(player.PlayFiled.FindForm());
                if (ctrlr.Attached) {
                    ctrlr.Start();
                }
            }
            if (null != m_timer)
                m_timer.Start();
        }

        /// <summary>
        /// Game over
        /// </summary>
        public override void Stop() {
            base.Stop();
            if (null != m_timer)
                m_timer.Stop();

            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                if (null != panel) {
                    panel.Status = EnumGameStatus.Over;
                    panel.RePaint();
                }

                IController ctrlr = player.Controller;
                if (null != ctrlr) {
                    if (ctrlr.Attached)
                        ctrlr.Deattach();
                    ctrlr.Stop();
                }
            }
        }

        /// <summary>
        /// Pause
        /// </summary>
        public override void Pause() {
            if (null != m_timer)
                m_timer.Stop();
            base.Pause();
            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                if (null != panel) {
                    panel.Status = EnumGameStatus.Paused;
                    panel.RePaint();
                }
            }
        }

        /// <summary>
        /// Resume
        /// </summary>
        public override void Resume() {
            base.Resume();
            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                if (null != panel) {
                    panel.Status = EnumGameStatus.Running;
                    panel.RePaint();
                }
            }

            if (null != m_timer)
                m_timer.Start();

        }

        #endregion


        #region events

        protected virtual void OnController_Pressed(object sender, ControllerPressedEventArgs e) {
            if (this.Status != EnumGameStatus.Running)
                return;

            IController c = sender as IController;
            string action = string.Empty;
            Player player;
            if (null != e.Keys && e.Keys.Count > 0) {
                lock (c) {
                    action = c.Translate(e.Keys[0]);
                    player = GetPlayer(c);
                }

                lock (player) {
                    object act = null;
                    action = action.ToUpper();
                    if (ActionMapping.TryGetValue(action, out act))
                        player.PlayFiled.Go(act);
#if DEBUG
                    player.PlayFiled.DebugString = e.sDebug;
                    player.PlayFiled.RePaint();
#endif
                }
            }
        }

        protected void OnTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;

                // move shape
                if (null != panel)
                    panel.Go();

                //TODO: score and level
            }
        }

        #endregion


        #region IDisposable Members
        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            if (null != m_timer) {
                m_timer.Stop();
                m_timer.Dispose();
            }
            base.Dispose();
            _disposed = true;
        }

        #endregion

        #region Fields

        protected System.Timers.Timer m_timer;

        #endregion

    }
}
