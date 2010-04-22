using System;
using Net.SamuelChen.Tetris.Blocks.SharpFactories;

namespace Net.SamuelChen.Tetris.Blocks {
    /// <summary>
    /// SharpFactory is the factory to create Sharps.
    /// </summary>
    public abstract class SharpFactory {

        protected static Random m_rnd = new System.Random();   // Random seed
        protected int m_nInitX = 1; // Initial X (cell based)
        protected int m_nInitY = 1; // Initial Y (cell based)

        /// <summary>
        /// Create a sharp factory instance by specified block number.
        /// </summary>
        /// <param name="nBlockNum">How many blocks of a sharp for the factory.</param>
        /// <returns>A sharp factory instance to create N-blocks sharp.</returns>
        public static SharpFactory CreateInstance(int nBlockNum) {
            switch (nBlockNum) {
                case 4:
                    return new SharpFactory4();
                default:
                    throw new NotImplementedException("The specified sharp factory is not implemented.");
            }
        }

        /// <summary>
        /// Get a number represents how many blocks in a sharp.
        /// </summary>
        /// <param name="min">the min value</param>
        /// <param name="max">the max value</param>
        /// <returns>the random block number</returns>
        public static int GetRandomSharpNumber(int min, int max) {
            return m_rnd.Next(min, max);
        }


        /// <summary>
        /// TypeCount represents total sharp types for this factory.
        /// </summary>
        public abstract int TypeCount { get; }

        /// <summary>
        /// Create a sharp by given type.
        /// </summary>
        /// <param name="nType">Which type sharp to create.</param>
        /// <returns>An instance of the sharp.</returns>
        public abstract Sharp CreateSharp(int nType);

        /// <summary>
        /// Create a sharp by random type.
        /// </summary>
        /// <returns>An instance of the sharp.</returns>
        public Sharp CreateRandomSharp() {
            return this.CreateSharp(m_rnd.Next(0, this.TypeCount - 1));
        }

        /// <summary>
        /// Set the initiall top-left point of a created sharp.
        /// </summary>
        /// <param name="x">The left.</param>
        /// <param name="y">The top.</param>
        protected void SetInitXY(int x, int y) {
            m_nInitX = x;
            m_nInitY = y;
        }

    }
}
