
//=======================================================================
// <copyright file="Connection.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To define a network client
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace Net.SamuelChen.Tetris.Service {

    public class Client : IClient, IDisposable {

        protected TcpClient m_client;
        protected BackgroundWorker m_worker;

        public Client(int port) {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            this.LocalEndPoint = new IPEndPoint(host.AddressList[0], port);
            this.Encoding = Encoding.Unicode;
            this.Name = Dns.GetHostName();
        }

        public Client(int port, Encoding encoding)
            : this(port) {
            this.Encoding = encoding;
        }

        #region Properties

        public IPEndPoint LocalEndPoint { get; set; }
        public Encoding Encoding { get; private set; }
        public string Name { get; set; }
        public RemoteInformation Host { get; set; }

        #endregion

        #region IClient Members

        public event NetworkDataValidationHandler HostDataValidating;
        public event EventHandler<NetworkEventArgs> HostCalled;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public bool Connect(string hostNameOrIP, int port) {
            if (null != m_worker || null != m_client)
                return false;

            m_worker = new BackgroundWorker();
            m_worker.DoWork += new DoWorkEventHandler(Client_DoWork);
            m_worker.ProgressChanged += new ProgressChangedEventHandler(Client_ProgressChanged);
            m_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Client_RunWorkerCompleted);
            m_worker.WorkerReportsProgress = true;
            m_worker.WorkerSupportsCancellation = true;

            IPEndPoint hostEndPoint = new IPEndPoint(Dns.GetHostEntry(hostNameOrIP).AddressList[0], port);
            m_worker.RunWorkerAsync(hostEndPoint);

            return true;
        }

        #region worker processes
        void Client_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            Trace.TraceInformation("Client \"{0}\" disconnected.");

            if (null != e.Error) {
                Trace.TraceWarning("Catched error while disconnecting. \n{0}\n{1}",
                    e.Error.Message, e.Error.StackTrace);
            }

            if (null != this.Disconnected)
                this.Disconnected(this, new EventArgs());
        }

        void Client_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage == 0) {
                // 0: Connected.
                m_client = e.UserState as TcpClient;
                if (null == m_client)
                    return;

                if (null != this.Connected)
                    this.Connected(this, new EventArgs());

            } else if (e.ProgressPercentage == 1) {
                // 1: Host called
                NetworkContent content = e.UserState as NetworkContent;
                if (null == content)
                    return;

                bool succeed = true; // Assume HostDataValidation == null
                if (null != this.HostDataValidating)
                    succeed = this.HostDataValidating(content);

                if (succeed && null != this.HostCalled)
                    this.HostCalled(this, new NetworkEventArgs(null, content));
            }
        }

        void Client_DoWork(object sender, DoWorkEventArgs e) {
            IPEndPoint hostEndPoint = e.Argument as IPEndPoint;
            BackgroundWorker worker = sender as BackgroundWorker;
            if (null == hostEndPoint || worker == null) {
                e.Cancel = true;
                return;
            }

            byte[] buf = new byte[256];
            int i = 0, j = 0, l = 0;
            TcpClient client = new TcpClient();

            try {
                client.Connect(hostEndPoint);

                //trigger event Connected.
                worker.ReportProgress(0, client);

                NetworkStream ns = client.GetStream();

                while (true) {

                    if (worker.CancellationPending)
                        break;

                    if (ns.CanRead) {
                        using (MemoryStream ms = new MemoryStream()) {
                            while (ns.DataAvailable && (i = ns.Read(buf, i, buf.Length)) != 0) {
                                l = i - j;
                                ms.Write(buf, 0, l);
                                j = i;
                            }
                            ms.Flush();
                            NetworkContent content = null;
                            if (ms.Length > 0) {
                                content = new NetworkContent(
                                    EnumNetworkContentType.Bianary, ms.GetBuffer(), this.Encoding); // Setter of this.Encoding is private, no need lock

                                // trigger DataValidating and HostCalled.
                                worker.ReportProgress(1, content);
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(0);
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

        public bool CallHost(NetworkContent content) {
            if (null == m_client || null == content)
                return false;

            byte[] buf = content.GetBinary();

            NetworkStream ns = m_client.GetStream();
            if (null != ns && ns.CanWrite && buf != null) {
                ns.Write(buf, 0, buf.Length);
                ns.Flush();
            }

            return true;
        }

        #endregion

        #region IDisposable Members

        public void Dispose() {
            throw new NotImplementedException();
        }

        #endregion

    }
}
