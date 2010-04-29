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

        /// <summary>
        /// To create a controller factory of given type.
        /// </summary>
        /// <param name="type">Controller type.</param>
        /// <returns>A implemented controller factory</returns>
        public static ControllerFactory CreateInstance(EnumControllerFactoryType type) {
            ControllerFactory factory = null;
            switch (type) {
                case EnumControllerFactoryType.Virtual:
                    factory = new VirtualControllerFactory();
                    break;
                case EnumControllerFactoryType.DirectX:
                    factory = new DxControllerFactory();
                    break;
                default:
                    throw new ControllerException("This type of ControllerFactory is not implmented.", null);
            }
            return factory;
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
