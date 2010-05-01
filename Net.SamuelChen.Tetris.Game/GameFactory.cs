using System;
using System.Collections.Generic;
using System.Text;

namespace Net.SamuelChen.Tetris.Game {
    public class GameFactory {
        private static GameFactory m_instance;

        /// <summary>
        /// To get a factory instance.
        /// </summary>
        public static GameFactory Instance {
            get {
                if (null == m_instance)
                    m_instance = new GameFactory();
                return m_instance;
            }
            protected set {
                m_instance = value;
            }
        }

        /// <summary>
        /// To create a game of given type.
        /// </summary>
        /// <param name="type">game type.</param>
        /// <returns>a game instance</returns>
        public static IGame CreateGame(EnumGameType type) {
            IGame game = null;
            switch (type) {
                case EnumGameType.Single:
                case EnumGameType.Multiple:
                    game = new LocalGame(type, null); // assign the container later
                    break;
                case EnumGameType.Host:
                    game = new ServerGame();
                    break;
                case EnumGameType.Client:
                    game = new ClientGame();
                    break;
                default:
                    throw new GameException("This type of game is not implmented.", null);
            }
            return game;
        }
    }
}
