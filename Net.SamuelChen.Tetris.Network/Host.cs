using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Network {
    public class Host {

        public const int DEFAULT_PORT = 9527;

        #region ctor

        public Host()
            : this(DEFAULT_PORT) {
            this.Init();
        }

        public Host(int port) {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            this.LocalEndPoint = new IPEndPoint(host.AddressList[0], port);
            this.Encoding = Encoding.Unicode;  //default encoding
            this.Name = host.HostName;
            this.Init();
        }

        public Host(int port, Encoding encoding)
            : this(port) {
            this.Encoding = encoding;
            this.Init();
        }

        protected virtual void Init() {
        }

        #endregion

        #region Properties

        public IPEndPoint LocalEndPoint { get; set; }
        public Encoding Encoding { get; protected set; }
        public string Name { get; set; }

        #endregion

        #region Data sending/receving Methods

        public static byte[] RecevieData(TcpClient client) {

            if (null == client || !client.Connected)
                return null;

            NetworkStream ns = client.GetStream();
            client.NoDelay = true;
            if (null == ns || !ns.CanRead)
                return null;

            byte[] buf = new byte[256];
            byte[] data = null;
            try {
                using (MemoryStream ms = new MemoryStream()) {

                    int n = 0;
                    while (ns.DataAvailable && (n = ns.Read(buf, 0, buf.Length)) != 0) {
                        ms.Write(buf, 0, n);
                    }
                    ms.Flush();

                    if (ms.Length > 0)
                        data = ms.ToArray();
                    ms.Close();
                }
            } catch (SocketException err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("\"{2}\" fails to receive data from \"{3}\". \n{0}\n{1}",
                    err.Message, err.StackTrace, client.Client.LocalEndPoint.ToString(),  
                    client.Client.RemoteEndPoint.ToString());
            }
            
            ns = null;
            return data;
        }

        public static bool SendData(TcpClient client, byte[] data) {
            if (null == client || !client.Connected || null == data)
                return false;

            NetworkStream ns = client.GetStream();
            client.NoDelay = true;
            if (null == ns || !ns.CanWrite)
                return false;

            try {
                ns.Write(data, 0, data.Length);
            } catch (SocketException err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("\"{2}\" fails to send data to {3}. \n{0}\n{1}",
                    err.Message, err.StackTrace, client.Client.LocalEndPoint.ToString(), 
                    client.Client.RemoteEndPoint.ToString());
            }

            return true;

        }

        #endregion
    }
}
