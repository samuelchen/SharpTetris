
//=======================================================================
// <copyright file="Command.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Rule base class
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Net.SamuelChen.Tetris.Rule {
    public class Rule : IRule {

        public Rule(object id) {
            this.ID = id;
        }

        public Rule(object id, RuleVerifyHandler handler) : this(id) {
            this.Handler = handler;
        }

        public Rule(object id, string name, RuleVerifyHandler handler)
            : this(id, handler) {
            this.Name = name;
        }

        public Rule(object id, string name, RuleVerifyHandler handler, params object[] inputs) 
            : this(id, name, handler) {
            if (null != inputs && inputs.Length > 0) {
                this.Inputs = inputs;
                this.InputDescriptions = new string[inputs.Length];
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string[] InputDescriptions { get; protected set; }

        #region IRule Members

        public object ID { get; protected set; }
        public object[] Inputs { get; set; }
        public RuleVerifyHandler Handler { get; set; }

        public bool Verify() {
            Debug.Assert(!(null == this.Inputs || null == this.Handler));
            if (null == this.Inputs || null == this.Handler)
                return false;

            try {
                return this.Handler(this.Inputs);
            } catch (Exception err) {
                Trace.TraceWarning("Fail to verify rule \"{0}\".", this.Name);
                Trace.TraceWarning(err.ToString());
            }

            return false;
        }

        #endregion
    }
}
