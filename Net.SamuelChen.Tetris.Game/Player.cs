
//=======================================================================
// <copyright file="Player.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To define a player
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using Net.SamuelChen.Tetris.Controller;
using System.Net;

namespace Net.SamuelChen.Tetris.Game
{
	/// <summary>
	/// Player reprensents a game player.
	/// </summary>
	public class Player : IDisposable
	{
        private static int m_autoId = 1;

		public Player() {
			this.Name = Player.CreateName();
		}
		
		public Player(string sName) {
			this.Name = sName;
		}

		/// <summary>
		/// Player name.
		/// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User data.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Controller belongs to current player
        /// </summary>
        public IController Controller { get; set; }

        /// <summary>
        /// the place which player plays on.
        /// </summary>
        public PlayPanel PlayFiled { get; set; }

        public InfoPanel InfoPanel { get; set; }

        /// <summary>
        /// IP address for network game
        /// </summary>
        public IPEndPoint EndPoint { get; set; }

        /// <summary>
        /// Server name for network game
        /// </summary>
        public string HostName { get; set; }

        public static string CreateName() {
            return string.Format("Player{0}", m_autoId++);
        }

        #region IDisposable Members

        private bool _disposed = false;
        public virtual void Dispose() {
            if (_disposed)
                return;

            if (null != this.PlayFiled) {
                this.PlayFiled.Dispose();
                this.PlayFiled = null;
            }
            if (null != this.InfoPanel) {
                this.InfoPanel.Dispose();
                this.InfoPanel = null;
            }
            if (null != this.Controller) {
                this.Controller.Stop();
                this.Controller = null;
                // don't dispose controller here.
            }

            _disposed = true;
        }

        #endregion
    }
}
