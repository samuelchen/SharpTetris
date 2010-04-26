
//=======================================================================
// <copyright file="Host.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To define a network host
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.ComponentModel;

namespace Net.SamuelChen.Tetris.Service {
    public class Host : IHost, IDisposable {

        private Dictionary<string, RemoteInformation> m_clients;
        private BackgroundWorker m_worker;
        private int m_autoNameId = 0;

        public static object _syncObject = new object();

        public Host(int port) {
            m_clients = new Dictionary<string, RemoteInformation>();
           
            IPHostEntry host = Dns.GetHostEntry("localhost");
            this.LocalEndPoint = new IPEndPoint(host.AddressList[0], port);
            this.Encoding = Encoding.Unicode;  //default encoding
            this.Name = Dns.GetHostName();
        }

        public Host(int port, Encoding encoding)
            : this(port) {
            this.Encoding = encoding;
        }

        #region Properties

        public IPEndPoint LocalEndPoint { get; set; }
        public Encoding Encoding { get; private set; }
        public string Name { get; set; }

        #endregion

        #region IHost Members

        public event NetworkDataValidationHandler ClientDataValidating;
        public event EventHandler<NetworkEventArgs> ClientConnected;
        public event EventHandler<NetworkEventArgs> ClientDisconnected;
        public event EventHandler Started;
        public event EventHandler<NetworkEventArgs> ClientCalled;
        public event EventHandler Stopped;

        public void Start() {
            if (null != m_worker)
                return;

            m_worker = new BackgroundWorker();
            m_worker.DoWork += new DoWorkEventHandler(Host_DoWork);
            m_worker.ProgressChanged += new ProgressChangedEventHandler(Host_ProgressChanged);
            m_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Host_RunWorkerCompleted);
            m_worker.WorkerReportsProgress = true;
            m_worker.WorkerSupportsCancellation = true;
            m_worker.RunWorkerAsync(this.LocalEndPoint);
        }

        public void Stop() {
            if (null == m_worker)
                return;

            foreach (KeyValuePair<string, RemoteInformation> item in m_clients) {
                string name = item.Key; //remote name
                RemoteInformation ri = item.Value;
                ri.Dispose();
            }
            m_clients.Clear();

            m_worker.CancelAsync();
            m_worker = null;

            if (null != this.Stopped)
                this.Stopped(this, new EventArgs());
        }

        public void Boardcast(NetworkContent content) {
            if (null == m_worker || null == content || m_clients.Count == 0)
                return;

            foreach (string name in m_clients.Keys) {
                this.CallClient(name, content);
            }
        }

        public bool CallClient(string name, NetworkContent content) {
            if (string.IsNullOrEmpty(name) || null == content)
                return false;

            RemoteInformation ri = null;
            if (!m_clients.TryGetValue(name, out ri))
                return false;

            Stream s = ri.GetStream();
            byte[] buf = content.GetBinary();
                
            if (null != s && s.CanWrite && buf != null) {
                s.Write(buf, 0, buf.Length);
                s.Flush();
            }

            return true;
        }

        public bool CallClient(RemoteInformation ri, NetworkContent content) {
            if (null == ri || null == content)
                return false;

            Stream s = ri.GetStream();
            byte[] buf = content.GetBinary();

            if (null != s && s.CanWrite && buf != null) {
                s.Write(buf, 0, buf.Length);
                s.Flush();
            }

            return true;
        }

        #endregion

        #region Host client connection process
        void Host_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (null != e.Error) {
                Trace.TraceWarning("Host catched error while waiting client. \n{0}\n{1}",
                    e.Error.Message, e.Error.StackTrace);
            }

            Trace.TraceInformation("Host stopped waiting clients.");

            m_worker.Dispose();
            m_worker = null;

