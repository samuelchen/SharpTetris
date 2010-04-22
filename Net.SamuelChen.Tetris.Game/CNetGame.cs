using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;

namespace Net.SamuelChen.Tetris.Game
{
	public delegate void ConnectHandler(object client, CConnectEventArgs e);
	public delegate void HostHandler(object host, CConnectEventArgs e);

	public class CConnectEventArgs : System.EventArgs {
		public CConnectEventArgs() {}
		public CConnectEventArgs(string dat) {
		}
	}


	/// <summary>
	/// CNetGame ��ժҪ˵����
	/// </summary>
	public class CNetGame : Game
	{
		public event HostHandler		Hosted;
		public event ConnectHandler		Connected;

		public CNetGame(System.Windows.Forms.Form form) : base(form){}

		#region ������غ���
		public void Host() {
			Thread thHost = new Thread(new ThreadStart(this.HostProc));
		}

		private void HostProc() {
			// Set the TcpListener on port 13000.
			Int32 port = 13000;
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");

			TcpListener host = new TcpListener(localAddr, port);
			byte[] buf = new byte[256];
			string pack = null;

			host.Start();

			// �ѽ��������¼�
			if (null != Hosted)
				Hosted(host, new CConnectEventArgs(null));

			while(true) {
				// �ȴ�����...
				TcpClient client = host.AcceptTcpClient();

				pack = null;

				NetworkStream stream = client.GetStream();
				Int32 i;

				// ѭ��ȡ����
				while((i = stream.Read(buf, 0, buf.Length))!=0) {   
					// ���뵽ASCII��.
					string dat = System.Text.Encoding.ASCII.GetString(buf, 0, i);

					// ��������
					pack += dat;

					Byte[] msg = System.Text.Encoding.ASCII.GetBytes(dat);

					// ����
					stream.Write(msg, 0, msg.Length);
				}

				// �����¼�
				if (null != Connected) 
					OnConnected(client, pack);
			}
		}

		public void OnConnected(object sender, string dat) {
			XmlDocument xml = new XmlDocument();
            xml.LoadXml(dat);
			XmlNodeList nodeList;
			//System.Collections.IEnumerator enumerator;
			if (null != (nodeList = xml.GetElementsByTagName("player"))) {
				XmlElement elmt = (XmlElement)nodeList[0];
				switch (elmt.GetAttribute("action")) {
					case "create" :
						Player player = new Player();
						player.Name = elmt.GetAttribute("name");
						player.IP.Address = IPAddress.Parse(elmt.GetAttribute("ip"));
						player.IP.Port = Convert.ToInt32(elmt.GetAttribute("port"));
						this.AddPlayer(ref player);
						break;
					case "move" :
						break;
					default:
						break;
				}
				
			}else if ( null != (nodeList = xml.GetElementsByTagName("move"))) {

			}
			
			Connected(sender, new CConnectEventArgs(dat));
		}

		public void Connect(){
		}

		private void SearchHost(){
		}
		#endregion

	}

}
