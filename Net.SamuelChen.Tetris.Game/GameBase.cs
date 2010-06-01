
//=======================================================================
// <copyright file="GameBase.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Abstract Game
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Game {
    public abstract class GameBase : IGame {

        public static IDictionary<string, object> ActionMapping
            = new Dictionary<string, object>();

        public GameBase()
            : base() {
            Players = new Dictionary<string, Player>();
        }

        #region IGame Members

        public EnumGameStatus Status { get; protected set; }

        public object Tag { get; set; }

        #region player management

        /// <summary>
        /// Players in a game.
        /// </summary>
        public IDictionary<string, Player> Players { get; set; }

        /// <summary>
        /// Add a player to game
        /// </summary>
        public virtual void AddPlayer(Player player) {
            if (null == player)
                return;

            Players.Add(player.Name, player);
        }

        public void RemovePlayer(Player player) {
            if (null == player || string.IsNullOrEmpty(player.Name))
                return;
            this.RemovePlayer(player.Name);
        }

        /// <summary>
        /// remove a player.
        /// </summary>
        /// <param name="name">player name</param>
        /// <returns>The removed player instance.</returns>
        public virtual Player RemovePlayer(string name) {
            Player player = null;
            if (Players.TryGetValue(name, out player))
                return null;

            Players.Remove(name);
            return player;
        }

        /// <summary>
        /// Change a player name.
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public bool ChangePlayerName(string oldName, string newName) {
            Debug.Assert(null != Players);
            Debug.Assert(!oldName.Equals(newName, StringComparison.InvariantCultureIgnoreCase));
            Debug.Assert(!Players.Keys.Contains(newName));

            if (null == Players)
                return false;

            if (oldName.Equals(newName, StringComparison.InvariantCultureIgnoreCase))
                return false;

            if (Players.Keys.Contains(newName))
                return false;

            Player player = null;
            if (Players.TryGetValue(oldName, out player)) {
                player.Name = newName;
                Players.Remove(oldName);
                Players.Add(newName, player);
            }
            return true;
        }

        #endregion

        public virtual void New() {
            Status = EnumGameStatus.Ready;
        }

        public virtual void Pause() {
            Status = EnumGameStatus.Paused;
        }

        public virtual void Resume() {
            Status = EnumGameStatus.Running;
        }

        public virtual void Stop() {
            Status = EnumGameStatus.Over;
        }

        public virtual void Start() {
            Status = EnumGameStatus.Running;
        }

        #region IDisposable

        private bool _disposed = false;
        public virtual void Dispose() {
            if (_disposed)
                return;

            this.Stop();
            foreach (Player player in this.Players.Values)
                player.Dispose();
            this.Players.Clear();
            this.Players = null;

            _disposed = true;
        }

        #endregion

        #endregion
    }
}
