
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

namespace Net.SamuelChen.Tetris.Game
{
	/// <summary>
	/// Player reprensents a game player.
	/// </summary>
	public class Player : IDisposable
	{
		public Player() {
			this.Name = "Player";
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

        /// <summary>
        /// IP address
        /// </summary>
        public System.Net.IPEndPoint IP { get; set; }

        /// <summary>
        /// Is playing local game?
        /// </summary>
        public bool IsLocal {
            get {
                return true;
            }
        }

		/// <summary>
		/// Whether is playing a local game with another player
		/// </summary>
		/// <param name="theOtherPlayer">another player</param>
		/// <returns>ture= yes, false=no</returns>
		public bool IsLocalTo(Player theOtherPlayer) {
			return true;
		}

        //public void Start() {
        //    Player player = this;
        //    player.PlayFiled.Status = EnumGameStatus.Running;
        //    player.Controller.Attach(player.PlayFiled);
        //    if (player.Controller.Attached) {
        //        player.Controller.Pressed += new ControllerPressHandler(Controller_Pressed);
        //        player.Controller.Start();
        //    }
        //}

        //public void Stop() {
        //    if (null != this.Controller)
        //        this.Controller.Stop();
        //}


        #region IDisposable Members

        private bool _disposed = false;
        public void Dispose() {
            if (_disposed)
                return;

            if (null != this.PlayFiled)
                this.PlayFiled.Dispose();
            if (null != this.Controller) {
                this.Controller.Stop();
                // don't dispose controller here.
            }
            this.IP = null;

            _disposed = true;
        }

        #endregion
    }
}
