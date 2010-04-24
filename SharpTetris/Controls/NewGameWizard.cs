
//=======================================================================
// <copyright file="NewGameWizard.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : A wizard to start a new game.
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
using Net.SamuelChen.Tetris.Game;

namespace Net.SamuelChen.Tetris {
    public partial class NewGameWizard : UserControl, IWizard {

        public NewGameWizard() {
            InitializeComponent();

            m_pages = new LinkedList<IWizardPage>();

            m_curPage = m_pages.First;
            LoadSkins();
        }

        public EnumGameType GameType { get; protected set; }


        private void LoadSkins() {
            
        }

        #region IWizard Members

        protected LinkedList<IWizardPage> m_pages;
        protected LinkedListNode<IWizardPage> m_curPage;

        public void ShowFirst() {
            m_curPage = m_pages.First;
            if (null != m_curPage && null != m_curPage.Value)
                m_curPage.Value.Show(null);
        }

        public IWizardPage AddPage(IWizardPage page) {
            panelPages.Controls.Add(page as Control);
            m_pages.AddLast(page);
            return page;
        }

        public bool CanNext() {
            if (null == m_curPage || null == m_curPage.Value)
                return false;
            if (null != m_curPage.Next && null != m_curPage.Next.Value)
                return true;
            return false;
        }

        public bool CanPrev() {
            if (null == m_curPage || null == m_curPage.Value)
                return false;
            if (null != m_curPage.Previous && null != m_curPage.Previous.Value)
                return true;
            return false;
        }

        public void Next(IList<object> options) {
            if (!CanNext())
                return;
            m_curPage.Value.Hide();
            m_curPage = m_curPage.Next;
            if (null == m_curPage)
                return;

            IWizardPage page = m_curPage.Value;
            if (!page.Show(options)) {
                page.Hide();
                Next(options);
            }
        }

        public void Prev(IList<object> options) {
            if (!CanPrev())
                return;

            m_curPage.Value.Hide();
            m_curPage = m_curPage.Previous;
            if (null == m_curPage)
                return;

            IWizardPage page = m_curPage.Value;
            if (!page.Show(options)) {
                page.Hide();
                Prev(options);
            }
        }

        public void Finish() {
            
        }

        #endregion

    }
}
