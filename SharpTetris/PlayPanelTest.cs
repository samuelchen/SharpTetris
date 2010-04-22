using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Game;
using Net.SamuelChen.Tetris.Blocks;

namespace Net.SamuelChen.Tetris {
    public partial class PlayPanelTest : Form {
        public PlayPanelTest() {
            InitializeComponent();
        }

        private void PlayPanelTest_Load(object sender, EventArgs e) {
            
        }

        private void button5_Click(object sender, EventArgs e) {
            PlayPanel.CreateNextSharp();
            PlayPanel.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e) {
            PlayPanel.MoveCurSharp(EnumMoving.Rotate);
        }

        private void button2_Click(object sender, EventArgs e) {
            PlayPanel.MoveCurSharp(EnumMoving.Left);
        }

        private void button3_Click(object sender, EventArgs e) {
            PlayPanel.MoveCurSharp(EnumMoving.Right);
        }

        private void button4_Click(object sender, EventArgs e) {
            PlayPanel.MoveCurSharp(EnumMoving.Down);
        }

        private void button6_Click(object sender, EventArgs e) {
            PlayPanel.MoveCurSharp(EnumMoving.DirectDown);
            PlayPanel.Invalidate();
        }

        private void button7_Click(object sender, EventArgs e) {
            PlayPanel.Initialize();
        }
    }
}
