
//=======================================================================
// <copyright file="IController.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Define the abstract interface of a controller
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;

namespace Net.SamuelChen.Tetris.Controller {
    /// <summary>
    /// The delegate for controller pressing.
    /// NOTICE: this event is in controller process thread.
    /// </summary>
    public delegate void ControllerPressHandler(object sender, ControllerPressedEventArgs e);

    /// <summary>
    /// The interface for controller.
    /// </summary>
    public interface IController : IDisposable {
        event ControllerPressHandler Pressed;

        int ButtonCount { get; }
        Guid ID { get; }
        string Name { get; }
        object Target { get; }
        EnumControllerType Type { get; }
        ControllerKeyMap KeyMap { get; set; }
        object Tag { get; set; }

        int Interval { get; set; }

        bool Attached { get; }
        bool Working { get; }

        void Attach(Object target);
        void Deattach();
        void Start();
        void Stop();
        ControllerKey[] Translate(string action);
        string Translate(ControllerKey key);
    }
}
