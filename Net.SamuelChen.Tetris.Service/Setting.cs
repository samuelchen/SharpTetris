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
            m_general = new Dictionary<string,string>();

            string logName = "#Tetris";
//            string logSource = "#Tetris";
//            if (!EventLog.Exists(logName)){
//                EventLog.CreateEventSource (logSource, eventLogName);
//Console.WriteLine ("CreatingEventSource");
//}
            //EventLog log = new EventLog(logName);
            //log.ModifyOverflowPolicy(OverflowAction.OverwriteOlder, 1);
            m_listener = new EventLogTraceListener(logName);
            Trace.Listeners.Add(m_listener);
        }

        #region properties

        /// <summary>
        /// Setting singleton instance
        /// </summary>
        public static Setting Instance {
            get {
                if (null == m_instance)
                    m_instance = new Setting();
                return m_instance;
            }
        }

        /// <summary>
        /// Setting file full path
        /// </summary>
        public string SettingPath { get; set; }

        /// <summary>
        /// Default base path (directory only) of setting file.
        /// Initialized as application path.
        /// </summary>
        protected string DefaultPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }

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

                LoadGeneral();
                LoadKeymaps();
                //LoadPlayers();
                

            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("Fail to save setting.\n{0}", err);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save setting to file specified in <code><see cref="SettingPath"/></code> property.
        /// It's often the loaded setting file.
        /// </summary>
        /// <returns></returns>
        public bool Save() {
            return Save(this.SettingPath);
        }

        /// <summary>
        /// Save setting to specified path.
        /// </summary>
        /// <param name="filepath">Setting file name.</param>
        /// <returns></returns>
        public bool Save(string filepath) {
            string path = string.Empty;
            try {
                if (Path.IsPathRooted(filepath))
                    path = filepath;
                else
                    path = string.Format(@"{0}\{1}", this.DefaultPath, filepath);

                SaveGeneral();
                SaveKeymaps();
                //SavePlayers();
                
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

        #region Clean up methods

        public void Clear() {
            ClearKeymaps();
        }

        public void ClearKeymaps() {
            m_keymaps.Clear();
        }

        public void ClearPlayers() {
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

        #region General setting methods

        /// <summary>
        /// Save general settings.
        /// </summary>
        protected void SaveGeneral() {

            XmlNode node, general;
            XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("general");

            if (null == nodeList && nodeList.Count == 0)
                general = m_xmldoc.CreateElement("general");
            else
                general = nodeList[0];

            foreach (KeyValuePair<string, string> item in m_general) {
                string name = item.Key;
                string value = item.Value;

                node = general[name];

                if (null == node) {
                    node = m_xmldoc.CreateElement(name);
                    general.AppendChild(node);
                }

                node.InnerText = value;
            }
        }

        protected void LoadGeneral() {

            XmlNode general;
            XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("general");

            if (null == nodeList && nodeList.Count == 0)
                general = m_xmldoc.CreateElement("general");
            else
                general = nodeList[0];

            m_general.Clear();
            foreach (XmlNode node in general.ChildNodes){
                m_general.Add(node.Name, node.InnerText);
            }
        }

        /// <summary>
        /// Set value for a general setting.
        /// </summary>
        /// <param name="name">the setting name</param>
        /// <param name="value">the value to be set</param>
        protected void Set(string name, string value) {
            if (string.IsNullOrEmpty(name) || null == value)
                return;

            if (m_general.ContainsKey(name))
                m_general[name] = value;
            else
                m_general.Add(name, value);
        }

        /// <summary>
        /// Get a general setting value.
        /// </summary>
        /// <param name="name">the setting name</param>
        /// <returns></returns>
        protected string Get(string name) {
            if (string.IsNullOrEmpty(name))
                return null;

            if (!m_general.ContainsKey(name))
                m_general.Add(name, string.Empty);

            return m_general[name];
        }

        #endregion

        #region Fields
        protected XmlDocument m_xmldoc;
        protected XmlNode m_skin = null;
        protected Dictionary<string, Dictionary<string, string>> m_keymaps;
        private TraceListener m_listener;
        private Dictionary<string, string> m_general;

        private static Setting m_instance;
        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (null != m_keymaps)
                m_keymaps.Clear();
            m_keymaps = null;
            m_skin = null;
            m_xmldoc = null;
            m_listener.Dispose();
        }

        #endregion
    }
}
