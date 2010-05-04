
//=======================================================================
// <copyright file="NetworkContent.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Text;
using System.Runtime.Serialization;

namespace Net.SamuelChen.Tetris.Network {

    public enum EnumNetworkContentType {
        Null = 0,
        Bianary,
        String,
    }
    
    [Serializable]
    public class NetworkContent : MarshalByRefObject  {

        public NetworkContent(EnumNetworkContentType type, object data) {
            if (null == data)
                this.Type = EnumNetworkContentType.Null;
            else
                this.Type = type;

            if (type == EnumNetworkContentType.Null)
                this.Data = null;
            else
                this.Data = data;

            this.Encoding = Encoding.Unicode; //default encoding
        }

        public NetworkContent(EnumNetworkContentType type, object data, Encoding encoding)
            : this(type, data) {
            this.Encoding = encoding;
        }

        public EnumNetworkContentType Type { get; protected set; }
        public Encoding Encoding { get; set; }
        public object Data { get; protected set; }

        public string GetString() {
            switch (this.Type) {
                case EnumNetworkContentType.String:
                    return this.Data as string;
                case EnumNetworkContentType.Bianary:
                    return this.Encoding.GetString(this.Data as byte[]);
                case EnumNetworkContentType.Null:
                    return null;
                default:
                    break;
            }

            return null;
        }

        public byte[] GetBinary() {
            switch (this.Type) {
                case EnumNetworkContentType.String:
                    return this.Encoding.GetBytes(this.Data as string);
                case EnumNetworkContentType.Bianary:
                    return this.Data as byte[];
                case EnumNetworkContentType.Null:
                    return null;
                default:
                    break;
            }
            return null;
        }


        public override string ToString() {
            return this.GetString();
        }
    }
}
