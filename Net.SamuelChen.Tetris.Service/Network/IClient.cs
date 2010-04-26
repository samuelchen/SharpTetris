
//=======================================================================
// <copyright file="IClient.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Interface to join a remote game
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Net.SamuelChen.Tetris.Service {
    public interface IClient {
        event NetworkDataValidationHandler HostDataValidating;
        event EventHandler<NetworkEventArgs> HostCalled;
        event EventHandler Connected;
        event EventHandler Disconnected;

        bool Connect(string hostNameOrIP, int port);
        void Disconnect();
        bool CallHost(NetworkContent content);
    }
}
