
//=======================================================================
// <copyright file="Controller.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Net.SamuelChen.Tetris.Controller {

    /// <summary>
    /// Controller represents a game controller.
    /// </summary>
    public abstract class Controller : IController, IDisposable {

        /// <summary>
        /// Pressed fires when a key (keys) pressed.
        /// </summary>
        public event ControllerPressHandler Pressed;

        /// <summary>
        /// ctor().
        /// </summary>
        public Controller() {
            this.KeyMap = new ControllerKeyMap();
            Interval = 120; //default 
        }

        #region Properties

        /// <summary>
        /// The controller instance GUID.
        /// </summary>
        public Guid ID { get; protected set; }

        /// <summary>
        /// The controller instance name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The controller type.
        /// </summary>
        public EnumControllerType Type { get; protected set; }

        /// <summary>
        /// The button count.
        /// </summary>
        public int ButtonCount { get; protected set; }

        /// <summary>
        /// The target play panel.
        /// </summary>
        public object Target { get; protected set; }

        /// <summary>
        /// The map of key-action pairs.
        /// </summary>
        public ControllerKeyMap KeyMap { get; set; }

        /// <summary>
        /// The gathering interval (ms).
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// the controller started working
        /// </summary>
        public bool Working { get; protected set; }

        /// <summary>
        /// the controller is attached
        /// </summary>
        public bool Attached { get; protected set; }
        #endregion

        /// <summary>
        /// To trigger the Pressed event.
        /// </summary>
        /// <param name="e"></param>
        public void Press(ControllerPressedEventArgs e) {
            if (Pressed != null) {
                //lock (Pressed) {
                    Pressed(this, e);
                //}
            }
        }

        #region IController Interfaces

        /// <summary>
        /// Attach to a specified play panel.
        /// </summary>
        /// <param name="target">The target play panel which the controller is attaching to.</param>
        public virtual void Attach(Control target) {
            this.Attached = true;
        }
        public void Attach(object target) {
            this.Attach(target as Control);
        }

        /// <summary>
        /// Deattach from a attached play panel.
        /// </summary>
        public virtual void Deattach() {
            this.Attached = false;
        }

        /// <summary>
        /// Start to use this controller.
        /// </summary>
        public void Start() {
            Thread t = new Thread(new ParameterizedThreadStart(Controller.ControllerProc));
            t.Name = this.Name;
            this.Working = true;
            m_thread = t;
            t.Start(this);
        }

        /// <summary>
        /// Stop using this controller.
        /// </summary>
        public void Stop() {
            lock (this) {
                this.Working = false;
            }
        }

        /// <summary>
        /// Force to stop using this controller.
        /// Only be invoked if Stop() does not work.
        /// </summary>
        public void Teminate() {
            if (null != m_thread) {
                m_thread.Suspend();
            }
        }

        public ControllerKey[] Translate(string action) {
            if (null == this.KeyMap)
                return null;
            ControllerKey[] keys = null;

            this.KeyMap.TryGetValue(action, out keys);
            return keys;

        }

        public string Translate(ControllerKey key) {
            if (null == this.KeyMap)
                return null;

            string action = string.Empty;
            foreach (KeyValuePair<string, ControllerKey[]> item in this.KeyMap) {
                if (null == item.Value || string.IsNullOrEmpty(item.Key))
                    continue;

                ControllerKey[] keys = item.Value;
                for (int i = 0; i < keys.Length; i++) {
                    if (key.EqualsTo(keys[i])) {
                        action = item.Key;
                        break;
                    }
                }
            }

            return action;
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


        #region IDisposable Members

        public void Dispose() {
            if (null != m_thread)
                m_thread.Abort();
        }

        #endregion
    }

}
