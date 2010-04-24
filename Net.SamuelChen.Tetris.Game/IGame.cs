
//=======================================================================
// <copyright file="IGame.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Interface to define a game
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
namespace Net.SamuelChen.Tetris.Game {
    public interface IGame {
        IDictionary<string, Player> Players { get; }
        EnumGameStatus Status { get; }
        object Tag { get; set; }
        void AddPlayer(Player player);
        void RemovePlayer(Player player);

        void New();
        void Start();
        void Pause();
        void Resume();
        void Over();
    }
}
