using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Service {

    /// <summary>
    /// The setting service
    /// </summary>
    public class Setting : IDisposable {
        //public const int	adMaxPlayerNum = 4;			// 最大控制器个数
        //public const int	adDefaultBlockNum = 4;		// 默认块数常量

        public Setting() {
            m_xmldoc = new XmlDocument();
            m_keymaps = new Dictionary<string, Dictionary<string, string>>();

            string logName = "#Tetris";
//            string logSource = "#Tetris";
//            if (!EventLog.Exists(logName)){
//                EventLog.CreateEventSource (logSource, eventLogName);
//Console.WriteLine ("CreatingEventSource");
//}
            //EventLog log = new EventLog(logName);
            //log.ModifyOverflowPolicy(OverflowAction.OverwriteOlder, 1);
            listener = new EventLogTraceListener(logName);
            Trace.Listeners.Add(listener);
        }

        #region properties

        public static Setting Instance {
            get {
                if (null == m_instance)
                    m_instance = new Setting();
                return m_instance;
            }
        }


        public string SettingPath { get; set; }
        protected string DefaultPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }


        public string Skin {
            get {
                return m_skin.InnerText;
            }
            set {
                m_skin.InnerText = value;
            }
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
        /// The default player name.
        /// </summary>
        public string DefaultPlayerName { get; set; }

        public string DefaultPlayerControllerId { get; set; }

        #endregion

        /// <summary>
        /// Load setting from a xml file.
        /// </summary>
        /// <param name="filepath">the file path</param>
        public bool Load(string filepath) {
            try {
                if (Path.IsPathRooted(filepath)) {
                    m_xmldoc.Load(filepath);
                } else {
                    string path = string.Format(@"{0}\{1}", this.DefaultPath, filepath);
                    m_xmldoc.Load(path);
                }
                this.SettingPath = filepath;

                XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("skin");
                if (null != nodeList && nodeList.Count > 0)
                    m_skin = nodeList[0];
                else {
                    m_skin = m_xmldoc.CreateElement("skin");
                    m_skin.InnerText = "default";
                }

                LoadKeymaps();
                LoadPlayers();

            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("Fail to save setting.\n{0}", err);
                return false;
            }

            return true;
        }

        public bool Save() {
            return Save(this.SettingPath);
        }
        public bool Save(string filepath) {
            string path = string.Empty;
            try {
                if (Path.IsPathRooted(filepath))
                    path = filepath;
                else
                    path = string.Format(@"{0}\{1}", this.DefaultPath, filepath);

                SaveKeymaps();
                SavePlayers();

                m_xmldoc.Save(path);

            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("Fail to save setting.\n{0}", err);
                return false;
            }
            return true;
        }

        public void Clear() {
            ClearKeymaps();
        }

        public void ClearKeymaps() {
            m_keymaps.Clear();
        }

        public void ClearPlayers() {
        }

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

        #region Player methods

        #region public
        

        #endregion

        #region protected / private
        protected void SavePlayers() {
            XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("player");
            XmlNode node;
            if (null == nodeList || 0 == nodeList.Count)
                node = m_xmldoc.AppendChild(m_xmldoc.CreateElement("player"));
            else
                node = nodeList[0];
            node.RemoveAll();
            XmlAttribute attr = m_xmldoc.CreateAttribute("controller");
            attr.Value = this.DefaultPlayerControllerId;
            node.Attributes.Append(attr);
            node.InnerText = this.DefaultPlayerName;
        }

        protected void LoadPlayers() {
            XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("player");
            if (null == nodeList || 0 == nodeList.Count)
                return;
            XmlNode node = nodeList[0];
            this.DefaultPlayerControllerId = node.Attributes["controller"].Value;
            this.DefaultPlayerName = node.InnerText;
        }

        #endregion

        #endregion // player methods

        #region Fields
        protected XmlDocument m_xmldoc;
        protected XmlNode m_skin = null;
        protected Dictionary<string, Dictionary<string, string>> m_keymaps;
        private static Setting m_instance;
        TraceListener listener;
        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (null != m_keymaps)
                m_keymaps.Clear();
            m_keymaps = null;
            m_skin = null;
            m_xmldoc = null;
            listener.Dispose();
        }

        #endregion
    }
}
