
//=======================================================================
// <copyright file="RemoteInformation.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;
using System.IO;

namespace Net.SamuelChen.Tetris.Network {
    /// <summary>
    /// Remote Connection information
    /// </summary>
    public class RemoteInformation : ISerializable, IDisposable {
        public string Name { get; set; }
        public TcpClient Connection { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public BackgroundWorker Worker { get; set; }
        public NetworkContent Content { get; set; }

        public Stream GetStream() {
            if (null == this.Connection)
                return null;

            return this.Connection.GetStream();
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (null != this.Connection)
                this.Connection.Close();
            this.Connection = null;
            if (null != this.Worker)
                this.Worker.CancelAsync();
            this.Worker = null;
            this.EndPoint = null;
            this.Name = null;
        }

        #endregion
    }
}
