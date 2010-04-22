
//=======================================================================
// <copyright file="ControllerEvent.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : 
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;

namespace Net.SamuelChen.Tetris.Controller
{
	/// <summary>
    /// ControllerPressedEventArgs reprents an argument for ControllerPressedEvent
	/// </summary>
	public class ControllerPressedEventArgs : System.EventArgs
	{

#if DEBUG
        public string sDebug = string.Empty;
#endif

		public ControllerPressedEventArgs()
		{
		}

        public ControllerPressedEventArgs(ControllerKey[] keyNumbers) {
            if (null == keyNumbers || 0 == keyNumbers.Length)
                return;

            if (null == Keys)
                Keys = new List<ControllerKey>(keyNumbers.Length);
            Keys.AddRange(keyNumbers);
		}

		/// <summary>
		/// The key pressed in event.
		/// </summary>
        public List<ControllerKey> Keys { get; protected set; }

	}
}
