
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
                new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return commands;
        }

        public void ExecuteCommand(string[] command) {
            Debug.Assert(null != command && command.Length > 0);
            if (null == command || command.Length < 2)
                return;

            string name = command[0].Trim();
            string action = command[1];
             
            if (action.Equals("GO")) {
                this.Go();
            } else if (action.Equals("MOVE")) {
                Debug.Assert(command.Length > 2);
                this.Move(name, command[2]);
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
                if (command.Length == 3)
                    lv = Convert.ToInt32(command[2]);
                this.Start(lv);
            } else if (action.Equals("PLAYER")) {
                if (command.Length == 3)
                    this.HostGetPlayerName(name);
                else if (command.Length == 3)
                    this.HostSetPlayerName(name, command[2]);
            } else if (action.Equals("CLIENT")) {
                if (command.Length == 2)
                    this.HostGetClientName(name);
                else
                    this.HostSetClientName(name, command[2]);
            } else if (action.StartsWith("CTRL")) {
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
            string act = m_player.Controller.Translate(e.Keys[0]);
            if (!string.IsNullOrEmpty(act)) {
                this.CallServer("MOVE," + act);
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

        public override void RemovePlayer(Player player) {
            if (this.Players.Count == 1)
                m_player = null;                        
            base.RemovePlayer(player);
        }

        #endregion


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
