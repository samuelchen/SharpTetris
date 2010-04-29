
//=======================================================================
// <copyright file="Setting.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Service {

    /// <summary>
    /// The setting service
    /// </summary>
    public class Setting : IDisposable {

        public Setting() {
            m_xmldoc = new XmlDocument();
            m_general = new Dictionary<string,string>();

            string logName = AppDomain.CurrentDomain.ApplicationIdentity.FullName;
            m_listener = new EventLogTraceListener(logName);
            Trace.Listeners.Add(m_listener);
        }

        #region properties

        /// <summary>
        /// Setting singleton instance.
        /// </summary>
        public static Setting Instance {
            get {
                if (null == m_instance)
                    m_instance = new Setting();
                return m_instance;
            }
            set {
                m_instance = value;
            }
        }

        /// <summary>
        /// Setting file full path
        /// </summary>
        public virtual string SettingPath { get; set; }

        /// <summary>
        /// Default base path (directory only) of setting file.
        /// Initialized as application path.
        /// </summary>
        protected virtual string DefaultPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }

        #endregion

        /// <summary>
        /// Load setting from a xml file. Override this to load your own setting.
        /// </summary>
        /// <param name="filepath">the file path</param>
        public virtual bool Load(string filepath) {
            try {
                if (Path.IsPathRooted(filepath)) {
                    m_xmldoc.Load(filepath);
                } else {
                    string path = string.Format(@"{0}\{1}", this.DefaultPath, filepath);
                    m_xmldoc.Load(path);
                }
                this.SettingPath = filepath;

                LoadGeneral();
 
            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                Trace.TraceError("Fail to load setting.\n{0}", err);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save setting to file specified in <code><see cref="SettingPath"/></code> property.
        /// It's often the loaded setting file.
        /// </summary>
        /// <returns></returns>
        public virtual bool Save() {
            return Save(this.SettingPath);
        }

        /// <summary>
        /// Save setting to specified path.Override this to save your own setting.
        /// </summary>
        /// <param name="filepath">Setting file name.</param>
        /// <returns></returns>
        public virtual bool Save(string filepath) {
            string path = string.Empty;
            try {
                if (Path.IsPathRooted(filepath))
                    path = filepath;
                else
                    path = string.Format(@"{0}\{1}", this.DefaultPath, filepath);

                SaveGeneral();
                
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

        /// <summary>
        /// Clear settings. Override this to save your own setting.
        /// </summary>
        public virtual void Clear() {
            m_general.Clear();
        }

        #endregion


        #region General setting methods

        /// <summary>
        /// Save general settings.
        /// </summary>
        protected virtual void SaveGeneral() {

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

        protected virtual void LoadGeneral() {

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
        protected virtual void Set(string name, string value) {
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
        protected virtual string Get(string name) {
            if (string.IsNullOrEmpty(name))
                return null;

            if (!m_general.ContainsKey(name))
                m_general.Add(name, string.Empty);

            return m_general[name];
        }

        #endregion

        #region Fields
        protected XmlDocument m_xmldoc;
        protected TraceListener m_listener;
        protected Dictionary<string, string> m_general;
        protected static Setting m_instance;
        #endregion

        #region IDisposable Members

        private bool _disposed = false;

        public virtual void Dispose() {
            if (_disposed)
                return;

            this.Clear();
            m_xmldoc = null;
            Trace.Listeners.Remove(m_listener);
            m_listener.Dispose();

            _disposed = true;
        }

        #endregion
    }
}
