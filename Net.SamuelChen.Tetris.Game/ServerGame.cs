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

            // tell new player how many players here
            foreach (Player p in this.Players.Values) {
                this.CallClient(player.HostName, "JOIN," + p.HostName);
            }

            // tell all players the new player joined
            this.CallClients("JOIN," + player.HostName);

            // Get name
            this.CallClient(player.HostName, "NAME");
        }

        void OnClientDisconnected(object sender, NetworkEventArgs e) {
            RemoteInformation ri = e.RemoteInformation;
            if (null == ri)
                return;
            Player player = this.GetPlayerByhostName(ri.Name);
            if (null != player)
                this.RemovePlayer(player);

            if (null != this.PlayerLeaved)
                this.PlayerLeaved(this, new PlayerEventArgs(player));

            this.CallClients(player.HostName, "QUIT");
        }

        void OnClientCalled(object sender, NetworkEventArgs e) {
            if (null == e.Content)
                return;
            string hostName = e.RemoteInformation.Name;
            string tmp = e.Content.GetString();
            if (string.IsNullOrEmpty(tmp))
                return;

            string[] commands = tmp.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands) {

                string[] cmd = this.ParseCommand(command);

                if (cmd.Length < 2)
                    break;
                //string hostName = cmd[0];
                //string action = cmd[1];
                //string arg = 
                if (cmd.Length > 2 && cmd[1].Equals("NAME")) {
                    string playerName = cmd[2];
                    Player player = null;
                    if (!this.Players.TryGetValue(playerName, out player)) {
                        player = this.GetPlayerByhostName(hostName);
                        player.Name = playerName;
                    } else {
                        // name is in use
                        player.Name = playerName + "@" + hostName;
                        this.CallClients(hostName, "NAME," + player.Name);
                    }
                } else if (cmd.Length > 1 && cmd[1].Equals("READY")) {
                    Player player = this.GetPlayerByhostName(hostName);
                    player.Tag = "READY";
                    bool bReady = true;
                    foreach (Player p in this.Players.Values) {
                        if (!p.Tag.Equals("READY")) {
                            bReady = false;
                            break;
                        }
                    }
                }
                this.DispatchCommand(e.Content);
            }            
        }

        #endregion

        protected string[] ParseCommand(string command) {
            string[] commands = command.Split(
                new char[] { '#', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return commands;
        }

        protected Player GetPlayerByhostName(string hostName) {
            foreach (Player player in this.Players.Values) {
                if (hostName.Equals(player.HostName, StringComparison.CurrentCultureIgnoreCase))
                    return player;
            }
            return null;
        }

        /*
        public NetworkContent CallClient(string hostName, string command)
        {
            Player player = GetPlayerByhostName(hostName);
            Debug.Assert(null != player);
            if (null == player)
                return null;

            NetworkContent content = new NetworkContent(EnumNetworkContentType.String,
                string.Format("({0}:{1})", player.Name.ToUpper(), command.ToUpper()));
            return m_server.CallClient(hostName, content);
        }
        */

        public void CallClient(string hostName, string command) {
            Player player = GetPlayerByhostName(hostName);
            NetworkContent content = new NetworkContent(EnumNetworkContentType.String, 
                string.Format("({0}#{1})", hostName, command.ToUpper()));
            m_server.CallClient(hostName, content);
        }

        public void CallClients(string command) {
            this.CallClients("ALL", command);
        }

        public void CallClients(string hostName, string command) {
            Debug.Assert(!string.IsNullOrEmpty(hostName) && !string.IsNullOrEmpty(command));
            if (string.IsNullOrEmpty(hostName) || string.IsNullOrEmpty(command))
                return;

            m_server.Boardcast(new NetworkContent(EnumNetworkContentType.String,
                string.Format("({0}#{1})", hostName, command.ToUpper())));
        }

        public void DispatchCommand(NetworkContent content)
        {
            m_server.Boardcast(content);
        }

        #region Game Control

        public void StartService() {
            m_server.MaxConnections = this.MaxPlayers;
            this.CallClients("NAME");
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
            this.CallClients("NEW");
            m_timer.Interval = 3000;

        }

        /// <summary>
        /// Start a game
        /// </summary>
        /// <param name="nLevel">start level</param>
        /// <returns></returns>
        public override void Start(int level) {
            base.Start(level);
            this.CallClients(string.Format("START,{0}", level));
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
            this.CallClients("STOP");
            m_timer.Stop();
            m_server.Stop();
        }

        /// <summary>
        /// Pause
        /// </summary>
        public override void Pause() {
            m_timer.Stop();
            this.CallClients("PAUSE");
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

        //public override void Refresh() {
        //    base.Refresh();
        //}

        //public override void AddPlayer(Player player) {
        //    base.AddPlayer(player);
        //    this.CallClients("JOIN," + player.Name);
        //}

        //public override Player RemovePlayer(string name) {
        //    Player player = base.RemovePlayer(name);
        //    this.CallClients("QUIT," + player.Name);
        //    return player;
        //}

        #endregion

        #region events

        protected void OnTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            foreach (Player player in this.Players.Values) {
                //m_server.CallClient(player.HostName, new NetworkContent(EnumNetworkContentType.String, "GETACTION"));

                // move shape
                this.CallClients("GO");

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
