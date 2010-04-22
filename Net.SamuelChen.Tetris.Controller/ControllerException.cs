
//=======================================================================
// <copyright file="ControllerException.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Controller {
    public class ControllerException : Exception {
        public ControllerException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
