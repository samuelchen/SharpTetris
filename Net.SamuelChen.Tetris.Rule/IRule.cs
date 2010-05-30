
//=======================================================================
// <copyright file="IRule.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Inteface to define a rule
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Rule {

    public delegate bool RuleVerifyHandler(params object[] inputs);

    public interface IRule {
        object ID { get; }
        object[] Inputs { get; set; }
        RuleVerifyHandler Handler { get; set; }
       
        /// <summary>
        /// To verfify whether this rule is passed.
        /// </summary>
        /// <returns>ture if passed. false if failed.</returns>
        bool Verify();
    }

    interface IMovementRule : IRule {
    }

    interface IActionRule : IRule {
    }

    interface ISystemRule : IRule {
    }
}
