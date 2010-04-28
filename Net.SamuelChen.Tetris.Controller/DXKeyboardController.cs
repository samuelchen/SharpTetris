
//=======================================================================
// <copyright file="KeyboardController.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using Microsoft.DirectX.DirectInput;

namespace Net.SamuelChen.Tetris.Controller {
    internal class DXKeyboardController : DXController {

        public DXKeyboardController(Guid guid)
            : base(guid) {
        }

        #region DirectXController Interfaces

        public override bool Poll() {
            Device dev = this.Device;
            Key[] devKeys = null;
            try {
                dev.Poll();
                devKeys = dev.GetPressedKeys();
            } catch (NotAcquiredException) {
                System.Diagnostics.Trace.TraceWarning("Device {0} ({1}) is not acquired.\n", this.Name, this.ID);
                return false;
            }

            List<ControllerKey> keys = new List<ControllerKey>();
            for (int i = 0; i < devKeys.Length; i++) {
                if (devKeys[i] > 0)
                    keys.Add(new ControllerKey((int)devKeys[i]));
            }

            if (keys.Count > 0) {
                this.Press(new ControllerPressedEventArgs(keys.ToArray()));
                return true;
            }

            return false;
        }

        #endregion

        #region Controller Interfaces

        public override void Attach(System.Windows.Forms.Control target) {
            if (null == this.Device)
                return;
            
            Device dev = this.Device;

            // Acquire the device
            dev.SetCooperativeLevel(target,
                CooperativeLevelFlags.Background |
                CooperativeLevelFlags.NonExclusive);
            dev.SetDataFormat(DeviceDataFormat.Keyboard);
            //dev.SetActionMap();
            //dev.SetEventNotification();
            dev.Acquire();

            // Find the capabilities of the joystick
            DeviceCaps cps = dev.Caps;
            this.ButtonCount = cps.NumberButtons;

            base.Attach(target);
        }

        #endregion
    }
}
