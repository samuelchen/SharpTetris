
//=======================================================================
// <copyright file="VirtualControllerFactory.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Controller {
    internal class VirtualControllerFactory : ControllerFactory {

        protected IDictionary<Guid, IController> m_controllers;
        private static int m_autoId = 0;

        public VirtualControllerFactory()
            : base() {
            m_controllers = new Dictionary<Guid, IController>();
        }

        public override IEnumerable<IController> EnumControlls() {
            return m_controllers.Values;
        }

        public override IController GetController(string controllerId) {
            if (string.IsNullOrEmpty(controllerId))
                return null;

            try {
                Guid id = new Guid(controllerId);
                return GetController(id);
            } catch {
                return null;
            }
        }

        public override IController GetController(Guid controllerId) {
            IController c = null;
            Guid id = controllerId;
            m_controllers.TryGetValue(id, out c);
            return c;
        }

        public override IController CreateController() {
            IController c = new VirtualController();
            //TODO: m_controller will keep increasing. Try use weak reference.
            // WeakReference r = new WeakReference(c);
            m_controllers.Add(c.ID, c);
            return c;
        }

        public static string CreateName() {
            return string.Format("Virtual Controller {0}", m_autoId++);
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
