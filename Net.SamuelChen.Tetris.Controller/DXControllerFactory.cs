
//=======================================================================
// <copyright file="DXControllerFactory.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To acquire DirectX controllers.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.DirectInput;

namespace Net.SamuelChen.Tetris.Controller {

    internal class DxControllerFactory : ControllerFactory {

        // GUID, IController value pairs.
        protected Dictionary<Guid, IController> m_joysticks;
        protected Dictionary<Guid, IController> m_keyboards;

        public DxControllerFactory()
            : base() {
            m_joysticks = new Dictionary<Guid, IController>();
            m_keyboards = new Dictionary<Guid, IController>();
        }

        // Enum all joystick controllers.
        public override List<IController> EnumJoysticks() {
            List<IController> controllers = new List<IController>();
            m_joysticks.Clear();
            try {
                foreach (DeviceInstance di in Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly)) {
                    IController c = new DXJoystickController(di.InstanceGuid);
                    controllers.Add(c);
                    m_joysticks.Add(di.InstanceGuid, c);
                }
            } catch (Exception err) {
                System.Diagnostics.Trace.TraceWarning(err.Message);
            }
            return controllers;
        }

        // Enum all keyboard controllers.
        public override List<IController> EnumKeyboards() {
            List<IController> controllers = new List<IController>();
            m_keyboards.Clear();
            try {
                foreach (DeviceInstance di in Manager.GetDevices(DeviceClass.Keyboard, EnumDevicesFlags.AttachedOnly)) {
                    IController c = new DXKeyboardController(di.InstanceGuid);
                    controllers.Add(c);
                    m_keyboards.Add(di.InstanceGuid, c);
                }
            } catch (Exception err) {
                System.Diagnostics.Trace.TraceWarning(err.Message);
            }
            return controllers;
        }

        public override IController GetController(string controllerId) {
            if (string.IsNullOrEmpty(controllerId))
                return null;

            IController c = null;
            Guid id = new Guid(controllerId);
            if (m_joysticks.Count == 0)
                EnumJoysticks();
            if (!m_joysticks.TryGetValue(id, out c)) {
                if (m_keyboards.Count == 0)
                    EnumKeyboards();
                m_keyboards.TryGetValue(id, out c);
            }
            return c;
        }
    }
}
