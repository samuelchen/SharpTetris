
//=======================================================================
// <copyright file="Connection.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To define a network client
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.ComponentModel;

namespace Net.SamuelChen.Tetris.Network {

    public class Client : Host, IClient, IDisposable {

        protected TcpClient m_client;
        protected BackgroundWorker m_worker;

        #region ctor

        public Client() : base() { }
        public Client(int port) : base(port) { }
        public Client(int port, Encoding encoding) : base(port, encoding) { }

        protected override void Init() {
            base.Init();
        }

        #endregion

        #region Properties

        public RemoteInformation RemoteHost { get; set; }

        public bool IsConnected { get; protected set; }

        #endregion

        #region IClient Members

        public event NetworkDataValidationHandler ServerDataValidating;
        public event EventHandler<NetworkEventArgs> ServerCalled;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public void Connect(string hostNameOrIP, int port) {
            if (string.IsNullOrEmpty(hostNameOrIP))
                return;

            IPEndPoint serverEndPoint = new IPEndPoint(Dns.GetHostEntry(hostNameOrIP).AddressList[0], port);
            this.Connect(serverEndPoint);

        }

        public void Connect(IPEndPoint serverEndPoint) {
            if (null != m_worker || null != m_client || null == serverEndPoint)
                return;

            m_worker = new BackgroundWorker();
            m_worker.DoWork += new DoWorkEventHandler(Client_DoWork);
            m_worker.ProgressChanged += new ProgressChangedEventHandler(Client_ProgressChanged);
            m_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Client_RunWorkerCompleted);
            m_worker.WorkerReportsProgress = true;
            m_worker.WorkerSupportsCancellation = true;

            m_worker.RunWorkerAsync(serverEndPoint);

        }

        #region worker processes
        void Client_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Trace.TraceInformation("Client \"{0}\" disconnected.");

            if (null != e.Error) {
                Trace.TraceWarning("Catched error while disconnecting. \n{0}\n{1}",
                    e.Error.Message, e.Error.StackTrace);
            }

            this.IsConnected = false;

            if (null != this.Disconnected)
                this.Disconnected(this, new EventArgs());
        }

        void Client_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage == 0) {
                // 0: Connected.
                m_client = e.UserState as TcpClient;
                if (null == m_client)
                    return;

                this.IsConnected = true;
                if (null != this.Connected)
                    this.Connected(this, new EventArgs());

            } else if (e.ProgressPercentage == 1) {
                // 1: Host called
                NetworkContent content = e.UserState as NetworkContent;
                if (null == content)
                    return;

                bool succeed = true; // Assume HostDataValidation == null
                if (null != this.ServerDataValidating)
                    succeed = this.ServerDataValidating(content);

                if (succeed && null != this.ServerCalled)
                    this.ServerCalled(this, new NetworkEventArgs(null, content));
            }
        }

        void Client_DoWork(object sender, DoWorkEventArgs e) {
            IPEndPoint hostEndPoint = e.Argument as IPEndPoint;
            BackgroundWorker worker = sender as BackgroundWorker;
            if (null == hostEndPoint || worker == null) {
                e.Cancel = true;
                return;
            }

            TcpClient client = new TcpClient();
            try {
                
                client.Connect(hostEndPoint);

                if (client.Connected) {

                    //trigger event Connected.
                    worker.ReportProgress(0, client);

                    while (true) {

                        if (worker.CancellationPending || !client.Connected)
                            break;

                        byte[] data = Host.RecevieData(client);
                        if (null != data) {
                            NetworkContent content = new NetworkContent(
                                EnumNetworkContentType.Bianary, data, this.Encoding);

                            // trigger DataValidating and HostCalled.
                            worker.ReportProgress(1, content);
                        }

                        System.Threading.Thread.Sleep(0);
                    }

                }
            } catch (SocketException err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("Client socket error \n{0}\n{1}", err.Message, err.StackTrace);
            } finally {
                client.Close();
            }
        }
        #endregion

        public void Disconnect() {
            if (null != m_client)
                m_client.Close();
            m_client = null;

            if (null != m_worker)
                m_worker.CancelAsync();
            m_worker = null;
        }

        public bool CallServer(NetworkContent content) {
            if (null == m_client || null == content)
                return false;

            return Host.SendData(m_client, content.GetBinary());
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            throw new NotImplementedException();
        }

        #endregion

    }
}
