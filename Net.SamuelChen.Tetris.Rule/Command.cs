
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

namespace Net.SamuelChen.Tetris.Rule {
    public class Command : ICommand {
        public Command(object id) {
            this.ID = id;
        }

        public Command(object id, CommandHandler handler)
            : this(id) {
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
        public virtual object Result { get; protected set; }
        public string ErrorMessage { get; protected set; }

        public virtual bool Execute() {
            this.Result = null;
            this.ErrorMessage = null;

            if (null == this.Handler)
                return false;

            try {
                this.Result = this.Handler(this.Parameters);
            } catch (Exception err) {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Error occurs whlile executing command '{0}'.", this.ID);
                sb.AppendLine();
                sb.AppendFormat("Name: {0}", this.Name);
                sb.AppendLine();
                sb.AppendFormat("Description: {0}", this.Description);
                sb.AppendLine();
                sb.AppendFormat("Handler: {0}.{1}", this.Handler.Method.ReflectedType, this.Handler.Method.Name);
                sb.AppendLine();
                sb.AppendLine("Parameters:");
                for (int i = 0; i < this.Parameters.Length; i++) {
                    sb.AppendFormat("{0}: {1}", i, this.Parameters[i]);
                    sb.AppendLine();
                }
                this.ErrorMessage = sb.ToString();
                return false;
            }
            return true;
        }

        #endregion
    }
}
