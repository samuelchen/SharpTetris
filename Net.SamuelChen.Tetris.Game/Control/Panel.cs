
//=======================================================================
// <copyright file="Panel.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  :  
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Game {

    public abstract class Panel : IDisposable {

        public Panel(Control container) {
            Debug.Assert(null != container);

            if (null == container)
                throw new ArgumentNullException("Container can not be null.");

            m_container = container;
            m_container.Paint += new PaintEventHandler(OnContainerPaint);

            this.Top = this.Left = 0;
            this.Width = this.Height = 100;
            this.Border = 2;
            this.BorderBrush = Brushes.Black;
            this.BackgroundBrush = Brushes.LightSalmon;
        }

        #region Properties

        public int Top { get; set; }
        public int Left { get; set; }

        public int Bottom { get { return this.Top + this.Height; } }
        public int Right { get { return this.Left + this.Width; } }

        public int Width {
            get { return m_width; }
            set {
                if (value < 10)
                    m_width = 0;
                else
                    m_width = value;
            }
        }

        public int Height {
            get { return m_height; }
            set {
                if (value < 10)
                    m_height = 10;
                else
                    m_height = value;
            }
        }

        public int Border {
            get { return m_border; }
            set {
                int h = 0, w = 0;
                if (value < 1)
                    m_border = 1;
                else if (value < (h = Height / 2) && value < (w = Width / 2))
                    m_border = value;
                else
                    m_border = h > w ? w : h;
            }
        }

        public Brush BorderBrush { get; set; }
        public Brush BackgroundBrush { get; set; }

        public bool AutoRefresh { get; set; }

        public Control Container { get; set; }
        #endregion

        public virtual void Show(int x, int y) {
            this.Left = x;
            this.Top = y;
            this.Show();
        }

        public virtual void Show() {
            this.Invalidate();
        }

        public virtual void Refresh() {
            this.Invalidate();
        }

        public virtual void Invalidate() {
            Graphics gr = m_container.CreateGraphics();
            this.Paint(gr);
            gr.Dispose();
            gr = null;
        }

        public virtual void Clear() {
        }

        #region Drawing

        protected virtual void OnContainerPaint(object sender, PaintEventArgs e) {
            this.Paint(e.Graphics);
        }

        protected virtual void Paint(Graphics gr) {
            if (null == gr)
                return;

            //Draw Background
            if (null != this.BackgroundBrush)
                gr.FillRectangle(this.BackgroundBrush, this.Left + this.Border, this.Top + this.Border,
                this.Width - this.Border * 2, this.Height - this.Border * 2);

            //Draw Border
            if (null != this.BorderBrush && this.Border > 0) {
                gr.FillRectangle(this.BorderBrush, this.Left, this.Top,
                    this.Width, this.Border);
                gr.FillRectangle(this.BorderBrush, this.Left,
                    this.Bottom - this.Border, this.Width, this.Border);
                gr.FillRectangle(this.BorderBrush, this.Left,
                    this.Top + this.Border, this.Border, this.Height - this.Border * 2);
                gr.FillRectangle(this.BorderBrush, this.Right - this.Border,
                    this.Top + this.Border, this.Border, this.Height - this.Border * 2);
            }

        }

        #endregion

        #region IDisposable Members

        private bool _disposed = false;
        public virtual void Dispose() {
            if (_disposed)
                return;

            this.Clear();
            m_container.Paint -= this.OnContainerPaint;
            m_container = null;
            _disposed = true;
        }

        #endregion

        #region Fields

        private int m_width;
        private int m_height;
        private int m_border;

        private Control m_container;

        #endregion
    }
}
