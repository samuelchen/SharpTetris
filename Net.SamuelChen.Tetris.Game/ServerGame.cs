using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Network;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Game {
    public class ServerGame : TetrisGame {

        public event EventHandler<PlayerEventArgs> PlayerJoined;
        public event EventHandler<PlayerEventArgs> PlayerLeaved;

        public ServerGame()
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
            
        }

        bool OnClientDataValidating(NetworkContent data) {
            //throw new NotImplementedException();
            return true;
        }

        void OnClientConnecting(object sender, NetworkEventArgs e) {
            if (this.Players.Count == this.MaxPlayers)
                e.Cancelled = true;
        }

        void OnClientConnected(object sender, NetworkEventArgs e) {
            RemoteInformation ri = e.RemoteInformation;
            if (null == ri)
                return;

            Player player = new Player();
            player.HostName = ri.Name;
            player.EndPoint = ri.EndPoint;
            this.AddPlayer(player);

            if (null != this.PlayerJoined)
                this.PlayerJoined(this, new PlayerEventArgs(player));

            NetworkContent content = this.CallClient(player.HostName, "NAME");
            string[] tmp = content.GetString().Split(new char[] { ':' });
            player.Name = tmp[0];
        }

        void OnClientDisconnected(object sender, NetworkEventArgs e) {
            RemoteInformation ri = e.RemoteInformation;
            if (null == ri)
                return;
            Player player = this.GetPlayerByHostName(ri.Name);
            if (null != player)
                this.RemovePlayer(player);

            if (null != this.PlayerLeaved)
                this.PlayerLeaved(this, new PlayerEventArgs(player));
        }

        void OnClientCalled(object sender, NetworkEventArgs e) {
            if (null == e.Content)
                return;

            this.DispatchCommand(e.Content);
            
        }

        #endregion

        protected Player GetPlayerByHostName(string hostname) {
            foreach (Player player in this.Players.Values) {
                if (player.HostName.Equals(hostname, StringComparison.CurrentCultureIgnoreCase))
                    return player;
            }
            return null;
        }

        public NetworkContent CallClient(string hostname, string command)
        {
            Player player = GetPlayerByHostName(hostname);
            Debug.Assert(null != player);
            if (null == player)
                return null;

            NetworkContent content = new NetworkContent(EnumNetworkContentType.String,
                string.Format("({0}:{1})", player.Name.ToUpper(), command.ToUpper()));
            return m_server.CallClient(hostname, content);
        }

        public void NotifyClient(string hostname, string command) {
            Player player = GetPlayerByHostName(hostname);
            Debug.Assert(null != player);
            if (null == player)
                return;
            NetworkContent content = new NetworkContent(EnumNetworkContentType.String, 
                string.Format("({0}:{1})", player.Name.ToUpper(), command.ToUpper()));
            m_server.NotifyClient(hostname, content);
            content = null;

        }

        public void NotifyClients(string command) {
            this.NotifyClients("ALL", command);
        }

        public void NotifyClients(string hostname, string command) {
            Debug.Assert(!string.IsNullOrEmpty(hostname) && !string.IsNullOrEmpty(command));
            if (string.IsNullOrEmpty(hostname) || string.IsNullOrEmpty(command))
                return;

            Player player = GetPlayerByHostName(hostname);
            Debug.Assert(null != player);
            if (null == player)
                return;
            m_server.Boardcast(new NetworkContent(EnumNetworkContentType.String,
                string.Format("({0}:{1})", player.Name.ToUpper(), command.ToUpper())));
        }

        public void DispatchCommand(NetworkContent content)
        {
            m_server.Boardcast(content);
        }

        #region Game Control

        public void StartService() {
            m_server.MaxConnections = this.MaxPlayers;
            m_server.Start();
        }
        public void StopService() {
            m_server.Stop();
        }

        /// <summary>
        /// New game
        /// </summary>
        public override void New() {
            base.New();
            this.NotifyClients("NEW");
            m_timer.Interval = 3000;

        }

        /// <summary>
        /// Start a game
        /// </summary>
        /// <param name="nLevel">start level</param>
        /// <returns></returns>
        public override void Start(int level) {
            base.Start(level);
            this.NotifyClients(string.Format("START,{0}", level));
            m_timer.Interval = 3000 - level * 100;
            m_timer.Start();
        }

        public override void Start() {
            this.Start(0);
        }

        /// <summary>
        /// Game over
        /// </summary>
        public override void Stop() {
            base.Stop();
            this.NotifyClients("STOP");
            m_timer.Stop();
            m_server.Stop();
        }

        /// <summary>
        /// Pause
        /// </summary>
        public override void Pause() {
            m_timer.Stop();
            this.NotifyClients("PAUSE");
            base.Pause();
        }

        /// <summary>
        /// Resume
        /// </summary>
        public override void Resume() {
            base.Resume();
            this.NotifyClients("RESUME");
            m_timer.Start();
        }

        //public override void Refresh() {
        //    base.Refresh();
        //}

        #endregion

        #region events

        protected void OnTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            foreach (Player player in this.Players.Values) {
                //m_server.CallClient(player.HostName, new NetworkContent(EnumNetworkContentType.String, "GETACTION"));

                // move shape
                this.NotifyClients("GO");

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

        #endregion
    }
}
