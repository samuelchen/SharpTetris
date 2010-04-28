
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

        /// <summary>
        /// Start to use this controller.
        /// </summary>
        public override void Start() {
            Thread t = new Thread(new ParameterizedThreadStart(DXController.ControllerProc));
            t.Name = this.Name;
            this.Working = true;
            m_thread = t;
            t.Start(this);
        }

        /// <summary>
        /// Stop using this controller.
        /// </summary>
        public override void Stop() {
            base.Stop();
            if (null != m_thread)
                m_thread.Abort();
            m_thread = null;

        }

        #endregion

        /// <summary>
        /// Force to stop using this controller.
        /// Only be invoked if Stop() does not work.
        /// </summary>
        public virtual void Teminate() {
            if (null != m_thread) {
                m_thread.Suspend();
                m_thread = null;
            }
        }

        /// <summary>
        /// Get the state of current device.
        /// </summary>
        public abstract bool Poll();

        #region IDispose
        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            this.Deattach();
            this.Stop();
            this.Teminate();
            this.Device.Dispose();
            this.Device = null;

            base.Dispose();
            _disposed = true;
        }
        #endregion

        #region Threading

        private Thread m_thread = null;                 // the thread instance

        /// <summary>
        /// The controller working thread process
        /// </summary>
        /// <param name="controller">Which controller the thread is working for.</param>
        public static void ControllerProc(object controller) {
            bool working = false;
            bool attached = false;
            int interval = 0;
            bool fired = false;
            // get the thread working state
            DXController ctrlr = controller as DXController;

            if (null == ctrlr) {
                Thread.CurrentThread.Abort();
                return;
            }

            lock (ctrlr) {
                // check the flag
                working = ctrlr.Working;
                attached = ctrlr.Attached;
                interval = ctrlr.Interval;
            }
            // capture the device
            while (working && attached) {
                Thread.Sleep(fired ? interval : 0);

                lock (ctrlr) {
                    fired = ctrlr.Poll();

                    // check the flag
                    working = ctrlr.Working;
                    attached = ctrlr.Attached;
                }
            }
            Thread.CurrentThread.Abort();
        }

        #endregion

    }
}
