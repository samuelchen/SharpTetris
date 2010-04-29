using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Network;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Game {
    class HostGame : TetrisGame {

        public HostGame()
            : base() {
            PrivateInit();
        }

        #region Properties

        public int ServicePort { get; set; }
        
        #endregion

        private void PrivateInit() {
            m_timer = new System.Timers.Timer();
            m_timer.Elapsed += this.OnTimer_Elapsed;

            this.GameType = EnumGameType.Host;
            this.ServicePort = 9527;

            m_clientPlayers = new Dictionary<string, string>();

            m_server = new Server(this.ServicePort);
            m_server.MaxConnections = this.MaxPlayers;
            m_server.ClientConnecting += new EventHandler<NetworkEventArgs>(OnClientConnecting);
            m_server.ClientConnected += new EventHandler<NetworkEventArgs>(OnClientConnected);
            m_server.ClientCalled += new EventHandler<NetworkEventArgs>(OnClientCalled);
            m_server.ClientDataValidating += new NetworkDataValidationHandler(OnClientDataValidating);
            m_server.ClientDisconnected += new EventHandler<NetworkEventArgs>(OnClientDisconnected);
            m_server.Started += new EventHandler(OnStarted);
            m_server.Stopped += new EventHandler(OnStopped);

        }

        #region Server events

        void OnStopped(object sender, EventArgs e) {
            //throw new NotImplementedException();
        }

        void OnStarted(object sender, EventArgs e) {
            //throw new NotImplementedException();
        }

        bool OnClientDataValidating(NetworkContent data) {
            //throw new NotImplementedException();
            return true;
        }

        void OnClientConnecting(object sender, NetworkEventArgs e) {
            if (this.Status != EnumGameStatus.Initialized
                || this.Players.Count == this.MaxPlayers)
                e.Cancelled = true;
        }

        void OnClientConnected(object sender, NetworkEventArgs e) {
            RemoteInformation ri = e.RemoteInformation;
            if (null == ri)
                return;

            NetworkPlayer player = new NetworkPlayer();
            player.HostName = ri.Name;
            player.EndPoint = ri.EndPoint;
            this.AddPlayer(player);
        }

        void OnClientDisconnected(object sender, NetworkEventArgs e) {
            RemoteInformation ri = e.RemoteInformation;
            if (null == ri)
                return;
            NetworkPlayer player = this.GetPlayerByHostName(ri.Name);
            if (null != player)
                this.RemovePlayer(player);
        }

        void OnClientCalled(object sender, NetworkEventArgs e) {
            if (null == e.Content)
                return;

            this.CallClients(e.Content.GetString());
            
        }

        #endregion

        protected NetworkPlayer GetPlayerByHostName(string hostname) {
            foreach (NetworkPlayer player in this.Players.Values) {
                if (player.HostName.Equals(hostname, StringComparison.CurrentCultureIgnoreCase))
                    return player;
            }
            return null;
        }

        public void CallClient(string name, string command) {
            m_server.CallClient(name, new NetworkContent(EnumNetworkContentType.String, command));
        }

        public void CallClients(string command) {
            m_server.Boardcast(new NetworkContent(EnumNetworkContentType.String, command));
        }

        #region override

        public override void AddPlayer(Player player) {
            NetworkPlayer p = player as NetworkPlayer;
            Debug.Assert(p != null && p.Name != null && p.HostName != null);
            base.AddPlayer(p);
            m_clientPlayers.Add(p.HostName, p.Name);
        }

        public override void RemovePlayer(Player player) {
            NetworkPlayer p = player as NetworkPlayer;
            Debug.Assert(p != null && p.Name != null && p.HostName != null);
            base.RemovePlayer(p);
            m_clientPlayers.Remove(p.HostName);
        }

        /// <summary>
        /// New game
        /// </summary>
        public override void New() {
            this.CallClients("NEW");
            base.New();
            m_timer.Interval = 3000;
            m_server.Start();
        }


        /// <summary>
        /// Start a game
        /// </summary>
        /// <param name="nLevel">start level</param>
        /// <returns></returns>
        public override void Start(int level) {
            base.Start(level);
            this.CallClients(string.Format("START,{0}", level));
            m_timer.Start();
            Refresh();
        }

        /// <summary>
        /// Game over
        /// </summary>
        public override void Stop() {
            this.CallClients("STOP");
            base.Stop();
            m_timer.Stop();
            m_server.Stop();
        }

        /// <summary>
        /// Pause
        /// </summary>
        public override void Pause() {
            this.CallClients("PAUSE");
            m_timer.Stop();
            base.Pause();
        }

        /// <summary>
        /// Resume
        /// </summary>
        public override void Resume() {
            base.Resume();
            this.CallClients("RESUME");
            m_timer.Start();
        }

        public override void Refresh() {
            base.Refresh();
        }

        #endregion


        #region events

        protected void OnTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            foreach (NetworkPlayer player in this.Players.Values) {
                //m_server.CallClient(player.HostName, new NetworkContent(EnumNetworkContentType.String, "GETACTION"));

                // move shape
                m_server.Boardcast(new NetworkContent(EnumNetworkContentType.String, "GO"));

                //TODO: score and level
            }
        }

        #endregion


        #region IDisposable Members

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            m_timer.Stop();
            m_timer.Dispose();
            base.Dispose();

            _disposed = true;
        }

        #endregion

        #region Fields

        protected System.Timers.Timer m_timer;
        protected Server m_server;
        protected IDictionary<string, string> m_clientPlayers;

        #endregion
    }
}
