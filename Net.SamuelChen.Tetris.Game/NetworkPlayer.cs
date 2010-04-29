using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Net.SamuelChen.Tetris.Game {
    public class NetworkPlayer : Player {

        public NetworkPlayer() :base(){
        }

        public NetworkPlayer(string name ): base(name){
        }

        /// <summary>
        /// IP address
        /// </summary>
        public IPEndPoint EndPoint { get; set; }

        public string HostName { get; set; }

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

    }
}
