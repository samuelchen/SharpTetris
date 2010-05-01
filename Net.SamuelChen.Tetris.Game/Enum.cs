﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Game {
    public enum EnumGameStatus {
        None = 0,
        Ready,
        Running,
        Paused,
        Defeated,
        Over,
    }

    public enum EnumGameType {
        Single = 0,
        Multiple,
        Host,
        Client,
    }

    //public enum EnumMoving  {
    //    Empty = 0, //default
    //    Left,
    //    Right,
    //    Down,
    //    Rotate,
    //    DownDirectly,
    //}
}
