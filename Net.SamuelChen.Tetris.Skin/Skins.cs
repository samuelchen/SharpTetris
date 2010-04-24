
//=======================================================================
// <copyright file="Skins.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : To manage skins.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace Net.SamuelChen.Tetris.Skin {

    /// <summary>
    /// The skin service
    /// </summary>
    public class Skins : IDisposable {

        public Skins() {
            this.SkinPath = string.Format(@"{0}\skin\",
                AppDomain.CurrentDomain.BaseDirectory);
            m_xmldoc = new XmlDocument();
            m_images = new Dictionary<string, IList<Image>>();
            m_strings = new Dictionary<string, string>();
        }

        public string SkinPath { get; set; }

        public static Skins Instance {
            get {
                if (null == m_instance)
                    m_instance = new Skins();
                return m_instance;
            }
        }
        /// <summary>
        /// Load setting from a xml file.
        /// </summary>
        /// <param name="filepath">the file path</param>
        public bool Load(string filepath) {
            try {
                if (Path.IsPathRooted(filepath)) {
                    m_xmldoc.Load(filepath);
                } else {
                    this.SkinPath = string.Format(@"{0}\{1}", this.SkinPath, filepath);
                    string path = string.Format(@"{0}\skin.xml", this.SkinPath);
                    m_xmldoc.Load(path);
                }

                LoadStrings();
                LoadImages();
            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                System.Diagnostics.Trace.TraceError(err.Message);
                return false;
            }

            return true;
        }

        public string GetShortCutString(string name) {
            string str = this.GetString(name);
            str = str.Replace('^', '&');
            return str;
        }

        public string GetString(string name) {
            if (null == name)
                return string.Empty;
            if (!m_strings.ContainsKey(name))
                return name.ToUpper();
            return m_strings[name];
        }

        public Image GetImage(string name) {
            return GetImage(name, 0);
        }

        public Image GetImage(string name, int idx) {
            if (null == name || !m_images.ContainsKey(name) || idx < 0)
                return null;

            IList<Image> imgs = m_images[name];

            if (null != imgs) {
                if (imgs.Count <= idx)
                    return imgs[0];
                return imgs[idx];
            }

            return null;
        }

        protected bool LoadStrings() {

            XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("string");
            if (null == nodeList)
                return false;

            try {
                m_strings.Clear();
                XmlNode node;
                for (int i = 0; i < nodeList.Count; i++) {
                    node = nodeList[i];
                    m_strings.Add(node.Attributes["name"].Value, node.InnerText);
                }
                return true;

            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                System.Diagnostics.Trace.TraceError(err.Message);
            }
            return false;
        }

        protected bool LoadImages() {
            XmlNodeList nodeList = m_xmldoc.GetElementsByTagName("img");
            if (null == nodeList)
                return false;

            try {
                m_images.Clear();

                XmlNode node;
                for (int i = 0; i < nodeList.Count; i++) {
                    node = nodeList[i];
                    m_images.Add(node.Attributes["name"].Value, LoadImages(node.InnerText));
                    node = node.NextSibling;
                }
                return true;

            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                System.Diagnostics.Trace.TraceError(err.Message);
            }
            return false;
        }

        /// <summary>
        /// Load images into image list from give image pathes.
        /// </summary>
        /// <param name="filepath">the file path</param>
        protected IList<Image> LoadImages(string filepathes) {
            string[] pathes = filepathes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (null == pathes || pathes.Length == 0)
                return null;

            List<Image> imgs = new List<Image>();
            for (int i = 0; i < pathes.Length; i++) {
                imgs.Add(LoadImage(pathes[i].Trim()));
            }

            return imgs;
        }

        /// <summary>
        /// Load a image from given path
        /// </summary>
        /// <param name="filepath">the file path</param>
        private Image LoadImage(string filepath) {
            Image img = null;
            string path = null;
            try {
                if (!Path.IsPathRooted(filepath))
                    path = string.Format(@"{0}\{1}", this.SkinPath, filepath);
                else
                    path = filepath;
                Uri uri = new Uri(path);
                img = Bitmap.FromFile(path);
            } catch (Exception err) {
#if DEBUG
                throw err;
#endif
                System.Diagnostics.Trace.TraceError(err.Message);
                return null;
            }

            return img;
        }

        #region Fields
        protected XmlDocument m_xmldoc;
        protected Dictionary<string, string> m_strings;
        protected Dictionary<string, IList<Image>> m_images;
        private static Skins m_instance;
        #endregion

        #region IDisposable Members

        public void Dispose() {
            if (null != m_strings)
                m_strings.Clear();
            if (null != m_images) {
                m_images.Clear();
            }
            m_images = null;
            m_strings = null;
            m_xmldoc = null;
        }

        #endregion
    }
}
