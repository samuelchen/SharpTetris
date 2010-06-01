
//=======================================================================
// <copyright file="InfoPanel.cs" company="Samuel Chen Studio">
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

    public class InfoPanel : Panel {

        public InfoPanel(Control container) 
            : base(container) {

            m_images = new Dictionary<string, Image>();
            m_strings = new Dictionary<string, string>();
            m_stringFonts = new Dictionary<string, Font>();
            m_brushes = new Dictionary<string, Brush>();
            m_rectangles = new Dictionary<string, Rectangle>();
            m_lines = new List<string>();
        }

        #region Properties

        #endregion

        public override void Clear() {
            m_images.Clear();
            m_strings.Clear();
            m_lines.Clear();
            foreach (Font font in m_stringFonts.Values)
                font.Dispose();
            m_stringFonts.Clear();
            m_brushes.Clear();
            foreach (Brush brush in m_brushes.Values)
                brush.Dispose();
            m_rectangles.Clear();

            base.Clear();
        }

        #region Object Management

        #region Image

        public void AddImage(string name, Image img, int x, int y, int width, int height) {
            Rectangle rect = new Rectangle(x, y, width, height);
            this.AddImage(name, img, rect);
        }
        public void AddImage(string name, Image img, Rectangle rect) {
            Debug.Assert(!string.IsNullOrEmpty(name) && null != img);

            m_images.Add(name, img);
            m_rectangles.Add("i_" + name, rect);

            if (this.AutoRefresh)
                this.Invalidate();
        }
        public void SetImage(string name, Image img, int x, int y, int width, int height) {
            this.RemoveImage(name);
            this.AddImage(name, img, x, y, width, height);
        }
        public void SetImage(string name, Image img, Rectangle rect) {
            this.SetImage(name, img, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public void RemoveImage(string name) {
            Debug.Assert(!string.IsNullOrEmpty(name));

            m_rectangles.Remove("i_" + name);
            m_images.Remove(name);
        }

        #endregion

        #region String

        public void AddString(string name, string value, Font font, Brush brush, int x, int y, int width, int height) {
            Rectangle rect = new Rectangle(x, y, width, height);
            this.AddString(name, value, font, brush, rect);
        }
        public void AddString(string name, string value, Font font, Brush brush, Rectangle rect) {
            Debug.Assert(!string.IsNullOrEmpty(name)
                && !string.IsNullOrEmpty(value) && null != font && null != brush);

            m_strings.Add(name, value);
            m_stringFonts.Add(name, font);
            m_brushes.Add("s_" + name, brush);
            m_rectangles.Add("s_" + name, rect);

            if (this.AutoRefresh)
                this.Invalidate();
        }
        public void SetString(string name, string value, Font font, Brush brush, int x, int y, int width, int height) {
            this.RemoveString(name);
            this.AddString(name, value, font, brush, x, y, width, height);
        }
        public void SetString(string name, string value, Font font, Brush brush, Rectangle rect) {
            this.SetString(name, value, font, brush, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public void RemoveString(string name) {
            Debug.Assert(!string.IsNullOrEmpty(name));

            m_rectangles.Remove("s_" + name);
            m_stringFonts.Remove(name);
            m_brushes.Remove("s_" + name);
            m_strings.Remove(name);
        }

        #endregion

        #region Line

        public void AddLine(string name, Brush brush, int x, int y, int len) {
            this.AddLine(name, brush, x, y, len, 1, false);
        }
        public void AddLine(string name, Brush brush, Point pos, int len) {
            this.AddLine(name, brush, pos, len, 1, false);
        }
        public void AddLine(string name, Brush brush, Point pos, int len, int width, bool verticle) {
            this.AddLine(name, brush, pos.X, pos.Y, len, width, verticle);
        }
        public void AddLine(string name, Brush brush, int x, int y, int len, int width, bool verticle) {
            Debug.Assert(!string.IsNullOrEmpty(name) && null != brush);

            Rectangle rect;
            if (verticle)
                rect = new Rectangle(x, y, width, len);
            else
                rect = new Rectangle(x, y, len, width);

            m_lines.Add(name);
            m_rectangles.Add("l_" + name, rect);
            m_brushes.Add("l_" + name, brush);

            if (this.AutoRefresh)
                this.Invalidate();
        }
        public void SetLine(string name, Brush brush, int x, int y, int len, int width, bool verticle) {
            this.RemoveString(name);
            this.AddLine(name, brush, x, y, len, width, verticle);
        }
        public void SetLine(string name, Brush brush, Point pos, int len, int width, bool verticle) {
            this.SetLine(name, brush, pos.X, pos.Y, len, width, verticle);
        }
        public void SetLine(string name, Brush brush, int x, int y, int len) {
            this.SetLine(name, brush, x, y, len, 2, false);
        }
        public void SetLine(string name, Brush brush, Point pos, int len) {
            this.SetLine(name, brush, pos.X, pos.Y, len, Width, false);
        }
        public void RemoveLine(string name) {
            Debug.Assert(!string.IsNullOrEmpty(name));

            m_rectangles.Remove("l_" + name);
            m_brushes.Remove("l_" + name);
            m_lines.Remove(name);
        }

        #endregion

        #endregion

        #region Drawing

        protected override void Paint(Graphics gr) {
            if (null == gr)
                return;

            base.Paint(gr);

            foreach (string name in m_images.Keys)
                this.DrawImage(gr, name);

            foreach (string name in m_strings.Keys)
                this.DrawString(gr, name);

            foreach (string name in m_lines)
                this.DrawLine(gr, name);
        }

        protected void DrawImage(Graphics gr, string name) {
            Debug.Assert(null != gr && !string.IsNullOrEmpty(name));

            Image img = m_images[name];
            Rectangle rect = m_rectangles["i_" + name];
            rect.Offset(this.Left, this.Top);

            gr.DrawImage(img, rect);
        }

        protected void DrawString(Graphics gr, string name) {
            Debug.Assert(null != gr && !string.IsNullOrEmpty(name));

            String value = m_strings[name];
            Font font = m_stringFonts[name];
            Brush brush = m_brushes["s_" + name];
            Rectangle rect = m_rectangles["s_" + name];
            rect.Offset(this.Left, this.Top);
            RectangleF rectF = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            bool bNewFont = false;
            if (null == font) {
                font = new Font(FontFamily.GenericSansSerif, 8);
                bNewFont = true;
            }

            if (null == brush)
                brush = Brushes.Gray;

            if (rectF.Width <= 0)
                rectF.Width = this.Width;
            if (rectF.Height <= 0)
                rectF.Height = this.Height;
            gr.DrawString(value, font, brush, rectF);

            if (bNewFont)
                font.Dispose();
        }

        protected void DrawLine(Graphics gr, string name) {
            Debug.Assert(null != gr && !string.IsNullOrEmpty(name));

            Rectangle rect = m_rectangles["l_" + name];
            rect.Offset(this.Left, this.Top);
            Brush brush = m_brushes["l_" + name];

            gr.FillRectangle(brush, rect);
        }

        #endregion

        #region IDisposable Members

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            this.Clear();
            base.Dispose();
            _disposed = true;
        }

        #endregion

        #region Fields

        protected Dictionary<string, Image> m_images;
        protected Dictionary<string, string> m_strings;
        protected List<string> m_lines;
        protected Dictionary<string, Font> m_stringFonts;
        protected Dictionary<string, Brush> m_brushes;
        protected Dictionary<string, Rectangle> m_rectangles;

        #endregion
    }
}
