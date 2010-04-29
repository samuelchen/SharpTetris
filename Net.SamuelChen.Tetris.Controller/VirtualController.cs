
//=======================================================================
// <copyright file="VirtualController.cs" company="Samuel Chen Studio">
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
    public class VirtualController : Controller {
        private static int m_autoId = 0;

        public VirtualController()
            : base() {
            this.ID = Guid.NewGuid();
            this.ButtonCount = -1;
            this.Name = VirtualController.CreateName();
        }

        public static string CreateName() {
            return string.Format("Virtual Controller {0}", m_autoId++);
        }

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            base.Dispose();
            _disposed = true;
        }
    }
}
