
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
    /// <param name="data">the data (utf8 string)</param>
    /// <returns></returns>
    public delegate bool NetworkDataValidationHandler(NetworkContent data);

    public partial class NetworkEventArgs : EventArgs {
        public RemoteInformation RemoteInformation;
        public NetworkContent Content;

        public NetworkEventArgs(RemoteInformation ri, NetworkContent content)
            : base() {
            this.RemoteInformation = ri;
            this.Content = content;
        }
    }
}
