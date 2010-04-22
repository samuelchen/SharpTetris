using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Net.SamuelChen.Tetris.Controller {
    internal partial class ControllerSettingInputDialog : Form {

        public List<ControllerKey> PressedKeys { get; set; }
        public bool Working = true;

        public ControllerSettingInputDialog(){
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e) {
            this.Working = false;
            base.OnClosing(e);
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
