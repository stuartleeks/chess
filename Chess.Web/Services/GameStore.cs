using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common = Chess.Common;

namespace Chess.Web.Services
{
    public class GameStore
    {
        static Common.Game _game = Common.Game.CreateStartingGame();

        // TODO - interface?? MongoDB, ...??
        // TODO - handle more than one game!

        public Common.Game GetGame()
        {
            return _game.Clone();
        }
        public void Save(Common.Game game)
        {
            _game = game;
        }

    }
}
