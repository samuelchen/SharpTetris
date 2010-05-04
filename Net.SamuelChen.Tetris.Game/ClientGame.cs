
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
            this.ExecuteCommand(this.ParseCommand(e.Content));
        }

        void OnDisconnected(object sender, EventArgs e) {

            if (this.Left != null && null != m_player) {
                PlayerEventArgs arg = new PlayerEventArgs();
                arg.Player = m_player;
                this.Left(this, arg);
            }
        }

        void OnConnected(object sender, EventArgs e) {
            
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

        protected string[] ParseCommand(NetworkContent data) {
            string tmp = data.GetString();
            string[] commands = tmp.Substring(1, tmp.Length - 2).Split(
                new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return commands;
        }

        public void ExecuteCommand(string[] command) {
            Debug.Assert(null != command && command.Length > 0);
            if (null == command || command.Length < 1)
                return;
             
            command[0] = command[0].Trim();
            if (command[0].Equals("GO")) {
                this.Go();
            } else if (command[0].Equals("MOVE")) {
                Debug.Assert(command.Length > 2);
                this.Move(command[0], command[2]);
            } else if (command[0].Equals("NEW")) {
                this.New();
            } else if (command[0].Equals("PAUSE")) {
                this.Pause();
            } else if (command[0].Equals("RESUME")) {
                this.Resume();
            } else if (command[0].Equals("STOP")) {
                this.Stop();
            } else if (command[0].Equals("START")) {
                int lv = 0;
                if (command.Length == 2)
                    lv = Convert.ToInt32(command[1]);
                this.Start(lv);
            } else if (command[0].Equals("PLAYER")) {
                if (command.Length == 2)
                    this.HostGetPlayerName(command[1]);
                else if (command.Length == 3)
                    this.HostSetPlayerName(command[1], command[2]);
            } else if (command[0].Equals("CLIENT")) {
                if (command.Length == 2)
                    this.HostGetClientName(command[1]);
                else
                    this.HostSetClientName(command[1], command[2]);
            } else if (command[0].StartsWith("CTRL")) {
            }

        }


        public void CallServer(string data) {
            Debug.Assert(!string.IsNullOrEmpty(data));

            if (m_player == null || string.IsNullOrEmpty(data))
                return;

            string command = string.Format("({0}:{1})", m_player.Name, data);
            NetworkContent content = new NetworkContent(EnumNetworkContentType.String, command);
            m_client.CallServer(content);
        }

        #region Game Action

        public void Go() {

            if (null == this.Players || this.Players.Count < 1)
                return;
            
            foreach (Player player in this.Players.Values){
                player.PlayFiled.Go();
                break;
            }             
        }

        public void Move(string playerName, string enumMoving) {
            Debug.Assert(!string.IsNullOrEmpty(playerName));

            playerName = playerName.Trim();
            if (playerName.Equals("ALL")) {
                foreach (Player player in this.Players.Values)
                    player.PlayFiled.Go(enumMoving);
            } else {
                Player player = null;
                if (this.Players.TryGetValue(playerName, out player)) {
                    player.PlayFiled.Go(enumMoving);
                }
            }
        }


        private void HostSetClientName(string clientName, string name) {
            throw new NotImplementedException();
        }

        private void HostGetClientName(string clientName) {
            throw new NotImplementedException();
        }

        private void HostSetPlayerName(string clientName, string name) {
            throw new NotImplementedException();
        }

        private void HostGetPlayerName(string clientName) {
            throw new NotImplementedException();
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
            this.CallServer("MOVE," + m_player.Controller.Translate(e.Keys[0]));
            base.OnController_Pressed(sender, e);
        }

        public override void AddPlayer(Player player) {
            if (null == player)
                return;
            
            base.AddPlayer(player);
            m_player = player;
        }

        public override void RemovePlayer(Player player) {
            base.RemovePlayer(player);
            m_player = null;
        }

        #endregion


        #region IDisposable Members

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

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
