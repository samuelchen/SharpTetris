
//=======================================================================
// <copyright file="JoystickController.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using Microsoft.DirectX.DirectInput;
using System.Windows.Forms;

namespace Net.SamuelChen.Tetris.Controller {
    internal class DXJoystickController : DXController {

        public DXJoystickController(Guid guid)
            : base(guid) {

        }

        #region DirectXController Interfaces

        public override bool Poll() {

            lock (this) {
                if (!this.Attached)
                    return false;
            }

            Device dev = this.Device;
            JoystickState state;
            try {
                dev.Poll();
                state = dev.CurrentJoystickState;
            } catch (NotAcquiredException) {
                System.Diagnostics.Trace.TraceWarning("Device {0} ({1}) is not acquired.\n", this.Name, this.ID);
                return false;
            }

            List<ControllerKey> keys = new List<ControllerKey>();

            byte[] btns = state.GetButtons();
            if (null != btns && btns.Length > 0) {
                for (int i = 0; i < btns.Length; i++) {
                    if (btns[i] > 0)
                        keys.Add(new ControllerKey(i));
                }
            }

            if (0 == state.X)
                keys.Add(new ControllerKey(101)); // Axis X left
            else if (65535 == state.X)
                keys.Add(new ControllerKey(102)); // Axis X right

            if (0 == state.Y)
                keys.Add(new ControllerKey(103)); // Axis Y up
            else if (65535 == state.Y)
                keys.Add(new ControllerKey(104)); // Axis Y down
  
            if (keys.Count > 0) {
                ControllerPressedEventArgs e = new ControllerPressedEventArgs(keys.ToArray());
#if DEBUG
                int axisRz = state.Rz;
                int axisRx = state.Rx;
                int axisX = state.X;
                int axisY = state.Y;

                e.sDebug = string.Format("Rz:{0}, Rx:{2}, X{2}, Y{3}",
                    axisRz, axisRx, axisX, axisY);
#endif
                this.Press(e);
                return true;
            }
            

            return false;
        }

        #endregion

        #region Controller Interfaces

        public override void Attach(Control target) {
            if (null == this.Device)
                return;
            this.Target = target;
            Device dev = this.Device;
            try {
                // Acquire the device
                dev.SetCooperativeLevel(target,
                    CooperativeLevelFlags.Background |
                    CooperativeLevelFlags.NonExclusive);
                dev.SetDataFormat(DeviceDataFormat.Joystick);
                //dev.SetActionMap();
                //dev.SetEventNotification();
                dev.Acquire();

                // Find the capabilities of the joystick
                DeviceCaps cps = dev.Caps;
                this.ButtonCount = cps.NumberButtons;
            } catch (Microsoft.DirectX.DirectInput.InputException err) {
                throw new ControllerException("You must attach to a TOP level control.", err);
            } catch (Exception err) {
                throw new ControllerException("Exception occurs while attaching.", err);
            }

            base.Attach(target);
        }

        #endregion

    }
}
