
//=======================================================================
// <copyright file="TetrisGame.cs" company="Samuel Chen Studio">
//     Copyright (c) 2010 Samuel Chen Studio. All rights reserved.
//     author   : Samuel Chen
//     purpose  : Abstract tetris game.
//     contact  : http://www.SamuelChen.net, samuel.net@gmail.com
// </copyright>
//=======================================================================


namespace Net.SamuelChen.Tetris.Game {
    public abstract class TetrisGame : GameBase {

        public TetrisGame()
            : base() {
            this.Level = 0;
            this.MaxPlayers = 4;
            GameType = EnumGameType.Single;
        }

        public TetrisGame(EnumGameType type) : this() {
            this.GameType = type;
        }

        #region properties

        /// <summary>
        /// Game level
        /// </summary>
        public int Level { get; protected set; }

        /// <summary>
        /// Game type
        /// </summary>
        public EnumGameType GameType { get; protected set; }

        public int MaxPlayers { get; set; }

        #endregion

        public virtual void Refresh() {
        }


        public override void Start() {
            base.Start();
            Start(1);
        }

        public virtual void Start(int level) {
            if (level < 1)
                Level = 1;
            else
                Level = level;
        }

        #region IDisposable Members

        private bool _disposed = false;
        public override void Dispose() {
            if (_disposed)
                return;

            this.Stop();
            base.Dispose();
            _disposed = true;
        }

        #endregion
    }
}
