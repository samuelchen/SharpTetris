using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Game
{
    public delegate object CommandHandler(params object[] parameters);

    public class Command
    {
        public Command()
        {
        }

        public Command(object command, CommandHandler handler)
        {
            this.Action = command;
            this.Handler = handler;
        }

        public object Action { get; set; }
        public object[] Parameters { get; set; }
        public CommandHandler Handler { get; set; }

        public object Execute()
        {
            if (null == this.Handler)
                return null;

            return this.Handler(this.Parameters);
        }
    }

    public delegate T CommandHandler<T>(params object[] parameters);

    public class Command<TAction, TResult>
    {
        public Command()
        {
        }

        public Command(TAction command, CommandHandler<TResult> handler)
        {
            this.Action = command;
            this.Handler = handler;
        }

        public TAction Action { get; set; }
        public object[] Parameters { get; set; }
        public CommandHandler<TResult> Handler { get; set; }

        public TResult Execute()
        {
            if (null == this.Handler)
                return default(TResult);

            return this.Handler(this.Parameters);
        }
    }

}
