
//=======================================================================
// <copyright file="Block.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;

namespace Net.SamuelChen.Tetris.Blocks {
    public enum EnumBlockType {
        RoadBlock = -2,
        Wall = -1,
        Blank = 0,
        Normal = 1,
        Destory = 2,
        Bomb = 3,
    }

    /// <summary>
    /// The basic part of Tetris. 
    /// A shape is combined with several blocks.
    /// </summary>
    public class Block {

        #region Properties

        /// <summary>
        /// pos X (game cell based)
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// pos Y (game cell based)
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// the type of the block
        /// </summary>
        public EnumBlockType Type { get; set; }

        /// <summary>
        /// User data
        /// </summary>
        public object Tag { get; set; }

        #endregion

        public Block() {
            Type = EnumBlockType.Normal;
        }

        public Block(EnumBlockType type) {
            Type = type;
        }

        public Block(int x, int y)
            : this() {
            X = x;
            Y = y;
        }

        public Block(int x, int y, EnumBlockType type) {
            X = x;
            Y = y;
            Type = type;
        }

        public bool IsAlive {
            get {
                return EnumBlockType.Normal == this.Type;
            }
        }
    }
}
