using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Common
{
    public class Game
    {
        public Board Board { get; set; }
        public Color CurrentTurn { get; set; }

        public static Game CreateStartingGame()
        {
            return new Game
            {
                CurrentTurn = Color.White,
                Board = Common.Board.CreateStartingBoard()
            };

        }

        public Game Clone()
        {
            return new Game
            {
                CurrentTurn = this.CurrentTurn,
                Board = this.Board.Clone()
            };
        }
    }

}
