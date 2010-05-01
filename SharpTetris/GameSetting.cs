using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Controller;

namespace Net.SamuelChen.Tetris {

    /// <summary>
    /// The setting service
    /// </summary>
    public class GameSetting : Setting, IDisposable {
        public const int	DEFAULT_MAX_PLAYERS = 4;
        public const int	LIMITED_MAX_PLAYERS = 8;

        public GameSetting() : base() {
            m_keymaps = new Dictionary<string, Dictionary<string, string>>();
        }

        #region properties

        public new static GameSetting Instance {
            get { return Setting.Instance as GameSetting; }
            set { Setting.Instance = value; }
        }

        /// <summary>
        /// Skin name. It's also the folder name which contains the skin.
        /// </summary>
        public string Skin {
            get { return this.Get("skin"); }
            set { this.Set("skin", value); }
        }

        /// <summary>
        /// Defined Controller IDs.
        /// </summary>
        public List<string> ControllerIds {
            get {
                List<string> ctrlrs = new List<string>(m_keymaps.Count);
                foreach (string controllerId in m_keymaps.Keys) {
                    ctrlrs.Add(controllerId);
                }
                return ctrlrs;
            }
        }
        
        /// <summary>
        /// The default player name in predefiend options.
        /// </summary>
        public string DefaultPlayerName {
            get { return this.Get("default_player"); }
            set { this.Set("default_player", value); }
        }

        /// <summary>
        /// The default player controller guid in predefiend options.
        /// </summary>
        public string DefaultPlayerControllerId {
            get { return this.Get("default_controller"); }
            set { this.Set("default_controller", value); }
        }

        /// <summary>
        /// Network port for network game.
        /// </summary>
        public string Port {
            get { return this.Get("port"); }
            set { this.Set("port", value); }
        }

        public int MaxPlayers {
            get {
                string s = this.Get("max_players");
                int n = Convert.ToInt32(s);
                if (n <= 0 || n > LIMITED_MAX_PLAYERS) {
                    n = DEFAULT_MAX_PLAYERS;
                    this.Set("max_players", n.ToString());
                }
                return n;
            }
            set {
                int n = value;
                if (n <= 0 || n > LIMITED_MAX_PLAYERS) {
                    throw new ArgumentOutOfRangeException(string.Format(
                        "MaxPlayers is out of range (1 - {0}).", LIMITED_MAX_PLAYERS));
                }
                this.Set("max_players", n.ToString());
            }
        }

        public ControllerFactory ControllerFactory { get; set; }

        #endregion

        /// <summary>
        /// Load setting from a xml file.
        /// </summary>
        /// <param name="filepath">the file path</param>
        public override bool Load(string filepath) {
            bool rc = base.Load(filepath);
            LoadKeymaps();

            return rc;
        }

        /// <summary>
        /// Save setting to file specified in <code><see cref="SettingPath"/></code> property.
        /// It's often the loaded setting file.
        /// </summary>
        /// <returns></returns>
        public override bool Save() {
            return this.Save(this.SettingPath);
        }

        /// <summary>
        /// Save setting to specified path.
        /// </summary>
        /// <param name="filepath">Setting file name.</param>
        /// <returns></returns>
        public override bool Save(string filepath) {
            SaveKeymaps();
            return base.Save(filepath);
        }

        #region Clean up methods

        public override void Clear() {
            base.Clear();
            ClearKeymaps();
        }

        public void ClearKeymaps() {
            m_keymaps.Clear();
        }

        #endregion

        #region Keymap methods

        #region Public
        /* I think this is not required for setting.
         * 
        public string GetKeymap(string id, string keyname) {
            if (null == id || null == keyname || !m_keymaps.ContainsKey(id))
                return null;
            Dictionary<string, string> keymap = m_keymaps[id];
            if (null == keymap || !keymap.ContainsKey(keyname))
                return null;
            return keymap[keyname];
        }
        */

        public Dictionary<string, string> GetKeymap(string controllerId) {
            if (null == controllerId || !m_keymaps.ContainsKey(controllerId))
                return null;
            string id = controllerId.ToLower();
            return m_keymaps[id];
        }

