
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
        /// user data
        /// </summary>
        public object Tag { get; set; }

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
            this.Target = target;
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
        public virtual void Start() {
            this.Working = true;
        }

        /// <summary>
        /// Stop using this controller.
        /// </summary>
        public virtual void Stop() {
            this.Working = false;
        }

        public virtual ControllerKey[] Translate(string action) {
            if (null == this.KeyMap)
                return null;
            ControllerKey[] keys = null;

            this.KeyMap.TryGetValue(action, out keys);
            return keys;

        }

        public virtual string Translate(ControllerKey key) {
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


        #region IDisposable Members

        private bool _disposed = false;
        public virtual void Dispose() {
            if (_disposed)
                return;

            this.Deattach();
            this.Stop();
            this.Target = null;
            this.Tag = null;
            this.KeyMap.Clear();
            this.KeyMap = null;

            _disposed = true;
        }

        #endregion
    }

}
