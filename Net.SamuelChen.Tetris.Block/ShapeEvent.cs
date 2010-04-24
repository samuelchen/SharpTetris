
//=======================================================================
// <copyright file="ShapeEvent.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


using System;

namespace Net.SamuelChen.Tetris.Blocks {

    // The delegate for Shape moving
    public delegate void ShapeMovingHandler(object sender, ShapeMovingEventArgs e);

    /// <summary>
    /// ShapeEventArgs reprets an argument for the events while a Shape moving.
    /// </summary>
    public class ShapeMovingEventArgs {

        private EnumMoving m_Direction;	// moving direction

        public ShapeMovingEventArgs() {
        }

        public ShapeMovingEventArgs(EnumMoving direction) {
            m_Direction = direction;
        }

        /// <summary>
        /// Direction property prepresents the moving direction while a Shape moving.
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
