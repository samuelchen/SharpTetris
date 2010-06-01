
//=======================================================================
// <copyright file="ClientGame.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Network;
using System.Net;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Game {
    public class ClientGame : LocalGame {

        public event EventHandler<PlayerEventArgs> Joined;
        public event EventHandler<PlayerEventArgs> Left;

        public event EventHandler<PlayerEventArgs> PlayerJoined;
        public event EventHandler<PlayerEventArgs> PlayerLeft;

        public ClientGame()
            : base() {
            PrivateInit();
        }

        public ClientGame(Form container)
            : base(EnumGameType.Client, container) {
            PrivateInit();
        }

        #region Properties
        //public int ServerPort { get; set; }
        #endregion

        private void PrivateInit() {
            this.GameType = EnumGameType.Client;

            // the timer is controlled by server.
            m_timer.Dispose();
            m_timer = null;

            m_client = new Client();
            m_client.Connected += new EventHandler(OnConnected);
            m_client.Disconnected += new EventHandler(OnDisconnected);
            m_client.ServerCalled += new EventHandler<NetworkEventArgs>(OnServerCalled);
            m_client.ServerDataValidating += new NetworkDataValidationHandler(OnServerDataValidating);

        }

        #region Network Events

        bool OnServerDataValidating(NetworkContent data) {
            string s = data.GetString();
            if (s.StartsWith("(") && s.EndsWith(")"))
                return true;
            return false;
        }

        void OnServerCalled(object sender, NetworkEventArgs e) {
            string tmp = e.Content.GetString();
            string[] commands = tmp.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < commands.Length; i++) {
                this.ExecuteCommand(this.ParseCommand(commands[i]));
            }
        }

        void OnDisconnected(object sender, EventArgs e) {

            if (this.Left != null && null != m_player) {
                PlayerEventArgs arg = new PlayerEventArgs();
                arg.Player = m_player;
                this.Left(this, arg);
            }
        }

        void OnConnected(object sender, EventArgs e) {

            m_client.Name = m_client.LocalEndPoint.ToString();
            m_player.HostName = m_client.Name;
            if (this.Joined != null && null != m_player) {
                PlayerEventArgs arg = new PlayerEventArgs();
                arg.Player = m_player;
                this.Joined(this, arg);
            }
        }

        #endregion

        public void Connect(IPEndPoint serverEndPoint) {
            m_client.Connect(serverEndPoint);
        }

        public void Connect(string hostNameOrIP, int port) {
            m_client.Connect(hostNameOrIP, port);
        }

        public void Disconnect() {
            m_client.Disconnect();
            OnDisconnected(this, new EventArgs());
        }

        protected string[] ParseCommand(string command) {
            string[] commands = command.Split(
                new char[] { '#', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return commands;
        }

        /// <summary>
        /// Command format (all upper case):
        /// [player]:[command],[argument]
        /// 
        /// player: player name or "ALL".
        /// Game Command:
        ///     NEW - prepare a new game and do initlization.
        ///     START - start the game.
        ///     PAUSE - pause a game.
        ///     RESUME - resume a paused game.
        ///     STOP - Stop a game.
        ///     QUIT - Quit a game. If server quit, all clients quit too.
        ///     JOIN - A new player joined.
        ///     
        /// Play Command:
        ///     GO - Game goes a frame
        ///     MV - Move a character. argument is the moving direction or action.
        ///     NAME - Get the player name of a host. if argument is given, it will set the playe name to argument.
        /// </summary>
        /// <param name="command"></param>
        public void ExecuteCommand(string[] command) {
            Debug.Assert(null != command && command.Length > 0);
            if (null == command || command.Length < 2)
                return;

            string hostName = command[0].Trim(); //client name
            string action = command[1].ToUpper();
            string arg = command.Length == 3 ? command[2] : null;
             
            if (action.Equals("GO")) {
                this.Go();
            } else if (action.Equals("MV")) {
                this.Move(hostName, arg);
            } else if (action.Equals("NEW")) {
                this.New();
            } else if (action.Equals("PAUSE")) {
                this.Pause();
            } else if (action.Equals("RESUME")) {
                this.Resume();
            } else if (action.Equals("STOP")) {
                this.Stop();
            } else if (action.Equals("START")) {
                int lv = 0;
                if (null != arg)
                    lv = Convert.ToInt32(arg);
                this.Start(lv);
            } else if (action.Equals("NAME")) {
                if (null != arg)
                    this.HostSetPlayerName(hostName, arg); // set name
                else
                    this.HostGetPlayerName();   // get name. when getting name, only return local player name.
            } else if (action.Equals("JOIN")) {
                this.Join(hostName); // arg is the hostName of which the player joined from.
            } else if (action.Equals("QUIT")) {
                this.Quit(hostName);
            } else if (action.Equals("MAX")) {
                if (null != arg)
                    this.MaxPlayers = Convert.ToInt32(arg);
            }
        }


        public void CallServer(string data) {
            Debug.Assert(!string.IsNullOrEmpty(data));

            if (m_client == null || string.IsNullOrEmpty(data))
                return;

            string command = string.Format("({0}#{1})", m_client.Name, data);
            NetworkContent content = new NetworkContent(EnumNetworkContentType.String, command);
            m_client.CallServer(content);
        }

        #region Game Action

        public void Go() {

            if (null == this.Players || this.Players.Count < 1)
                return;
            
            foreach (Player player in this.Players.Values){
                player.PlayFiled.Go();
            }             
        }

        public void Move(string hostName, string moving) {
            Debug.Assert(!string.IsNullOrEmpty(hostName));
            Debug.Assert(!string.IsNullOrEmpty(moving));
            
            moving = moving.ToUpper();
            object enumMoving = null;
            if (!GameBase.ActionMapping.TryGetValue(moving, out enumMoving)
                || null == enumMoving)
                return;

            hostName = hostName.Trim();
            if (hostName.Equals("ALL")) {
                foreach (Player player in this.Players.Values)
                    player.PlayFiled.Go(enumMoving);
            } else {
                Player player = this.GetPlayerByhostName(hostName);
                if (null != player && player != m_player)
                    player.PlayFiled.Go(enumMoving);
            }
        }


        private void HostSetPlayerName(string hostName, string playerName) {
            Debug.Assert(!hostName.Equals("ALL"));

            Player player = this.GetPlayerByhostName(hostName);
            Debug.Assert(null != player);
            if (null != player) {
                this.ChangePlayerName(player.Name, playerName);
            }
        }

        private void HostGetPlayerName() {
            Debug.Assert(null != m_player && null != m_client);
            if (null == m_player || null == m_client)
                return;

            if (string.IsNullOrEmpty(m_player.Name))
                m_player.Name = Player.CreateName();

            this.CallServer(string.Format("NAME,{0}", m_player.Name));
        }

        private void Join(string hostName) {
            Debug.Assert(!hostName.Equals("ALL"));
            Debug.Assert(!string.IsNullOrEmpty(hostName));
            if (hostName.Equals(m_client.Name) || string.IsNullOrEmpty(hostName))
                return;

            Player player = this.GetPlayerByhostName(hostName);
            Debug.Assert(null == player);
            if (null != player)
                return;

            player = new Player();
            player.HostName = hostName;
            player.PlayFiled = new PlayPanel();
            this.AddPlayer(player);

            if (null != this.PlayerJoined)
                this.PlayerJoined(this, new PlayerEventArgs(player));

            //if (this.Players.Count == this.MaxPlayers)
            this.CallServer("READY," + this.Players.Count.ToString());
        }

        private void Quit(string hostName) {
            Debug.Assert(!hostName.Equals("ALL"));
            if (hostName.Equals(m_client.Name))
                return;
            Player player = this.GetPlayerByhostName(hostName);
            this.RemovePlayer(player);
            if (null != this.PlayerLeft)
                this.PlayerLeft(this, new PlayerEventArgs(player));
            player = null;
        }

        #endregion


        #region override

        //public override void Start(int level) {
        //    if (null == m_player || this.Players.Count < 1)
        //        return;

        //    base.Start(level);

        //    if (m_player.Controller != null)
        //        m_player.Controller.Pressed += new Net.SamuelChen.Tetris.Controller.ControllerPressHandler(Controller_Pressed);
        //}

        //void Controller_Pressed(object sender, Net.SamuelChen.Tetris.Controller.ControllerPressedEventArgs e) {
        //    if (m_player != null && m_player.Controller != null)
        //        this.CallServer("MOVE," + m_player.Controller.Translate(e.Keys[0]));
        //}

        protected override void OnController_Pressed(object sender, Net.SamuelChen.Tetris.Controller.ControllerPressedEventArgs e) {
            string act = m_player.Controller.Translate(e.Keys[0]);
            if (!string.IsNullOrEmpty(act)) {
                this.CallServer("MV," + act);
            }
            base.OnController_Pressed(sender, e);
        }

        public override void AddPlayer(Player player) {
            if (null == player)
                return;

            base.AddPlayer(player);
            if (this.Players.Count == 1)
                m_player = player;
        }

        public override Player RemovePlayer(string name) {
            if (this.Players.Count == 1)
                m_player = null;                        
            return base.RemovePlayer(name);
        }

        #endregion

        protected Player GetPlayerByhostName(string hostName) {
            foreach (Player player in this.Players.Values) {
                if (hostName.Equals(player.HostName, StringComparison.CurrentCultureIgnoreCase))
                    return player;
            }
            return null;
        }

        #region IDisposable Members

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            m_player = null;
            base.Dispose();
            _disposed = true;
        }

        #endregion

        #region Fields

        protected Client m_client;
        protected Player m_player;

        #endregion

    }

}
