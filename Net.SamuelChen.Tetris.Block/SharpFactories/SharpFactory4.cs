
//=======================================================================
// <copyright file="SharpFactory4.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


using System;

namespace Net.SamuelChen.Tetris.Blocks.SharpFactories {
    /// <summary>
    /// The factory to create 4-blocks sharp.
    /// </summary>
    internal class SharpFactory4 : SharpFactory {

        protected int m_nType = 7;  // how many types

        /// <summary>
        /// Create a sharp of given type.
        /// The index of a sharp block is zero based. It increases from left to right, from top to bottom.
        /// 1 2 3     000    123
        /// 4 5 6      0      4
        /// 7 8 9
        /// </summary>
        /// <param name="nType">The sharp type to create.</param>
        /// <returns></returns>
        public override Sharp CreateSharp(int nType) {
            switch (nType) {
                case 0:
                    return CreateSharp_01();
                case 1:
                    return CreateSharp_02();
                case 2:
                    return CreateSharp_03();
                case 3:
                    return CreateSharp_04();
                case 4:
                    return CreateSharp_05();
                case 5:
                    return CreateSharp_06();
                case 6:
                    return CreateSharp_07();
                default:
                    throw new NotImplementedException("The specified type is not implemented.");
            }
        }

        public override int TypeCount {
            get { return m_nType; }
        }


        #region CreateSharp
        /// <summary>
        /// Type 01
        /// OOOO
        /// </summary>
        /// <returns></returns>
        protected Sharp CreateSharp_01() {
            Sharp sharp = new Sharp();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x++, y);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            sharp.Blocks = blocks;
            return sharp;
        }

        /// <summary>
        /// Type 02
        /// OO
        /// OO
        /// </summary>
        /// <returns></returns>
        protected Sharp CreateSharp_02() {
            Sharp sharp = new Sharp();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x++, y);
            blocks[2] = new Block(x, y++);
            blocks[3] = new Block(x, y);
            sharp.Blocks = blocks;
            return sharp;
        }

        /// <summary>
        /// Type 03
        /// OOO
        ///   O
        /// </summary>
        /// <returns></returns>
        protected Sharp CreateSharp_03() {
            Sharp sharp = new Sharp();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x, y++);
            blocks[2] = new Block(x--, y);
            blocks[3] = new Block(x, y);
            sharp.Blocks = blocks;
            return sharp;
        }

        /// <summary>
        /// Type 04
        /// OO
        ///  OO
        /// </summary>
        /// <returns></returns>
        protected Sharp CreateSharp_04() {
            Sharp sharp = new Sharp();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x, y++);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            sharp.Blocks = blocks;
            return sharp;
        }

        /// <summary>
        /// Type 05
        /// OOO
        /// O
        /// </summary>
        /// <returns></returns>
        protected Sharp CreateSharp_05() {
            Sharp sharp = new Sharp();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x, y++);
            blocks[1] = new Block(x++, y--);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            sharp.Blocks = blocks;
            return sharp;
        }

        /// <summary>
        /// Type 06
        /// OOO
        ///  O
        /// </summary>
        /// <returns></returns>
        protected Sharp CreateSharp_06() {
            Sharp sharp = new Sharp();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x, y++);
            blocks[2] = new Block(x++, y--);
            blocks[3] = new Block(x, y);
            sharp.Blocks = blocks;
            return sharp;
        }
        /// <summary>
        /// Type 07
        ///  OO
        /// OO
        /// </summary>
        /// <returns></returns>
        protected Sharp CreateSharp_07() {
            Sharp sharp = new Sharp();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, ++y);
            blocks[1] = new Block(x, y--);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            sharp.Blocks = blocks;
            return sharp;
        }

        #endregion

    }
}
