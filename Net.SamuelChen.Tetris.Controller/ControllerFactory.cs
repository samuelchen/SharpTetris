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

    public abstract class ControllerFactory :  IDisposable {

        private static ControllerFactory m_instance;

        /// <summary>
        /// To get a factory instance. It will create a default (DirectX) one if there is not.
        /// </summary>
        public static ControllerFactory Instance {
            get {
                if (null == m_instance)
                    CreateInstance(EnumControllerFactoryType.Virtual);
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
                case EnumControllerFactoryType.Virtual:
                    m_instance = new VirtualControllerFactory();
                    break;
                case EnumControllerFactoryType.DirectX:
                    m_instance = new DxControllerFactory();
                    break;
                default:
                    throw new ControllerException("This type of ControllerFactory is not implmented.", null);
            }
            return null;
        }

        /// <summary>
        /// Enum all controllers
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IController> EnumControlls();
        
        //// Enum all keyboard controllers.
        //public abstract List<IController> EnumKeyboards();

        //// Enum all joystick controllers.
        //public abstract List<IController> EnumJoysticks();
        
        /// <summary>
        /// Get a controller by GUID string
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public abstract IController GetController(string controllerId);

        /// <summary>
        /// get a controller by GUID
        /// </summary>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public abstract IController GetController(Guid controllerId);

        /// <summary>
        /// Create a virtual controller. Only availible for VirtualControllerFactory (Type is EnumControllerFactoryType.Virtual)
        /// Returns null if other type of factories.
        /// </summary>
        /// <returns></returns>
        public abstract IController CreateController();


        #region IDisposable Members

        public abstract void Dispose();

        #endregion
    }
}
