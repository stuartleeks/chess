using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Common
{
    public class Game
    {
        public string Id { get; private set; }
        public Board Board { get; private set; }
        public Color CurrentTurn { get; private set; }

        public IEnumerable<Move> Moves
        {
            get { return _moves.AsEnumerable(); }
        }

        private List<Move> _moves;

        public Game(string id)
        {
            Id = id;
        }
        public Game(string id, Color currentTurn, Board board, List<Move> moves)
        {
            Id = id;
            CurrentTurn = currentTurn;
            Board = board;
            _moves = moves;
        }


        public static Game CreateStartingGame()
        {
            return new Game(GenerateId())
            {
                CurrentTurn = Color.White,
                Board = Common.Board.CreateStartingBoard(),
                _moves = new List<Move>()
            };

        }

        public Game Clone()
        {
            return new Game(this.Id)
            {
                CurrentTurn = this.CurrentTurn,
                Board = this.Board.Clone(),
                _moves = this._moves.Select(m=>m.Clone()).ToList()
            };
        }
        private static string GenerateId()
        {
            // TODO implement friendlier IDs!
            return Guid.NewGuid().ToString();
        }

        public void MakeMove(SquareReference pieceReference, SquareReference endPositionReference)
        {
            Square square = Board[pieceReference];
            _moves.Add(new Move(DateTime.UtcNow, square.Piece, pieceReference, endPositionReference));
            Board.MovePiece(pieceReference, endPositionReference);
            CurrentTurn = (CurrentTurn == Color.Black) ? Color.White : Color.Black;
        }
    }

    public class Move
    {
        public Move(DateTime moveTimeUtc, Piece piece, SquareReference start, SquareReference end)
        {
            MoveTimeUtc = moveTimeUtc;
            Piece = piece;
            Start = start;
            End = end;
        }

        public DateTime MoveTimeUtc { get; private set; }
        public Piece Piece { get; private set; }
        public SquareReference Start { get; private set; }
        public SquareReference End { get; private set; }

        public Move Clone()
        {
            return new Move(MoveTimeUtc, Piece, Start, End);
        }
    }
}
