
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

        protected IDictionary<Guid, WeakReference> m_controllers;

        public VirtualControllerFactory()
            : base() {
            m_controllers = new Dictionary<Guid, WeakReference>();
        }

        public override IEnumerable<IController> EnumControlls() {
            List<IController> controllers = new List<IController>();
            foreach (KeyValuePair<Guid, WeakReference> item in m_controllers) {
                WeakReference r = item.Value;
                if (null == r || null == r.Target || !r.IsAlive)
                    m_controllers.Remove(item.Key);
                else
                    controllers.Add(r.Target as IController);
            }

            return controllers;
        }

        public override IController GetController(string controllerId) {
            if (string.IsNullOrEmpty(controllerId))
                return null;

            try {
                Guid id = new Guid(controllerId);
                return this.GetController(id);
            } catch {
                return null;
            }
        }

        public override IController GetController(Guid controllerId) {
            IController c = null;
            WeakReference r = null;
            Guid id = controllerId;
            if (m_controllers.TryGetValue(id, out r))
                if (null != r && null != r.Target && r.IsAlive)
                    c = r.Target as IController;
            return c;
        }

        public override IController CreateController() {
            IController c = new VirtualController();
            //TODO: m_controller will keep increasing. Try use weak reference.
            WeakReference r = new WeakReference(c);
            m_controllers.Add(c.ID, r);
            return c;
        }

        public void RemoveController(Guid guid) {
            if (null == guid)
                return;
            WeakReference r = null;
            if (m_controllers.TryGetValue(guid, out r)) {
                if (null != r && null != r.Target && r.IsAlive)
                    (r.Target as IController).Dispose();
            }
            m_controllers.Remove(guid);
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
