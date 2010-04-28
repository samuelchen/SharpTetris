
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
        protected IDictionary<Guid, IController> m_controllers;

        public DxControllerFactory()
            : base() {
            m_joysticks = new Dictionary<Guid, IController>();
            m_keyboards = new Dictionary<Guid, IController>();
            m_controllers = new Dictionary<Guid, IController>();
        }

        /// <summary>
        /// Enum all controllers
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IController> EnumControlls() {
            //List<IController> joysticks = this.EnumControlls();
            //List<IController> keyboards = this.EnumControlls();
            //List<IController> all = new List<IController>();
            //all.AddRange(joysticks);
            //all.AddRange(keyboards);
            //return all;

            m_controllers.Clear();
            try {
                foreach (DeviceInstance di in Manager.GetDevices(DeviceClass.All, EnumDevicesFlags.AttachedOnly)) {
                    IController c = null;
                    if (di.DeviceType == DeviceType.Gamepad || di.DeviceType == DeviceType.Joystick)
                        c = new DXJoystickController(di.InstanceGuid);
                    else if (di.DeviceType == DeviceType.Keyboard)
                        c = new DXKeyboardController(di.InstanceGuid);
                    else
                        continue;

                    m_controllers.Add(di.InstanceGuid, c);
                }
            } catch (Exception err) {
                System.Diagnostics.Trace.TraceWarning(err.Message);
            }
            return m_controllers.Values;
        }
        
        /// <summary>
        /// Enum all joystick controllers.
        /// </summary>
        /// <returns></returns>
        public List<IController> EnumJoysticks() {
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

        /// <summary>
        /// Enum all keyboard controllers.
        /// </summary>
        /// <returns></returns>
        public List<IController> EnumKeyboards() {
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

            try {
                Guid id = new Guid(controllerId);
                return GetController(id);
            }catch {
                return null;
            }
        }

        public override IController GetController(Guid controllerId) {
            IController c = null;
            Guid id = controllerId;
            if (m_joysticks.Count == 0)
                EnumJoysticks();
            if (!m_joysticks.TryGetValue(id, out c)) {
                if (m_keyboards.Count == 0)
                    EnumKeyboards();
                m_keyboards.TryGetValue(id, out c);
            }
            return c;
        }

        public override IController CreateController() {
            return null;
        }

        #region IDisposable Members

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            foreach (IController c in m_controllers.Values)
                c.Dispose();
            m_controllers.Clear();

            _disposed = true;
        }

        #endregion
    }
}
