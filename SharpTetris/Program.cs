
//=======================================================================
// <copyright file="filename.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : The app entry
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Net.SamuelChen.Tetris.Service;
using Net.SamuelChen.Tetris.Skin;
using System.Diagnostics;
using Net.SamuelChen.Tetris.Controller;
using Net.SamuelChen.Tetris.Game;
using Net.SamuelChen.Tetris.Blocks;

namespace Net.SamuelChen.Tetris {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif

            Setting.Instance.Load("setting.xml");
            Skins.Instance.Load(Setting.Instance.Skin);
            ControllerFactory.CreateInstance(EnumControllerFactoryType.DirectX);
            Trace.TraceInformation("#Tetris started.");
            GameBase.ActionMapping = new Dictionary<string, object>(){
                {"LEFT", EnumMoving.Left}, 
                {"RIGHT", EnumMoving.Right}, 
                {"DOWN", EnumMoving.Down},
                {"ROTATE", EnumMoving.Rotate},
                {"DIRECT", EnumMoving.DirectDown},
                {"PAUSE", EnumMoving.Pause}
            };
            
            Application.Run(new MainForm());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Debug.Assert(false, e.ToString());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Debug.Assert(false, e.ToString());
        }
    }
}
