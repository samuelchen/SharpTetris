
//=======================================================================
// <copyright file="WizPageSummary.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : A wizard page to the wizard summary.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Game;
using Net.SamuelChen.Tetris.Skin;

namespace Net.SamuelChen.Tetris {
    public partial class WizPageSummary : UserControl, IWizardPage {

        protected Skins m_skin = Skins.Instance;
        //public IList<object> Options { get; protected set; }

        public WizPageSummary() {
            InitializeComponent();
        }

        #region IWizardPage Members

        public bool Show(IList<object> options) {
            if (null == options || options.Count < 2)
                return false;
            //this.Options = options;
            
            EnumGameType type = (EnumGameType)options[0];
            List<Player> players = options[1] as List<Player>;

            StringBuilder sb = new StringBuilder();
            string[] summary = new string[4];

            summary[2] = "    N/A";
            summary[3] = "    N/A";

            summary[0] = string.Format("    {0}", type.ToString());

            if (type == EnumGameType.Multiple) {
                foreach (Player player in players) {
                    if (null == player)
                        continue;
                    sb.AppendLine( string.Format("    {0}: {1}\t{2}: {3}",
                        m_skin.GetString("wiz_muti_col_player"), player.Name,
                        m_skin.GetString("wiz_muti_col_controller"),
                        (null == player.Controller ? "N/A" : player.Controller.Name)
                        ));
                }
                summary[1] = sb.ToString();

            } else {

                Player player = null;
                if (null != players && players.Count > 0 && null != (player = players[0])) {

                    summary[1] = string.Format("    {0}: {1}\t{2}: {3}",
                        m_skin.GetString("wiz_muti_col_player"), player.Name,
                        m_skin.GetString("wiz_muti_col_controller"),
                        (null == player.Controller ? "-" : player.Controller.Name)
                        );
                }


                if (type == EnumGameType.Single) {

                } else if (type == EnumGameType.Host) {
                    if (options.Count < 3)
                        return false;

                    // host info format : name={0},ip={1},port={2},max_players={3}
                    string hostGameInfo = options[2] as string;
                    string[] info = hostGameInfo.Split(new char[] { ',', '=' });
                    string txtInfo = string.Format(m_skin.GetString("wiz_host_info"), info[3], info[5], info[7]);
                    summary[2] = txtInfo;

                } else if (type == EnumGameType.Client) {
                    if (options.Count < 4)
                        return false;

                    // client info format : name={0},server_ip={1},server_port={2}
                    string clientGameInfo = options[3] as string;
                    string[] info = clientGameInfo.Split(new char[] { ',', '=' });
                    string txtInfo = string.Format(m_skin.GetString("wiz_client_info"), info[3], info[5]);
                    summary[3] = txtInfo;
                }
            }
            txtSummary.Text = string.Format(m_skin.GetString("wiz_summary"),
                summary[0], summary[1], summary[2], summary[3]);
            
            this.Dock = DockStyle.Fill;
            this.Visible = true;
            this.BringToFront();

            return true;
        }

        public object GetValue() {
            return null;
        }

        public new void Hide() {
            base.Hide();
            this.Visible = false;
        }

        #endregion
    }
}
