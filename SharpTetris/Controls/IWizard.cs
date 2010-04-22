
//=======================================================================
// <copyright file="IWizard.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Interface for the wizard control.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris {
    public interface IWizard {
        IWizardPage AddPage(IWizardPage page);
        void Next(IList<object> options);
        void Prev(IList<object> options);
        void ShowFirst();
        void Finish();
        bool CanNext();
        bool CanPrev();
    }
}
