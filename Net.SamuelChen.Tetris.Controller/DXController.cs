
//=======================================================================
// <copyright file="DirectXController.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Define a abstract DirectX controller
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;
using System.Threading;

namespace Net.SamuelChen.Tetris.Controller {
    internal abstract class DXController : Controller{

        public DXController(Guid guid) : base() {
            this.ID = guid;
            this.Device = new Device(guid);
            this.Name = this.Device.DeviceInformation.InstanceName;
            switch (this.Device.DeviceInformation.DeviceType) {
                case DeviceType.Gamepad:
                case DeviceType.Joystick:
                    this.Type = EnumControllerType.Joystick;
                    break;
                case DeviceType.Keyboard:
                    this.Type = EnumControllerType.Keyboard;
                    break;
                default:
                    throw new ControllerException("This type of controller is not supported.", null);
            }

        }

        #region properties

        /// <summary>
        /// The device of current controller.
        /// </summary>
        protected Device Device { get; private set; }

        #endregion


        #region Controller Interfaces
        public override void Attach(Control target) {
            base.Attach(target);
        }

        public override void Deattach() {
            if (null == this.Device)
                return;

            //this.Device.SetEventNotification(null);
            this.Device.Unacquire();
            this.Target = null;

            base.Deattach();
        }

        #endregion

        /// <summary>
        /// Get the state of current device.
        /// </summary>
        public abstract bool Poll();

    }
}
