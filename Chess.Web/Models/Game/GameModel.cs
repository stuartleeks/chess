﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common = Chess.Common;

namespace Chess.Web.Models.Game
{
    public class GameModel
    {
        public Board Board { get; set; }
        public Common.Color CurrentPlayer { get; set; }
        public Common.Color Opponent { get; set; }
        public bool HasMoves { get; set; }
        public bool InCheck { get; set; }
        public List<HistoricalMove> MoveHistory { get; set; }
    }
    public class Board
    {
        public Common.SquareReference? SelectedSquare { get; set; }
        public BoardSquare[][] Squares { get; set; }
    }
    public class BoardSquare
    {
        public string PieceName { get; set; }
        public string PieceImage { get; set; }
        public string SquareColour { get; set; }
        /// <summary>
        /// When choosing a piece to move, this indicates squares with a piece that can be selected
        /// When moving a piece, this indicates squares that the piece can be moved to
        /// </summary>
        public bool CanSelect { get; set; }
        public string ReferenceString { get; set; }
        public Common.SquareReference Reference { get; set; }
        public string SelectUrl { get; set; }
    }
    public class HistoricalMove
    {
        public int TurnNumber { get; set; }
        public string White { get; set; }
        public string Black { get; set; }

    }
}
