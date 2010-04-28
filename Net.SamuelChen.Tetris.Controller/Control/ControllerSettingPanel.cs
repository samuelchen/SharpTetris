
//=======================================================================
// <copyright file="ControllerSettingPanel.cs" company="Samuel Chen Studio">
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

namespace Net.SamuelChen.Tetris.Controller {
    public partial class ControllerSettingPanel : UserControl {

        protected IController SelectedController { get; set; }
        protected Dictionary<int, IController> Controllers { get; set; }
        protected ListViewItem.ListViewSubItem SelectedSubItem { get; set; }

        public ControllerSettingPanel() {
            InitializeComponent();
        }

        public void Init() {
            ControllerFactory factory = ControllerFactory.Instance;
            IEnumerable<IController> controllers = factory.EnumControlls();
            int idx = -1;

            Controllers = new Dictionary<int, IController>();
            comboxDevices.Items.Clear();

            foreach (IController c in controllers) {
                if (null != c) {
                    idx = comboxDevices.Items.Add(string.Format("{0}, {1}", c.Name, c.ID));
                    this.Controllers.Add(idx, c);
                }
            }
        }

        public void SetActions(string[] actions) {
            if (null == actions || 0 == actions.Length)
                return;

            lvKeyMap.Items.Clear();
            foreach (string action in actions) {
                ListViewItem item = new ListViewItem(action.ToUpper());
                item.Name = action.ToUpper();
                lvKeyMap.Items.Add(item);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "-"));
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "-"));
            }
        }

        public string ControllerId {
            get {
                if (null == comboxDevices.SelectedItem)
                    return null;
                string str = comboxDevices.SelectedItem.ToString();
                string[] tmp = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (null == tmp || tmp.Length < 2)
                    return null;
                return tmp[1].Trim();

            }
            set {
                if (null == value)
                    return;
                foreach (object item in comboxDevices.Items) {
                    if (item.ToString().IndexOf(value) >= 0) {
                        comboxDevices.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        public string ControllerName {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public ControllerKeyMap GetKeyMap() {
            ControllerKeyMap map = new ControllerKeyMap();
            for (int i = 0; i < lvKeyMap.Items.Count; i++) {
                ListViewItem item = lvKeyMap.Items[i];
                ControllerKey[] keys = new ControllerKey[2];
                keys[0] = ControllerKey.FromString(item.SubItems[1].Text);
                keys[1] = ControllerKey.FromString(item.SubItems[2].Text);
                map.Add(item.Text.ToLower(), keys);
            }
            return map;
        }

        public void SetKeyMap(ControllerKeyMap map) {
            ControllerKeyMap.Enumerator en = map.GetEnumerator();
            while (en.MoveNext()) {
                string action = en.Current.Key.ToUpper();
                ListViewItem item = lvKeyMap.Items[action];
                if (null == item) {
                    item = lvKeyMap.Items.Add(action);
                    item.Name = action;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "-"));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "-"));

                }
                ControllerKey[] keys = en.Current.Value;
                if (null != keys && keys.Length > 0) {
                    int n = item.SubItems.Count - 1;
                    n = n > keys.Length ? keys.Length : n;
                    for (int i = 0; i < n; i++)
                        item.SubItems[i + 1].Text = keys[i].ToString();
                }
            }
        }

        public void SetKey(ControllerKey key) {
            //ControllerKeyMap map = GetKeyMap();
            //map.Delete(key);

            for (int i = 0; i < lvKeyMap.Items.Count; i++) {
                ListViewItem item = lvKeyMap.Items[i];
                if (ControllerKey.FromString(item.SubItems[1].Text).EqualsTo(key)) {
                    item.SubItems[1].Text = "-";
                    break;
                }
                if (ControllerKey.FromString(item.SubItems[2].Text).EqualsTo(key)) {
                    item.SubItems[2].Text = "-";
                    break;
                }
            }
            if (null != this.SelectedSubItem)
                this.SelectedSubItem.Text = key.ToString();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            lvKeyMap.View = View.Details;
        }

        protected override void OnResize(EventArgs e) {
            if (this.Width < 200)
                this.Width = 200;
            if (this.Height < 100)
                this.Height = 100;

            lvKeyMap.Columns[0].Width = (int)(this.Width * 0.4);
            lvKeyMap.Columns[1].Width = (int)(this.Width * 0.27);
            lvKeyMap.Columns[2].Width = (int)(this.Width * 0.27);

            base.OnResize(e);
        }

        private void comboxDevices_SelectedValueChanged(object sender, EventArgs e) {
            int idx = comboxDevices.SelectedIndex;
            this.SelectedController = Controllers[idx];
        }

        private void lvKeyMap_MouseClick(object sender, MouseEventArgs e) {
            if (lvKeyMap.SelectedItems.Count > 0) {
                ListViewItem item = lvKeyMap.SelectedItems[0];
                this.SelectedSubItem = item.GetSubItemAt(e.X, e.Y);
            } else
                this.SelectedSubItem = null;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(OnStartGatheringInput);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnEndGatheringInput);
            IController c = this.SelectedController;
            if (null == c)
                return;
            ControllerSettingInputDialog target = new ControllerSettingInputDialog();
            try {
                target.PressedKeys = null;
                target.Show(this);
                c.Pressed += new ControllerPressHandler(Controller_Pressed);
                c.Attach(target);
                c.Start();

            } catch (Exception err) {
                MessageBox.Show(err.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Enabled = false;
            worker.RunWorkerAsync(c); // the param will be in e.Argument.
        }

        #region Thread processes

        /// <summary>
        /// Fired when starting a background worker thread to gather controller input
        /// NOTICE: in worker thread
        /// </summary>
        /// <param name="sender">the worker thread</param>
        /// <param name="e">event argument contains argument and result</param>
        void OnStartGatheringInput(object sender, DoWorkEventArgs e) {
            e.Result = GatherInput(e.Argument, sender as BackgroundWorker);
        }

        /// <summary>
        /// Fired when the background worker thread finished gathering controller input
        /// NOTICE: in main thread
        /// </summary>
        /// <param name="sender">the worker thread</param>
        /// <param name="e">event argument contains argument and result</param>
        void OnEndGatheringInput(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null) {
            } else if (e.Cancelled) {
            } else {
                List<ControllerKey> keys = e.Result as List<ControllerKey>;
                IController c = this.SelectedController;
                ControllerSettingInputDialog target = c.Target as ControllerSettingInputDialog;
                if (null != target)
                    target.Close();
                c.Stop();
                c.Deattach();
                if (null != keys && keys.Count > 0)
                    this.SetKey(keys[0]);
            }
            this.Enabled = true;
        }

        // NOTICE: in worker thread
        object GatherInput(object obj, BackgroundWorker worker) {

            IController c = obj as IController;
            if (null == c || null == worker)
                return null;

            bool working = true, keyPressed = false;
            ControllerSettingInputDialog dlg = c.Target as ControllerSettingInputDialog;
            try {

                c.Interval = 0;
                c.Start();

                while (working && !keyPressed) {
                    System.Threading.Thread.Sleep(10);

                    lock (dlg) {
                        working = dlg.Working;
                        keyPressed = (null != dlg.PressedKeys);
                    }
                }

                c.Stop();

            } catch (Exception err) {
                MessageBox.Show(err.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            object ret = null;
            lock (dlg) {
                ret = dlg.PressedKeys;
                dlg.PressedKeys = null;
            }
            return ret;
        }

        // NOTICE: in controller thread
        public static void Controller_Pressed(object sender, ControllerPressedEventArgs e) {
            IController c = sender as IController;
            ControllerSettingInputDialog target = c.Target as ControllerSettingInputDialog;
            if (null != target) {
                target.PressedKeys = e.Keys;
            }

            c.Stop();
        }

        #endregion
    }
}
