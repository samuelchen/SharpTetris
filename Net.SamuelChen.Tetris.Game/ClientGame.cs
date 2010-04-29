
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


        public ClientGame()
            : base() {
            PrivateInit();
        }

        public ClientGame(Form container)
            : base(EnumGameType.Client, container) {
            PrivateInit();
        }

        #region Properties
        public int ServerPort { get; set; }
        #endregion

        private void PrivateInit() {
            this.GameType = EnumGameType.Client;

            // the timer is controlled by server.
            m_timer.Dispose();
            m_timer = null;

            m_clientPlayers = new Dictionary<string, string>();

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
            //throw new NotImplementedException();
        }

        void OnConnected(object sender, EventArgs e) {
            //throw new NotImplementedException();
        }

        #endregion

        public void Connect(IPEndPoint serverEndPoint) {
            m_client.Connect(serverEndPoint);
        }

        public void Disconnect() {
            m_client.Disconnect();
        }

        protected string[] ParseCommand(NetworkContent data) {
            string tmp = data.GetString();
            string[] commands = tmp.Substring(1, tmp.Length - 2).Split(
                new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
            return commands;
        }

        public void ExecuteCommand(string[] command) {
            Debug.Assert(null != command && command.Length > 1);
            if (null != command && command.Length > 1)
                return;

            command[1] = command[1].Trim();
            if (command[1].Equals("GO")) {
                this.Go(command[0]);
            } else if (command[1].Equals("MOVE")) {
                Debug.Assert(command.Length > 2);
                this.Move(command[0], command[2]);
            } else if (command[1].Equals("NEW")) {
                this.New();
            } else if (command[1].Equals("PAUSE")) {
                this.Pause();
            } else if (command[1].Equals("RESUME")) {
                this.Resume();
            } else if (command[1].Equals("STOP")) {
                this.Stop();
            } else if (command[1].Equals("START")) {
                this.Start();
            } else if (command[1].Equals("PLAYER")) {
                if (command.Length == 2)
                    this.HostGetPlayerName(command[0]);
                else
                    this.HostSetPlayerName(command[0], command[2]);
            } else if (command[1].Equals("CLIENT")) {
                if (command.Length == 2)
                    this.HostGetClientName(command[0]);
                else
                    this.HostSetClientName(command[0], command[2]);
            } else if (command[1].StartsWith("CTRL")) {
            }

        }


        public void CallServer(string data) {
            Debug.Assert(!string.IsNullOrEmpty(data));

            string command = string.Format("({0}:{1})", m_client.Name, data);
            NetworkContent content = new NetworkContent(EnumNetworkContentType.String, command);
            m_client.CallServer(content);
        }

        #region Game Action
        public void Go(string clientName) {
            clientName = clientName.Trim();
            if (clientName.Equals("ALL")) {
                foreach (NetworkPlayer player in this.Players.Values)
                    player.PlayFiled.Go();
            } else {
                Player player = null;
                string name = null;
                if (!m_clientPlayers.TryGetValue(clientName, out name) || name == null)
                    return;
                player = this.Players[name];
                player.PlayFiled.Go();
            }

            
        }

        public void Move(string clientName, string enumMoving) {
            clientName = clientName.Trim();
            if (clientName.Equals("ALL")) {
                foreach (NetworkPlayer player in this.Players.Values)
                    player.PlayFiled.Go(enumMoving);
            } else {
                Player player = null;
                string name = null;
                if (!m_clientPlayers.TryGetValue(clientName, out name) || name == null)
                    return;
                player = this.Players[name];
                player.PlayFiled.Go(enumMoving);
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
        protected NetworkPlayer m_player;
        protected IDictionary<string, string> m_clientPlayers;

        #endregion

    }

}
