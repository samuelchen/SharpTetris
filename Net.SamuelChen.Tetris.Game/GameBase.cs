
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
using Net.SamuelChen.Tetris.Rule;

namespace Net.SamuelChen.Tetris.Game {
    public abstract class GameBase : IGame {

        public EventHandler GameElapsed;

        public static IDictionary<string, object> ActionMapping
            = new Dictionary<string, object>();

        public GameBase()
            : base() {
            Players = new Dictionary<string, Player>();
            Commands = new Dictionary<string, ICommand>();
        }

        #region IGame Members

        public EnumGameStatus Status { get; protected set; }

        public IDictionary<string, ICommand> Commands { get; protected set; }

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
        /// Change a player name of host.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        protected bool ChangePlayerName(string hostName, string newName) {
            Debug.Assert(!string.IsNullOrEmpty(hostName));
            Debug.Assert(!string.IsNullOrEmpty(newName));
            Debug.Assert(null != Players);

            if (string.IsNullOrEmpty(hostName) || string.IsNullOrEmpty(newName) 
                || null == this.Players || 0 == this.Players.Count)
                return false;

            Player player = this.GetPlayerByHostName(hostName);
            if (null == player)
                return false;
            if (player.Name.Equals(newName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            Player tmpPlayer = null;
            if (this.Players.TryGetValue(newName, out tmpPlayer))
                newName = string.Format("{0}@{1}", newName, player.HostName);

            this.Players.Remove(player.Name);
            player.Name = newName;
            this.Players.Add(player.Name, player);

            return true;
        }


        protected Player GetPlayerByHostName(string hostName) {
            Debug.Assert(null != this.Players);
            if (null == this.Players)
                return null;
            foreach (Player player in this.Players.Values) {
                if (hostName.Equals(player.HostName, StringComparison.CurrentCultureIgnoreCase))
                    return player;
            }
            return null;
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
