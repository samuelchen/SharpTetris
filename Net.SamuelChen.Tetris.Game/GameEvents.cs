using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Game {
    public class GameEventArgs : EventArgs {
    }

    public class PlayerEventArgs : EventArgs {
        public Player Player { get; set; }
        public PlayerEventArgs() : base() { }
        public PlayerEventArgs(Player player)
            : this() {
            this.Player = player;
        }
    }
}
