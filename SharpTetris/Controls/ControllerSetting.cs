
//=======================================================================
// <copyright file="ControllerSetting.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
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
using System.Diagnostics;
using Net.SamuelChen.Tetris.Controller;

namespace Net.SamuelChen.Tetris {
    public partial class ControllerSetting : UserControl {

        public const int MaxPlayers = 8;

        private List<string> m_controllers = null;  // controllers

        public ControllerSetting() {
            InitializeComponent();
            m_controllers = new List<string>();
        }

        /// <summary>
        /// The action names.
        /// </summary>
        public List<string> Actions { get; set; }

        /// <summary>
        /// Player name list.
        /// </summary>
        public List<string> Controllers {
            get { return m_controllers; }
        }


        public void Clear() {
            tpSetting.TabPages.Clear();
            m_controllers.Clear();
        }

        public string GetControllerId(string name) {
            TabPage tab = tpSetting.TabPages[name];
            if (null == tab)
                return null;
            ControllerSettingPanel csp = tab.Controls[name] as ControllerSettingPanel;
            return csp.ControllerId;
        }

        /// <summary>
        /// Add muli-controllers to controller setting control.
        /// </summary>
        /// <param name="names">The controller identity list. Must be unique.</param>
        public void AddControllers(string[] names) {
            if (null == names || 0 == names.Length)
                return;

            tpSetting.Controls.Clear();
            foreach (string name in names) {
                try {
                    AddController(name);
                } catch (DuplicateNameException) {
                    Trace.TraceWarning("Player \"{0}\" is already existed.", name);
                }
            }
        }

        /// <summary>
        /// Add muli-controllers to controller setting control.
        /// </summary>
        /// <param name="names">The controller identity list. Must be unique.</param>
        public void AddControllers(List<string> names) {
            if (null == names || 0 == names.Count)
                return;
            AddControllers(names.ToArray());
        }

        /// <summary>
        /// Add a single controller to controller setting.
        /// </summary>
        /// <param name="name">The controller identity.</param>
        /// <exception cref="DuplicateNameException">Throws if the controller is alread added.</exception>
        public void AddController(string name) {
            if (m_controllers.Contains(name))
                throw new DuplicateNameException("The controller is already existed.");

            TabPage tab = new TabPage(name);
            ControllerSettingPanel csp = new ControllerSettingPanel();
            tpSetting.TabPages.Add(tab);
            tab.Name = name;
            csp.Dock = DockStyle.Fill;
            csp.Name = name;
            csp.Init();
            csp.SetActions(this.Actions.ToArray());
            tab.Controls.Add(csp);
            m_controllers.Add(name);
        }

        /// <summary>
        /// Remove current controller controller setting.
        /// </summary>
        public void RemoveController() {
            TabPage tab = tpSetting.SelectedTab;
            if (null == tab)
                return;
            RemoveController(tab.Name); 
        }

        /// <summary>
        /// Remove a controller from controller setting by controller identity.
        /// </summary>
        /// <param name="name">The controller identity.</param>
        public void RemoveController(string name) {
            TabPage tab = tpSetting.TabPages[name];
            if (null == tab)
                return;
            m_controllers.Remove(name);
            tpSetting.TabPages.RemoveByKey(name);    
        }
        
        /// <summary>
        /// Remove a controller from controller setting by given TabPage index.
        /// </summary>
        /// <param name="idx">The TabPage index.</param>
        public void RemoveController(int idx) {
            TabPage tab = tpSetting.TabPages[idx];
            if (null == tab)
                return;
            RemoveController(tab.Name);  
        }

        /// <summary>
        /// Retrive a keymap by given TabPage index.
        /// </summary>
        /// <param name="idx">The TabPage index.</param>
        /// <returns>The wanted keymap. Null if out of bounds.</returns>
        public ControllerKeyMap GetKeyMap(int idx, out string controllerId) {
            controllerId = null;
            if (idx <0 || idx >= tpSetting.TabCount)
                return null;
            TabPage tab = tpSetting.TabPages[idx];
            return GetKeyMap(tab.Name, out controllerId);
        }

        /// <summary>
        /// Retrive a keymap by controller identity.
        /// </summary>
        /// <param name="name">The controller identity.</param>
        /// <returns>The wanted keymap. Null if not find.</returns>
        public ControllerKeyMap GetKeyMap(string name, out string controllerId) {
            controllerId = null;
            TabPage tab = tpSetting.TabPages[name];
            if (null == tab)
                return null;
            ControllerSettingPanel csp = tab.Controls[name] as ControllerSettingPanel;
            controllerId = csp.ControllerId;
            return csp.GetKeyMap();
        }

        /// <summary>
        /// Set keymap for a controller.
        /// </summary>
        /// <param name="name">The controller identity.</param>
        /// <param name="controllerId">The controller instance guid.</param>
        /// <param name="keymap">The keymap will set to.</param>
        /// <returns>The original keymap of the controller.</returns>
        public ControllerKeyMap SetKeyMap(string name, string controllerId, ControllerKeyMap keymap) {
            TabPage tab = tpSetting.TabPages[name];
            if (null == tab)
                return null;
            ControllerSettingPanel csp = tab.Controls[name] as ControllerSettingPanel;
            ControllerKeyMap originalKeymap = csp.GetKeyMap();
            csp.SetKeyMap(keymap);
            csp.ControllerId = controllerId;
            return originalKeymap;
        }

        /// <summary>
        /// Set keymap for a controller.
        /// </summary>
        /// <param name="name">The controller identity.</param>
        /// <param name="keymap">The keymap will set to.</param>
        /// <returns>The original keymap of the controller.</returns>
        public ControllerKeyMap SetKeyMap(string name, ControllerKeyMap keymap) {
            TabPage tab = tpSetting.TabPages[name];
            if (null == tab)
                return null;
            return SetKeyMap(name, null, keymap);
        }

        /// <summary>
        /// Set keymap for TabPage.
        /// </summary>
        /// <param name="idx">The TabPage index.</param>
        /// <param name="keymap">The keymap will set to.</param>
        /// <returns>The original keymap of the controller.</returns>
        public ControllerKeyMap SetKeyMap(int idx, ControllerKeyMap keymap) {
            TabPage tab = tpSetting.TabPages[idx];
            if (null == tab)
                return null;
            return SetKeyMap(tab.Name, null, keymap);
        }

        /// <summary>
        /// Set keymap for TabPage.
        /// </summary>
        /// <param name="idx">The TabPage index.</param>
        /// <param name="controllerId">The controller instance guid.</param>
        /// <param name="keymap">The keymap will set to.</param>
        /// <returns>The original keymap of the controller.</returns>
        public ControllerKeyMap SetKeyMap(int idx, string controllerId, ControllerKeyMap keymap) {
            TabPage tab = tpSetting.TabPages[idx];
            if (null == tab)
                return null;
            return SetKeyMap(tab.Name, controllerId, keymap);
        }
        
        private void ControllerSetting_Resize(object sender, EventArgs e) {
            if (this.Width < 200)
                this.Width = 200;
            if (this.Height < 100)
                this.Height = 100;

            tpSetting.Height = (int)(this.Height * 0.8);
        }

    }
}
