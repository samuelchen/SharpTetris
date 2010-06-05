
//=======================================================================
// <copyright file="MainForm.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : The main window of the game.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Net.SamuelChen.Tetris.Skin;
using Net.SamuelChen.Tetris.Game;
using System.Diagnostics;
using System.Net;
using System.Drawing;
using Net.SamuelChen.Tetris.Rule;

namespace Net.SamuelChen.Tetris {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
            LoadSkins();

            for (int i = 0; i < mnMain.Items.Count; i++) {
                ToolStripItem mi = mnMain.Items[i];
                mi.Click += new EventHandler(OnAllMenu_Click);
            }

            m_gameContainer = this;
        }

        #region UI Events

        private void MainForm_Load(object sender, EventArgs e) {

        }

        private void miNew_Click(object sender, EventArgs e) {

            if (null != m_game)
                m_game.Pause();

            NewGameForm wizardNewGame = new NewGameForm();
            if (wizardNewGame.ShowDialog(this) == DialogResult.OK) {
                IList<object> options = wizardNewGame.Options;
                EnumGameType type = (EnumGameType)options[0];
                List<Player> players = options[1] as List<Player>;
                string hostGameInfo = options[2] as string;
                string clientGameInfo = options[3] as string;
                //wizardNewGame.Close();             

                switch (type){
                    case EnumGameType.Single:
                    case EnumGameType.Multiple:
                        this.StartLocalGame(type, players);
                        break;
                    case EnumGameType.Host:
                        this.StartHostGame(players, hostGameInfo);
                        break;
                    case EnumGameType.Client:
                        this.StartClientGame(players, clientGameInfo);
                        break;
                    default:
                        break;
                }

                //wizardNewGame.Close();
            }
        }

        private void miOption_Click(object sender, EventArgs e) {
            OptionForm dlg = new OptionForm();
            DialogResult result = dlg.ShowDialog();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (null != m_game) {
                m_game.Dispose();
                m_game = null;
            }
            if (null != m_serverGame) {
                m_serverGame.Dispose();
                m_serverGame = null;
            }
        }

        private void OnAllMenu_Click(object sender, EventArgs e) {
            if (null != m_game && m_game.Status == EnumGameStatus.Running)
                m_game.Pause();
        }

        #endregion

        #region Game methods

        private void StartLocalGame(EnumGameType type, IList<Player> players) {
            LocalGame game = GameFactory.CreateGame(type) as LocalGame;
            game.Container = m_gameContainer;
            foreach (Player player in players) {
                this.InitPlayer(player);
                game.AddPlayer(player);
            }

            if (null != m_game)
                m_game.Dispose();

            m_game = game;
            game.New();
            this.Refresh();
            game.Start();
        }

        private void StartHostGame(List<Player> players, string hostGameInfo) {
            // host game info format : name={0},ip={1},port={2},max_players={3}
            string[] info = hostGameInfo.Split(new char[] { ',', '=' });

            Debug.Assert(info.Length == 8 && players != null && players.Count > 0 
                && !string.IsNullOrEmpty(hostGameInfo));

            int port = Convert.ToInt32(info[5]);
            int max = Convert.ToInt32(info[7]);
            Debug.Assert(max >= 2 && max < GameSetting.LIMITED_MAX_PLAYERS);

            if (null != m_serverGame)
                m_serverGame.Dispose();
            m_serverGame = GameFactory.CreateGame(EnumGameType.Host) as ServerGame;
            InitServerCommands();
            m_serverGame.PlayerJoined += new EventHandler<PlayerEventArgs>(ServerGame_PlayerJoined);
            m_serverGame.PlayerLeaved += new EventHandler<PlayerEventArgs>(ServerGame_PlayerLeaved);
            m_serverGame.PlayerPrepared += new EventHandler<PlayerEventArgs>(ServerGame_PlayerPrepared);
            m_serverGame.ServicePort = port;
            m_serverGame.MaxPlayers = max;
            m_serverGame.StartService();

            // A host is also a client when hosting a server game.
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(info[3]), port);
            if (null != m_game)
                m_game.Dispose();
            ClientGame game = (m_game = GameFactory.CreateGame(EnumGameType.Client) as TetrisGame) as ClientGame;
            game.Joined += new EventHandler<PlayerEventArgs>(ClientGame_Joined);
            game.Left += new EventHandler<PlayerEventArgs>(ClientGame_Left);
            game.Left += new EventHandler<PlayerEventArgs>(ClientGame_Left);
            game.PlayerJoined += new EventHandler<PlayerEventArgs>(ClientGame_PlayerJoined);
            game.GameElapsed += new EventHandler(ClientGame_GameElapsed);
            game.Container = m_gameContainer;

            Player player = players[0]; // first player as local player
            player.Name = info[1];
            player.HostName = Dns.GetHostName();
            player.PlayFiled = new PlayPanel(true);
            this.InitPlayer(player);

            game.AddPlayer(player);
            game.Connect("localhost", port);

            this.Text = this.Text + " - Server";
            this.Refresh();

            //WaitForClientForm dlg = new WaitForClientForm();
            //DialogResult result = dlg.ShowDialog();
            //if (result == DialogResult.OK) {
            //    m_serverGame.Start();
            //} else {
            //    game.Disconnect();
            //    m_serverGame.Stop();
            //    game.Dispose();
            //    m_serverGame.Dispose();
            //    m_game = null;
            //    m_serverGame = null;
            //    return;
            //}

            
        }

        private void StartClientGame(List<Player> players, string clientGameInfo) {
            // client game info format : name={0},server_ip={1},server_port={2}
            string[] info = clientGameInfo.Split(new char[] { ',', '=' });

            Debug.Assert(info.Length == 6 && players != null && players.Count > 0
                && !string.IsNullOrEmpty(clientGameInfo));

            if (string.IsNullOrEmpty(info[3]) || string.IsNullOrEmpty(info[5])) {
                MessageBox.Show("You did not specify the correct host information.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (null != m_game)
                m_game.Dispose();
            ClientGame game = (m_game = GameFactory.CreateGame(EnumGameType.Client) as TetrisGame) as ClientGame;
            game.Container = m_gameContainer;
            game.Joined += new EventHandler<PlayerEventArgs>(ClientGame_Joined);
            game.Left += new EventHandler<PlayerEventArgs>(ClientGame_Left);
            game.PlayerJoined += new EventHandler<PlayerEventArgs>(ClientGame_PlayerJoined);
            game.PlayerLeft += new EventHandler<PlayerEventArgs>(ClientGame_PlayerLeft);
            game.GameElapsed += new EventHandler(ClientGame_GameElapsed);

            Player player = players[0]; // first player as local player
            player.Name = info[1];
            player.PlayFiled = new PlayPanel(true);
            this.InitPlayer(player);

            game.AddPlayer(player);
            game.Connect(info[3], Convert.ToInt32(info[5]));

            this.Text = this.Text + " - Client";
            this.Refresh();
        }

        private void ShowGameInfo() {
            if (null == m_game || null == m_game.Players)
                return;

            foreach (Player player in m_game.Players.Values) {
                player.InfoPanel.SetString("name", player.Name,
                    new Font(FontFamily.GenericSansSerif, 8),
                    Brushes.Gray, 5, 5, 0, 0);
            }
        }

        public override void Refresh() {
            base.Refresh();
            if (null == m_game)
                return;

            PlayPanel panel = null;
            InfoPanel info = null;
            int padX = 20, padY = 20;

            int i = 0;
            foreach (Player player in m_game.Players.Values) {
                info = player.InfoPanel;
                if (null != info) {
                    info.Show(i * (info.Width + padX) + padX, padY + 20);
                }

                panel = player.PlayFiled;
                if (null != panel) {
                    panel.Show(i * (panel.Width + padX) + padX, info.Bottom + padY);
                }

                i++;
            }

            if (m_game.GameType == EnumGameType.Client || m_game.GameType == EnumGameType.Host) {

                if (m_networkGameInfoPanel == null)
                    m_networkGameInfoPanel = new InfoPanel(m_gameContainer);

                // adjust NetworkGameInfoPanel size depends on last panel
                info = m_networkGameInfoPanel;
                info.Width = panel.Right - padX;
                info.Height = 100;
                info.Show(padX, panel.Bottom + padY);

                this.Height = info.Bottom + padY + 20;
                this.Width = info.Right + padX + 10;

            } else if (null != panel) {
                if (null != m_networkGameInfoPanel) {
                    m_networkGameInfoPanel.Dispose();
                    m_networkGameInfoPanel = null;
                }
                // adjust form size depends on last panel.
                this.Height = panel.Bottom + padY + 40;
                this.Width = panel.Right + padX + 10;
            }

            this.ShowGameInfo();

            this.Invalidate();

        }

        #endregion

        #region Server Game Events

        void ServerGame_PlayerLeaved(object sender, PlayerEventArgs e) {
            if (m_serverGame.Players.Count == 1) {
                m_serverGame.Stop();
                m_serverGame.PlayerJoined -= (ServerGame_PlayerJoined);
                m_serverGame.PlayerLeaved -= (ServerGame_PlayerLeaved);
            }

            this.Refresh();
        }

        void ServerGame_PlayerJoined(object sender, PlayerEventArgs e) {
            Player player = e.Player;
            //TODO: uses skin
            m_networkGameInfoPanel.SetString("info", string.Format(
                "Player {0} joined. Waiting for rest client players ({1}/{2}) ...", 
                    player.Name, m_serverGame.Players.Count, m_serverGame.MaxPlayers),
                new Font("MSYH", 8), Brushes.CadetBlue, 5, 5, 0, 0);
            //if (m_game.Players.Count < m_serverGame.Players.Count) {
            //    this.InitPlayer(player);
            //    m_game.AddPlayer(player);
            //}

            this.Refresh();
        }

        void ServerGame_PlayerPrepared(object sender, PlayerEventArgs e) {
            if (m_serverGame.Players.Count == m_serverGame.MaxPlayers) {
                if (m_serverGame.IsReady) {
                    m_serverGame.Start();
                    //break;
                }
                //DateTime dt = DateTime.Now;
                //while (true) {

                //    if (DateTime.Now.Subtract(dt).Seconds > 15) {
                //        // time out
                //        MessageBox.Show("Game starting time out.");
                //        m_serverGame.Stop();
                //        break;
                //    }

                //    System.Threading.Thread.Sleep(0);
                //}
            }
        }

        #endregion

        #region Client Game Events

        void ClientGame_Joined(object sender, PlayerEventArgs e) {
            ClientGame game = m_game as ClientGame; //sender as ClientGame;
            Player player = e.Player;
            if (null == game || null == player || null == player.InfoPanel)
                return;
            
            //TODO: uses skin
            player.InfoPanel.SetString("info", "You have joined the game. Waiting for other players...",
                 new Font("MSYH", 8), Brushes.Gray, 5, player.InfoPanel.Height - 20, 0, 0);

            this.Refresh();
        }

        void ClientGame_Left(object sender, PlayerEventArgs e) {
            ClientGame game = m_game as ClientGame; //sender as ClientGame;
            Player player = e.Player;
            if (null == game || null == player || null == player.InfoPanel)
                return;
            //TODO: uses skin
            player.InfoPanel.SetString("info", "You have left the game.",
                 new Font("MSYH", 8), Brushes.Gray, 5, player.InfoPanel.Height - 20, 0, 0);
            game.Stop();
            game.Joined -= (ClientGame_Joined);
            game.Left -= (ClientGame_Left);
            game.PlayerJoined -= (ClientGame_PlayerJoined);
            game.PlayerLeft -= (ClientGame_PlayerLeft);
            game.Dispose();
            game = null;

            this.Refresh();
        }

        void ClientGame_PlayerJoined(object sender, PlayerEventArgs e) {
            ClientGame game = m_game as ClientGame; //sender as ClientGame;
            Player player = e.Player;
            if (null == game || null == player)
                return;

            this.InitPlayer(player);

            //TODO: uses skin
            player.InfoPanel.SetString("info",
                string.Format("{0} has joined the game. Waiting for other players...", player.Name),
                new Font("MSYH", 8), Brushes.Gray, 5, player.InfoPanel.Height - 20, 0, 0);

            this.Refresh();
        }

        void ClientGame_PlayerLeft(object sender, PlayerEventArgs e) {
            ClientGame game = m_game as ClientGame; //sender as ClientGame;
            Player player = e.Player;
            if (null == game || null == player || null == player.InfoPanel)
                return;

            //TODO: uses skin
            player.InfoPanel.SetString("info", string.Format("{0} has left the game.", player.Name),
                 new Font("MSYH", 8), Brushes.Gray, 5, player.InfoPanel.Height - 20, 0, 0);

            this.Refresh();
        }

        void ClientGame_GameElapsed(object sender, EventArgs e) {
            this.ShowGameInfo();
        }

        #endregion

        #region Private methods

        private void InitServerCommands() {
            ICommand command = new Command("NEXT", "NEXT", 
                new CommandHandler(this.CreatShape));
            m_serverGame.Commands.Add(command.ID as string, command); 
        }

        private Random m_rnd = null;
        private object CreatShape(params object[] parameters) {
            int min = 4, max = 4, types = 7; // should be gotten from setting.
            if (null == m_rnd)
                m_rnd = new Random(DateTime.Now.Millisecond);
            int blocks = m_rnd.Next(min, max);
            int type = m_rnd.Next(types - 1);
            return string.Format("{0}-{1}", blocks, type);
        }

        private void LoadSkins() {
            this.Text = m_skin.GetString("app_name");
            miGame.Text = m_skin.GetShortCutString("mn_game");
            miNew.Text = m_skin.GetShortCutString("mn_new");
            miExit.Text = m_skin.GetShortCutString("mn_exit");
            miSetting.Text = m_skin.GetShortCutString("mn_setting");
            miHelp.Text = m_skin.GetShortCutString("mn_help");
            miAbout.Text = m_skin.GetShortCutString("mn_about");
            if (miHelp.Text.Length <= 0)
                miHelp.Text = "&Help";
            if (miAbout.Text.Length <= 0)
                miAbout.Text = "&About";
        }

        private void InitPlayer(Player player) {
            Debug.Assert(null != player.PlayFiled);
            //if (null == player.PlayFiled) {
            //    if (m_game.GameType == EnumGameType.Client ||
            //        m_game.GameType == EnumGameType.Host)
            //        player.PlayFiled = new PlayPanel(true);
            //    else
            //        player.PlayFiled = new PlayPanel(false);
            //    m_gameContainer.Controls.Add(player.PlayFiled);
            //}
            
            if (null == player.InfoPanel)
                player.InfoPanel = new InfoPanel(m_gameContainer);
            player.InfoPanel.AutoRefresh = true;
            player.InfoPanel.Width = player.PlayFiled.Width;
            player.InfoPanel.Height = 100;
        }

        #endregion

        #region fields

        protected GameSetting m_setting = GameSetting.Instance;
        protected Skins m_skin = Skins.Instance;
        private TetrisGame m_game = null;
        private ServerGame m_serverGame = null; // only used for host game.
        private Form m_gameContainer = null;
        private InfoPanel m_networkGameInfoPanel = null;

        #endregion

    }
}