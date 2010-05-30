
//=======================================================================
// <copyright file="Command.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Command base class
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Rule {
    public class Command : ICommand {
        public Command(object id) {
            this.ID = id;
        }

        public Command(object id, CommandHandler handler) : this(id) {
            this.Handler = handler;
        }

        public Command(object id, string name, CommandHandler handler)
            : this(id, handler) {
            this.Name = name;
        }

        public Command(object id, string name, CommandHandler handler, params object[] parameters) 
            : this(id, name, handler) {
            if (null != parameters && parameters.Length > 0) {
                this.Parameters = parameters;
                this.ParameterDescriptions = new string[parameters.Length];
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string[] ParameterDescriptions { get; protected set; }

        public virtual object Execute() {
            object result = null;
            this.Execute(out result);
            return result;
        }

        public string FindParameterDescription(object parameter) {
            if (null == this.Parameters)
                return string.Empty;
            for (int i = 0; i < this.Parameters.Length; i++) {
                if (parameter.Equals(this.Parameters[i]))
                    return this.ParameterDescriptions[i];
            }
            return string.Empty;
        }

        #region ICommand Members

        public virtual object ID { get; protected set; }
        public virtual CommandHandler Handler { get; set; }
        public virtual object[] Parameters { get; set; }
        
        public virtual bool Execute(out object result) {
            result = null;
            if (null == this.Handler)
                return false;

            try {
                result = this.Handler(this.Parameters);
            } catch (Exception err) {
                Trace.TraceWarning("Excute command {0} failed.", this.ID);
                Trace.TraceWarning(err.ToString());
                return false;
            }
            return true;
        }

        #endregion       
    }

}
