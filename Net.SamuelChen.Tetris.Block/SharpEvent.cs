
//=======================================================================
// <copyright file="SharpEvent.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


using System;

namespace Net.SamuelChen.Tetris.Blocks {

    // The delegate for Sharp moving
    public delegate void SharpMovingHandler(object sender, SharpMovingEventArgs e);

    /// <summary>
    /// SharpEventArgs reprets an argument for the events while a Sharp moving.
    /// </summary>
    public class SharpMovingEventArgs {

        private EnumMoving m_Direction;	// moving direction

        public SharpMovingEventArgs() {
        }

        public SharpMovingEventArgs(EnumMoving direction) {
            m_Direction = direction;
        }

        /// <summary>
        /// Direction property prepresents the moving direction while a Sharp moving.
        /// </summary>
        public EnumMoving Direction {
            get {
                return m_Direction;
            }
            set {
                m_Direction = value;
            }
        }
    }
}
