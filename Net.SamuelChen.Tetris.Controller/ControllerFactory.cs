//=======================================================================
// <copyright file="ControllerFactory.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : An abstract factory to acquire controllers.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Controller {

    public abstract class ControllerFactory {

        private static ControllerFactory m_instance;

        /// <summary>
        /// To get a factory instance. It will create a default (DirectX) one if there is not.
        /// </summary>
        public static ControllerFactory Instance {
            get {
                if (null == m_instance)
                    CreateInstance(EnumControllerFactoryType.DirectX);
                return m_instance;
            }
            protected set {
                m_instance = value;
            }
        }

        /// <summary>
        /// To create a controller factory of given type.
        /// </summary>
        /// <param name="type">Controller type.</param>
        /// <returns>A implemented controller factory</returns>
        public static ControllerFactory CreateInstance(EnumControllerFactoryType type) {
            switch (type) {
                case EnumControllerFactoryType.DirectX:
                    m_instance = new DxControllerFactory();
                    break;
                default:
                    throw new ControllerException("This type of ControllerFactory is not implmented.", null);
            }
            return null;
        }
        
        // Enum all keyboard controllers.
        public abstract List<IController> EnumKeyboards();

        // Enum all joystick controllers.
        public abstract List<IController> EnumJoysticks();
        
        // Get a controller.
        public abstract IController GetController(string controllerId);
    }
}
