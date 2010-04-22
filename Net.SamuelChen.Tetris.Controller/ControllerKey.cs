
//=======================================================================
// <copyright file="ControllerKey.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To define controller key related classes. 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Net.SamuelChen.Tetris.Controller {
    /// <summary>
    /// Represents a controller key.
    /// </summary>
    public class ControllerKey {
        public int Button = -1;

        public ControllerKey() {
        }

        public ControllerKey(int button) {
            this.Button = button;
        }

        public bool IsValid {
            get { return this.Button >= 0; }
        }

        public static ControllerKey FromString(string str) {
            int btn = -1;
            try {
                btn = Convert.ToInt32(str);
            } catch {
                str = str.ToLower();
                if (str == "axisx-")
                    btn = 101;
                else if (str == "axisx+")
                    btn = 102;
                else if (str == "axisy-")
                    btn = 103;
                else if (str == "axisy+")
                    btn = 104;
                else
                    btn = -1;
            }
            return new ControllerKey(btn);
        }

        public override string ToString() {
            string str = string.Empty;
            switch (Button) {
                case 101:
                    str = "AxisX-";
                    break;
                case 102:
                    str = "AxisX+";
                    break;
                case 103:
                    str = "AxisY-";
                    break;
                case 104:
                    str = "AxisY+";
                    break;
                case -1:
                    str = "-";
                    break;
                default:
                    str = Button.ToString();
                    break;
            }
        
            return str;
        }

        public bool EqualsTo(object obj) {
            ControllerKey key = obj as ControllerKey;
            if (null == key)
                return false;

            if (key.Button == this.Button)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// Key number map to action.
    /// </summary>
    public class ControllerKeyMap : Dictionary<string, ControllerKey[]> {
        public bool Contains(ControllerKey key) {
            ControllerKeyMap.Enumerator en = this.GetEnumerator();
            while (en.MoveNext()) {
                ControllerKey[] keys = en.Current.Value;
                if (null != keys && keys.Length > 0) {
                    for (int i = 0; i < keys.Length; i++) {
                        if (key.EqualsTo(keys[i]))
                            return true;
                    }
                }
            }
            return false;
        }

        public bool Delete(ControllerKey key) {
            ControllerKeyMap.Enumerator en = this.GetEnumerator();
            while (en.MoveNext()) {
                ControllerKey[] keys = en.Current.Value;
                if (null != keys && keys.Length > 0) {
                    for (int i = 0; i < keys.Length; i++) {
                        if (key.EqualsTo(keys[i])) {
                            string action = en.Current.Key;
                            List<ControllerKey> newKeys = new List<ControllerKey>(keys);
                            newKeys.RemoveAt(i);
                            this.Remove(action);
                            this.Add(action, newKeys.ToArray());
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public ControllerKeyMap Clone() {
            return new Dictionary<string, ControllerKey[]>(this) as ControllerKeyMap;
        }

        public Dictionary<string, string> ToStringDictionary() {
            Dictionary<string, string> newKeymap = new Dictionary<string, string>();
            ControllerKeyMap.Enumerator en = this.GetEnumerator();
            while (en.MoveNext()) {
                string action = en.Current.Key;
                StringBuilder sb = new StringBuilder();
                ControllerKey[] keys = en.Current.Value;
                if (null != keys && keys.Length > 0) {
                    for (int i = 0; i < keys.Length; i++) {
                        sb.Append(keys[i]);
                        sb.Append(", ");
                    }
                }
                newKeymap.Add(action, sb.ToString());
            }

            return newKeymap;
        }

        public static ControllerKeyMap FromStringDictionary(Dictionary<string, string> keymap) {
            ControllerKeyMap newKeymap = new ControllerKeyMap();
            Dictionary<string, string>.Enumerator en = keymap.GetEnumerator();
            while (en.MoveNext()) {
                string action = en.Current.Key;
                string keyStr = en.Current.Value;
                string[] keys = keyStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<ControllerKey> newKeys = new List<ControllerKey>();
                if (null != keys && keys.Length > 0) {
                    for (int i = 0; i < keys.Length; i++) {
                        ControllerKey key = ControllerKey.FromString(keys[i]);
                        newKeys.Add(key);
                    }
                }
                newKeymap.Add(action, newKeys.ToArray());
            }

            return newKeymap;
        }
    }

}
