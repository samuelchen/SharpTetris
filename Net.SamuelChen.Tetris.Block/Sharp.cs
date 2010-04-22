
//=======================================================================
// <copyright file="Sharp.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


using System;

namespace Net.SamuelChen.Tetris.Blocks {
    public enum EnumMoving {
        None = 0,
        Left,
        Right,
        Down,
        DirectDown,
        Rotate,
        Pause,
    }

    /// <summary>
    /// Net.SamuelChen.Tetris.Blocks.Sharp represents the moving staff on the screen in Tetris.
    /// It is the major part of the game.
    /// </summary>
    public class Sharp {

        public const int MIN_BLOCK_NUM = 2;
        public const int MAX_BLOCK_NUM = 8;
        public const int DEFAULT_BLOCK_NUM = 4;          // Default blocks quantity.

        protected Block[] m_arrBlocks;                  // The blocks array in the sharp
        protected int m_nBlockNum = DEFAULT_BLOCK_NUM;    // Quantity of the blocks in a sharp.

        /// <summary>
        /// Moved event will be triggered while a sharp moved.
        /// </summary>
        public event SharpMovingHandler Moved;

        /// <summary>
        /// ctor()
        /// </summary>
        public Sharp() {
            m_arrBlocks = new Block[m_nBlockNum];
        }

        /// <summary>
        /// ctor(nBlockNum)
        /// </summary>
        /// <param name="nBlockNum">The blocks quantity.</param>
        public Sharp(int nBlockNum) {

            if (nBlockNum > 8 || nBlockNum < 2)
                // That's too many. We uses default.
                m_nBlockNum = Sharp.DEFAULT_BLOCK_NUM;
            else
                m_nBlockNum = nBlockNum;
            m_arrBlocks = new Block[m_nBlockNum];
        }

        /// <summary>
        /// The instances of blocks in this sharp.
        /// </summary>
        public Block[] Blocks {
            get {
                return m_arrBlocks;
            }
            set {
                m_arrBlocks = value;
                m_nBlockNum = m_arrBlocks.Length;
            }
        }

        /// <summary>
        /// The sharp moves 1 cell(s). (1 block equals 1 cell)
        /// </summary>
        /// <param name="theDirection">The moving direction</param>
        public void Move(EnumMoving theDirection) {
            switch (theDirection) {
                case EnumMoving.Left:
                    for (int i = 0; i < m_nBlockNum; i++) {
                        m_arrBlocks[i].X--;
                    }
                    break;
                case EnumMoving.Right:
                    for (int i = 0; i < m_nBlockNum; i++) {
                        m_arrBlocks[i].X++;
                    }
                    break;
                case EnumMoving.Down:
                    for (int i = 0; i < m_nBlockNum; i++) {
                        m_arrBlocks[i].Y++;
                    }
                    break;
                case EnumMoving.Rotate:
                    Rotate();
                    break;
                default:
                    break;
            }

            //引发事件
            if (null != Moved)
                Moved(this, new SharpMovingEventArgs(theDirection));
        }

        /// <summary>
        /// The sharp moves n cell(s). (1 block equals 1 cell)
        /// The cells number to move is specified.
        /// </summary>
        /// <param name="theDirection">The moving direction</param>
        /// <param name="nSpeed">How many cells to move.</param>
        public void Move(EnumMoving theDirection, int nSpeed) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rotate a sharp in a clockwise direction.
        /// </summary>
        protected void Rotate() {
            ///TODO: It's not a perfect implementation. We should make it pix based.
            if (m_arrBlocks.Length <= 0)
                return;

            // Get the block at center.
            Block origin = m_arrBlocks[(int)(m_arrBlocks.Length + 1) / 2];
            int x, y;

            for (int i = 0; i < m_arrBlocks.Length; i++) {
                x = m_arrBlocks[i].Y - origin.Y + origin.X;
                y = origin.X - m_arrBlocks[i].X + origin.Y;
                m_arrBlocks[i].X = x;
                m_arrBlocks[i].Y = y;
            }

        }
    }
}
