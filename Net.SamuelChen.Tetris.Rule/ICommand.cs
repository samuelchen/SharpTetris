
//=======================================================================
// <copyright file="ICommand.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Command Interface
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Rule {
    public delegate object CommandHandler(params object[] parameters);

    public interface ICommand {

        object ID { get; }
        CommandHandler Handler { get; set; }
        object[] Parameters { get; set; }
        object Result { get; }
        string ErrorMessage { get; }
        bool Execute();
    }
}
