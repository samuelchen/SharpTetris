
//=======================================================================
// <copyright file="TetrisGame.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Abstract tetris game.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Net.SamuelChen.Tetris.Controller;
using System.Windows.Forms;

namespace Net.SamuelChen.Tetris.Game {
    public abstract class TetrisGame : GameBase, IDisposable {

        public TetrisGame()
            : base() {
            Level = 0;
            Type = EnumGameType.Single;
        }

        public TetrisGame(EnumGameType type, Form container) : this() {
            Type = type;
            this.Container = container;
        }

        /// <summary>
        /// Game level
        /// </summary>
        public int Level { get; protected set; }

        public Form Container { get; set; }

        /// <summary>
        /// Game type
        /// </summary>
        public EnumGameType Type { get; protected set; }


        public virtual void Refresh() {
            PlayPanel panel = null;
            int i = 0;
            foreach (Player player in this.Players.Values) {
                panel = player.PlayFiled;
                if (null != panel) {
                    panel.Show(i * (panel.Width + 20) + 20, 40);
                    //panel.InfoPanel.Show(10*(i+1) + panel.Width*i, 10);
                    //panel.Show(panel.InfoPanel.Left, panel.InfoPanel.Top + panel.InfoPanel.Height + 10);
                    i++;
                }
            }

            // adjust form size depends on last panel.
            if (null != panel) {
                this.Container.Height = panel.Height + 100;
                this.Container.Width = panel.Right + 30;
            }
        }

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

        #region IGame Members

        public override void New() {
            base.New();
            GetReady();
        }

        public void GetReady() {
            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                if (null != panel) {
                    panel.RePaint();
                }
            }
        }

        public override void Pause() {
            base.Pause();

            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                if (null != panel) {
                    panel.Status = EnumGameStatus.Paused;
                    panel.RePaint();
                }
            }
        }

        public override void Resume() {
            base.Resume();
            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                if (null != panel) {
                    panel.Status = EnumGameStatus.Running;
                    panel.RePaint();
                }
            }
        }

        public override void Over() {
            base.Over();

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

        public override void Start() {
            base.Start();
            Start(1);
        }

        public virtual void Start(int level) {
            if (level < 1)
                Level = 1;
            else
                Level = level;

            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                IController ctrlr = player.Controller;
                if (null == panel || null == ctrlr)
                    continue;

                panel.Status = EnumGameStatus.Running;
                panel.CreateNextShape();
                panel.RePaint();

                ctrlr.Pressed += new ControllerPressHandler(ctrlr_Pressed);
                ctrlr.Attach(player.PlayFiled.FindForm());
                if (ctrlr.Attached) {
                    ctrlr.Start();
                }
            }
        }

        void ctrlr_Pressed(object sender, ControllerPressedEventArgs e) {
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

        #endregion

        #region IDisposable Members

        public virtual void Dispose() {
            this.Over();

            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;
                IController ctrlr = player.Controller;
                if (null != ctrlr) {
                    try {
                        if (ctrlr.Attached)
                            ctrlr.Deattach();
                        ctrlr.Stop();
                    } catch {
                        ctrlr.Teminate();
                    }
                }
                if (null != panel) {
                    panel.Dispose();
                }
            }
        }

        #endregion
    }
}