        public void SetKeymap(string controllerId, Dictionary<string, string> keymap) {
            if (null == controllerId)
                return;
            string id = controllerId.ToLower();
            m_keymaps.Remove(id);
            m_keymaps.Add(id, keymap);
        }

        public Dictionary<string, string> RemoveKeymap(string controllerId) {
            if (null == controllerId || !m_keymaps.ContainsKey(controllerId))
                return null;
            string id = controllerId.ToLower();
            Dictionary<string, string> keymap = m_keymaps[id];
            m_keymaps.Remove(id);
            return keymap;
        }

        #endregion

        #region Protected / Private
        /// <summary>
        /// Load controller key maps
        /// </summary>
        /// <returns></returns>
        protected bool LoadKeymaps() {
            Dictionary<string, string> keymap;
            XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("controller");
            if (null == nodeList)
                return false;

            try {
                m_keymaps.Clear();
                foreach (XmlElement elmt in nodeList) {
                    keymap = new Dictionary<string, string>();
                    XmlNode node = elmt.FirstChild;
                    while (null != node) {
                        keymap.Add(node.Attributes["action"].Value.ToLower(), node.InnerText);
                        node = node.NextSibling;
                    }

                    m_keymaps.Add(elmt.Attributes["id"].Value.ToLower(), keymap);
                }
            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError(err.Message);
                return false;
            }
            return true;
        }

        protected void SaveKeymaps() {
            Dictionary<string, string> keymap;
            //IEnumerator theEnumer = m_keymaps.GetEnumerator();
            //KeyValuePair<string, Dictionary<string, string>> item;
            XmlNode nodeControllers;
            string id;

            // clean up
            nodeControllers = m_xmldoc.GetElementsByTagName("controllers")[0];
            if (null != nodeControllers)
                nodeControllers.RemoveAll();

            foreach (KeyValuePair<string, Dictionary<string, string>> item in m_keymaps) {

                id = item.Key.ToLower();
                keymap = item.Value;

                XmlNode nodeCtrlr = m_xmldoc.CreateElement("controller");
                XmlAttribute attrCtrlr = m_xmldoc.CreateAttribute("id");
                attrCtrlr.Value = id;
                nodeCtrlr.Attributes.Append(attrCtrlr);

                //KeyValuePair<string, string> childItem;
                //IEnumerator childEnumer = keymap.GetEnumerator();
                string action, keys;

                foreach (KeyValuePair<string, string> childItem in keymap) {
                    //childItem = childEnumer.Current as KeyValuePair<string, string>;
                    action = childItem.Key.ToLower();
                    keys = childItem.Value;

                    XmlNode node = m_xmldoc.CreateElement("key");
                    XmlAttribute attr = m_xmldoc.CreateAttribute("action");
                    attr.Value = action;
                    node.Attributes.Append(attr);
                    node.InnerText = keys;
                    nodeCtrlr.AppendChild(node);
                }
                nodeControllers.AppendChild(nodeCtrlr);
            }
#if DEBUG
            Trace.TraceInformation(nodeControllers.OuterXml);
#endif
        }

        //public int GetFirstNoneUsedCtrl(){
        //    XmlNodeList nodeList = this.GetElementsByTagName("keymap");
        //    if (null != nodeList) {
        //        System.Collections.IEnumerator theEnumer = nodeList.GetEnumerator();
        //        XmlElement elmt;
        //        for (int i=0; i<nodeList.Count; i++) {
        //            theEnumer.MoveNext();
        //            elmt = (XmlElement)theEnumer.Current;
        //            if ("" == elmt.GetAttribute("no")){
        //                elmt.SetAttribute("no", i.ToString());
        //                return i;
        //            }
        //        }
        //    }
        //    return -1;
        //}

        #endregion

        #endregion // Keymap methods

        #region Fields
        protected XmlNode m_skin = null;
        protected Dictionary<string, Dictionary<string, string>> m_keymaps;
        protected TraceListener m_fileListener;
        #endregion

        #region IDisposable Members

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            this.Clear();
            m_skin = null;
            base.Dispose();

            _disposed = true;
        }

        #endregion
    }
}
