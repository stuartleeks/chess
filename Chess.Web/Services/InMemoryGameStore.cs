﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common = Chess.Common;

namespace Chess.Web.Services
{
    public class InMemoryGameStore : IGameStore
    {
        private Dictionary<string, Common.Game> _games = new Dictionary<string, Common.Game>();

        public Common.Game GetGame(string gameId)
        {
            return _games[gameId]?.Clone();
        }
        public void Save(Common.Game game)
        {
            _games[game.Id] = game;
        }

    }
}
