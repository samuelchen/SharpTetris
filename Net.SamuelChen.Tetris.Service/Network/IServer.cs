
//=======================================================================
// <copyright file="IServer.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Interface to server a game
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================
        
        
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Net.SamuelChen.Tetris.Service {

    public interface IServer {

        event NetworkDataValidationHandler ClientDataValidating;
        event EventHandler<NetworkEventArgs> ClientConnecting;
        event EventHandler<NetworkEventArgs> ClientConnected;
        event EventHandler<NetworkEventArgs> ClientDisconnected;
        event EventHandler Started;
        event EventHandler<NetworkEventArgs> ClientCalled;
        event EventHandler Stopped;

        void Start();
        void Stop();
        void Boardcast(NetworkContent content);
        bool CallClient(string name, NetworkContent content);
    }
}
