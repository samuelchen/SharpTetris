
//=======================================================================
// <copyright file="ShapeFactory.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;

namespace Net.SamuelChen.Tetris.Blocks {
    /// <summary>
    /// ShapeFactory is the factory to create Shapes.
    /// </summary>
    public abstract class ShapeFactory {

        protected static Random m_rnd = new System.Random();   // Random seed
        protected int m_nInitX = 1; // Initial X (cell based)
        protected int m_nInitY = 1; // Initial Y (cell based)

        /// <summary>
        /// Create a shape factory instance by specified block number.
        /// </summary>
        /// <param name="nBlockNum">How many blocks of a shape for the factory.</param>
        /// <returns>A shape factory instance to create N-blocks shape.</returns>
        public static ShapeFactory CreateInstance(int nBlockNum) {
            switch (nBlockNum) {
                case 4:
                    return new ShapeFactory4();
                default:
                    throw new NotImplementedException("The specified shape factory is not implemented.");
            }
        }

        /// <summary>
        /// Get a number represents how many blocks in a shape.
        /// </summary>
        /// <param name="min">the min value</param>
        /// <param name="max">the max value</param>
        /// <returns>the random block number</returns>
        public static int GetRandomBlocksNumber(int min, int max) {
            return m_rnd.Next(min, max);
        }


        /// <summary>
        /// TypeCount represents total shape types for this factory.
        /// </summary>
        public abstract int TypeCount { get; }

        /// <summary>
        /// Create a shape by given type.
        /// </summary>
        /// <param name="nType">Which type shape to create.</param>
        /// <returns>An instance of the shape.</returns>
        public abstract Shape CreateShape(int nType);

        /// <summary>
        /// Create a shape by random type.
        /// </summary>
        /// <returns>An instance of the shape.</returns>
        public Shape CreateRandomShape() {
            return this.CreateShape(m_rnd.Next(0, this.TypeCount - 1));
        }

        /// <summary>
        /// Set the initiall top-left point of a created shape.
        /// </summary>
        /// <param name="x">The left.</param>
        /// <param name="y">The top.</param>
        protected void SetInitXY(int x, int y) {
            m_nInitX = x;
            m_nInitY = y;
        }

    }
}
