
//=======================================================================
// <copyright file="IWizardPage.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Interface for a wizard page control.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris {
    public interface IWizardPage {
        bool Show(IList<object> options);
        void Hide();
        object GetValue();
    }
}
