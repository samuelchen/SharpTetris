
//=======================================================================
// <copyright file="IClient.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Interface for client to join a remote game
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;

namespace Net.SamuelChen.Tetris.Network {
    public interface IClient {
        event NetworkDataValidationHandler ServerDataValidating;
        event EventHandler<NetworkEventArgs> ServerCalled;
        event EventHandler Connected;
        event EventHandler Disconnected;

        void Connect(string hostNameOrIP, int port);
        void Disconnect();
        bool NotifyServer(NetworkContent content);
    }
}
