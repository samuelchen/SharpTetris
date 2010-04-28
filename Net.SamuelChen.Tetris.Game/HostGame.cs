using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Controller;

namespace Net.SamuelChen.Tetris.Game {
    class HostGame : TetrisGame {

        public HostGame()
            : base() {
            PrivateInit();
        }

        public HostGame(EnumGameType type, Form container)
            : base(type, container) {
            PrivateInit();
        }

        #region Properties

        public int ServicePort { get; set; }
        //public int ClientPort { get; set; }
        #endregion

        private void PrivateInit() {
            m_timer = new System.Timers.Timer();
            m_timer.Elapsed += this.OnTimer_Elapsed;
            this.ServicePort = 9527;

            m_server = new Server(this.ServicePort);
            m_server.MaxConnections = this.MaxPlayers;
            m_server.ClientConnecting += new EventHandler<NetworkEventArgs>(Server_ClientConnecting);
            m_server.ClientConnected += new EventHandler<NetworkEventArgs>(Server_ClientConnected);
            m_server.ClientCalled += new EventHandler<NetworkEventArgs>(m_server_ClientCalled);
            m_client = new Client();
        }

        void Server_ClientConnecting(object sender, NetworkEventArgs e) {
            if (this.Status != EnumGameStatus.Initialized)
                e.Cancelled = true;
        }

        void Server_ClientConnected(object sender, NetworkEventArgs e) {
            RemoteInformation ri = e.RemoteInformation;
            if (null == ri)
                return;

            Player player = new Player(ri.Name);
            player.Controller = ControllerFactory.Instance.GetFreeController();
            if (null == player.Controller)
                player.Controller = ControllerFactory.Instance.CreateController();
        }

        void m_server_ClientCalled(object sender, NetworkEventArgs e) {
            throw new NotImplementedException();
        }

        public void WaitForClients() {
            
        }

        #region Game process

        /// <summary>
        /// New game
        /// </summary>
        public override void New() {
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
            m_timer.Start();
            Refresh();
        }

        /// <summary>
        /// Game over
        /// </summary>
        public override void Stop() {
            base.Stop();
            m_timer.Stop();
        }

        /// <summary>
        /// Pause
        /// </summary>
        public override void Pause() {
            m_timer.Stop();
            base.Pause();
        }

        /// <summary>
        /// Resume
        /// </summary>
        public override void Resume() {
            base.Resume();
            m_timer.Start();
        }

        #endregion


        #region events

        protected void OnTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            foreach (Player player in this.Players.Values) {
                PlayPanel panel = player.PlayFiled;

                // move shape
                if (null != panel)
                    panel.Go();

                //TODO: score and level
            }
        }

        #endregion


        #region IDisposable Members

        public override void Dispose() {
            m_timer.Stop();
            m_timer.Dispose();
            base.Dispose();
        }

        #endregion

        #region Fields

        protected System.Timers.Timer m_timer;
        protected Server m_server;
        protected Client m_client;

        #endregion
    }
}