            if (null != this.Stopped)
                this.Stopped(this, new EventArgs());

        }

        void Host_ProgressChanged(object sender, ProgressChangedEventArgs e) {

            if (e.ProgressPercentage == 0) {
                // 0: Started.
                if (null != this.Started)
                    this.Started(this, new EventArgs());

            } else if (e.ProgressPercentage == 1) {
                // 1: Client connected
                RemoteInformation ri = e.UserState as RemoteInformation;
                if (null == ri)
                    return;

                if (null != ri.Worker)
                    return; // already started.

                bool succeed = true; // Assume ClientDataValidating is null
                if (null != this.ClientDataValidating)
                    succeed = this.ClientDataValidating(ri.Content);

                if (succeed && null != this.ClientConnected) {
                    ri.Worker = new BackgroundWorker();
                    ri.Worker.DoWork += new DoWorkEventHandler(Client_DoWork);
                    ri.Worker.ProgressChanged += new ProgressChangedEventHandler(Client_ProgressChanged);
                    ri.Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Client_RunWorkerCompleted);
                    ri.Worker.WorkerReportsProgress = true;
                    ri.Worker.WorkerSupportsCancellation = true;
                    ri.Worker.RunWorkerAsync(ri);

                    NetworkEventArgs arg = new NetworkEventArgs(ri,
                        new NetworkContent(EnumNetworkContentType.String, ri.Name, this.Encoding)); // Setter of this.Encoding is private, no need lock
                    this.ClientConnected(this, arg);

                    // the name could be changed in event by caller.
                    m_clients.Add(ri.Name, ri);
                }
            }

        }

        void Host_DoWork(object sender, DoWorkEventArgs e) {
            IPEndPoint endpoint = e.Argument as IPEndPoint;
            BackgroundWorker worker = sender as BackgroundWorker;
            if (null == endpoint || null == worker) {
                e.Cancel = true;
                return;
            }

            TcpListener lisenter = new TcpListener(endpoint);
            try {
                byte[] buf = new byte[256];

                lisenter.Start();
                worker.ReportProgress(0, null);

                while (true) {
                    if (worker.CancellationPending)
                        break;

                    if (lisenter.Pending()) {
                        TcpClient client = lisenter.AcceptTcpClient();

                        NetworkStream ns = client.GetStream();
                        int i = 0, j = 0, l = 0;

                        using (MemoryStream ms = new MemoryStream()) {

                            while (ns.DataAvailable && (i = ns.Read(buf, i, buf.Length)) != 0) {
                                l = i - j;
                                ms.Write(buf, 0, l);
                                j = i;
                            }
                            ms.Flush();

                            NetworkContent content = null;
                            if (ms.Length > 0)
                                content = new NetworkContent(EnumNetworkContentType.Bianary, ms.GetBuffer(), this.Encoding);


                            RemoteInformation ri = new RemoteInformation();
                            ri.Connection = client;
                            ri.EndPoint = client.Client.RemoteEndPoint as IPEndPoint; //TODO: Need to verify - EndPoint as IPEndPoint 
                            ri.Name = CreateClientName();
                            ri.Content = content;

                            worker.ReportProgress(1, ri); // progress == 1 means client connected.

                            // Do not dispose ri here.
                        }
                    }

                    Thread.Sleep(0);
                }

            } catch (SocketException err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("Host socket error. \n{0}\n{1}", err.Message, err.StackTrace);
            } finally {
                lisenter.Stop();
            }

            e.Result = null;
        }

        #endregion

        #region methods to process client communication

        void Client_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            RemoteInformation ri = e.Result as RemoteInformation;
            string name = (null == ri ? @"N\A" : ri.Name);

            Trace.TraceInformation("Client \"{0}\" disconnected.", name);

            if (null != e.Error) {
                Trace.TraceWarning("Catched error while client \"{0}\" disconnected. \n{1}\n{2}",
                    name, e.Error.Message, e.Error.StackTrace);
            }

            if (null != this.ClientDisconnected)
                this.ClientDisconnected(this, new NetworkEventArgs(ri, null));
        }

        void Client_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            if (e.ProgressPercentage == 0) {
                // 0: started
                if (null != this.Started)
                    this.Started(this, new EventArgs());

            } else if (e.ProgressPercentage == 1) {
                // 1: Client connected.
                RemoteInformation ri = e.UserState as RemoteInformation;
                if (null == ri)
                    return;

                if (null != ri.Content) {
                    bool succeed = true; // Assume ClientDataValidating is null
                    if (null != this.ClientDataValidating)
                        succeed = this.ClientDataValidating(ri.Content);

                    if (succeed && null != this.ClientCalled) {
                        this.ClientCalled(this, new NetworkEventArgs(ri, ri.Content));
                    }
                }
            }
        }

        void Client_DoWork(object sender, DoWorkEventArgs e) {
            RemoteInformation ri = e.Argument as RemoteInformation;
            BackgroundWorker worker = sender as BackgroundWorker;
            if (null == ri || null == worker) {
                e.Cancel = true;
                return;
            }


            try {
                byte[] buf = new byte[256];
                TcpClient client = null;
                NetworkStream ns = null;
                while (true) {

                    if (worker.CancellationPending)
                        break;

                    lock (_syncObject) {
                        client = ri.Connection;
                    }

                    // disconnected.
                    if (null == client || null == (ns = client.GetStream()))
                        break;

                    if (ns.CanRead) {
                        using (MemoryStream ms = new MemoryStream()) {

                            int i = 0, j = 0, l = 0;
                            while (ns.DataAvailable && (i = ns.Read(buf, i, buf.Length)) != 0) {
                                l = i - j;
                                ms.Write(buf, 0, l);
                                j = i;
                            }
                            ms.Flush();

                            if (ms.Length > 0) {
                                ri.Content = new NetworkContent(EnumNetworkContentType.Bianary, ms.GetBuffer(), this.Encoding);

                                worker.ReportProgress(1, ri); // 1 means client called
                            }
                        }
                    }

                    Thread.Sleep(0);
                }

            } catch (SocketException err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("Client \"{0}\" socket error. \n{1}\n{2}", ri.Name, err.Message, err.StackTrace);
            } finally {
                ri.Content = null;
            }

            e.Result = ri;

        }

        #endregion


        private string CreateClientName() {
            return string.Format("client{0}", m_autoNameId++);
        }

        #region IDisposable Members

        public void Dispose() {
            this.Stop();
            if (null != m_worker)
                m_worker.CancelAsync();
        }

        #endregion
    }
}
