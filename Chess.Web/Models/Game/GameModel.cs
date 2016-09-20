using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common = Chess.Common;

namespace Chess.Web.Models.Game
{
    public class GameModel
    {
        public Common.Game Game { get; set; }

        public Board Board { get; set; }
    }
    public class Board
    {
        public BoardSquare[][] Squares { get; set; }
    }
    public class BoardSquare
    {
        public string PieceName { get; set; }
        public string PieceImage { get; set; }
        public string SquareColour { get; internal set; }
    }

}