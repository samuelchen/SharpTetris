
//=======================================================================
// <copyright file="ShapeFactory4.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


using System;

namespace Net.SamuelChen.Tetris.Blocks {
    /// <summary>
    /// The factory to create 4-blocks shape.
    /// </summary>
    internal class ShapeFactory4 : ShapeFactory {

        protected int m_nType = 7;  // how many types

        /// <summary>
        /// Create a shape of given type.
        /// The index of a shape block is zero based. It increases from left to right, from top to bottom.
        /// 1 2 3     000    123
        /// 4 5 6      0      4
        /// 7 8 9
        /// </summary>
        /// <param name="nType">The shape type to create.</param>
        /// <returns></returns>
        public override Shape CreateShape(int nType) {
            switch (nType) {
                case 0:
                    return CreateShape_01();
                case 1:
                    return CreateShape_02();
                case 2:
                    return CreateShape_03();
                case 3:
                    return CreateShape_04();
                case 4:
                    return CreateShape_05();
                case 5:
                    return CreateShape_06();
                case 6:
                    return CreateShape_07();
                default:
                    throw new NotImplementedException("The specified type is not implemented.");
            }
        }

        public override int TypeCount {
            get { return m_nType; }
        }


        #region CreateShape
        /// <summary>
        /// Type 01
        /// OOOO
        /// </summary>
        /// <returns></returns>
        protected Shape CreateShape_01() {
            Shape shape = new Shape();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x++, y);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            shape.Blocks = blocks;
            return shape;
        }

        /// <summary>
        /// Type 02
        /// OO
        /// OO
        /// </summary>
        /// <returns></returns>
        protected Shape CreateShape_02() {
            Shape shape = new Shape();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x++, y);
            blocks[2] = new Block(x, y++);
            blocks[3] = new Block(x, y);
            shape.Blocks = blocks;
            return shape;
        }

        /// <summary>
        /// Type 03
        /// OOO
        ///   O
        /// </summary>
        /// <returns></returns>
        protected Shape CreateShape_03() {
            Shape shape = new Shape();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x, y++);
            blocks[2] = new Block(x--, y);
            blocks[3] = new Block(x, y);
            shape.Blocks = blocks;
            return shape;
        }

        /// <summary>
        /// Type 04
        /// OO
        ///  OO
        /// </summary>
        /// <returns></returns>
        protected Shape CreateShape_04() {
            Shape shape = new Shape();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x, y++);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            shape.Blocks = blocks;
            return shape;
        }

        /// <summary>
        /// Type 05
        /// OOO
        /// O
        /// </summary>
        /// <returns></returns>
        protected Shape CreateShape_05() {
            Shape shape = new Shape();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x, y++);
            blocks[1] = new Block(x++, y--);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            shape.Blocks = blocks;
            return shape;
        }

        /// <summary>
        /// Type 06
        /// OOO
        ///  O
        /// </summary>
        /// <returns></returns>
        protected Shape CreateShape_06() {
            Shape shape = new Shape();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, y);
            blocks[1] = new Block(x, y++);
            blocks[2] = new Block(x++, y--);
            blocks[3] = new Block(x, y);
            shape.Blocks = blocks;
            return shape;
        }
        /// <summary>
        /// Type 07
        ///  OO
        /// OO
        /// </summary>
        /// <returns></returns>
        protected Shape CreateShape_07() {
            Shape shape = new Shape();
            Block[] blocks = new Block[4];
            int x, y;
            x = m_nInitX;
            y = m_nInitY;
            blocks[0] = new Block(x++, ++y);
            blocks[1] = new Block(x, y--);
            blocks[2] = new Block(x++, y);
            blocks[3] = new Block(x, y);
            shape.Blocks = blocks;
            return shape;
        }

        #endregion

    }
}
