
//=======================================================================
// <copyright file="NetworkEvent.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Service {
    /// <summary>
    /// Delegate to validate the data
    /// </summary>
    /// <param name="data">the data</param>
    /// <returns></returns>
    public delegate bool NetworkDataValidationHandler(NetworkContent data);

    public class NetworkEventArgs : EventArgs {
        public RemoteInformation RemoteInformation { get; set; }
        public NetworkContent Content { get; set; }
        public bool Cancelled { get; set; }

        public NetworkEventArgs(RemoteInformation ri, NetworkContent content)
            : base() {
            this.RemoteInformation = ri;
            this.Content = content;
            this.Cancelled = false;
        }
    }
}
