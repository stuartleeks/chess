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

        private List<Move> _moves;
        public IEnumerable<Move> Moves
        {
            get { return _moves.AsEnumerable(); }
        }

        public Game(string id)
        {
            Id = id;
        }
        public Game(string id, Color currentTurn, Board board, List<Move> moves = null)
        {
            Id = id;
            CurrentTurn = currentTurn;
            Board = board;
            _moves = moves ?? new List<Move>();
        }


        public static Game CreateStartingGame()
        {
            return new Game(GenerateId())
            {
                CurrentTurn = Color.White,
                Board = Common.Board.CreateStartingBoard()
            };

        }

        public Game Clone()
        {
            return new Game(this.Id)
            {
                CurrentTurn = this.CurrentTurn,
                Board = this.Board.Clone(),
                _moves = this._moves.Select(m => m.Clone()).ToList()
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

        public IEnumerable<SquareReference> GetAvailableMoves(SquareReference from)
        {
            var square = Board[from];
            switch (square.Piece.PieceType)
            {
                case PieceType.Pawn:
                    return GetAvailableMoves_Pawn(Board, square);
                case PieceType.Rook:
                    return GetAvailableMoves_Rook(Board, square);
                case PieceType.Knight:
                    return GetAvailableMoves_Knight(Board, square);
                case PieceType.Bishop:
                    return GetAvailableMoves_Bishop(Board, square);
                case PieceType.Queen:
                    return GetAvailableMoves_Queen(Board, square);
                case PieceType.King:
                    return GetAvailableMoves_King(Board, square);
                default:
                    throw new InvalidOperationException("Unhandled piece type!!");
            }
        }

        private IEnumerable<SquareReference> GetAvailableMoves_Pawn(Board board, Square square)
        {
            var start = square.Reference;
            var piece = square.Piece;

            var homeRow = piece.Color == Color.Black ? 1 : 6;
            var direction = piece.Color == Color.Black ? 1 : -1; // row 0 at top (black start)
            var opponentColor = piece.Color == Color.Black ? Color.White : Color.Black;

            var move1 = start.Move(direction, 0);
            if (move1 != null
                && board[move1.Value].Piece.Color == Color.Empty)
            {
                yield return move1.Value;
                if (start.Row == homeRow)
                {
                    var move2 = move1.Value.Move(direction, 0);
                    if (move2 != null
                        && board[move2.Value].Piece.Color == Color.Empty)
                    {
                        yield return move2.Value;
                    }
                }
            }
            var diagMoves = new[] { start.Move(direction, 1), start.Move(direction, -1) }
                            .WhereNotNull()
                            .Where(s => board[s].Piece.Color == opponentColor);
            foreach (var move in diagMoves)
            {
                yield return move;
            }
        }

        private class Movement
        {
            public int RowDelta { get; set; }
            public int ColumnDelta { get; set; }
        }
        private static Movement[] RookMovements = new[]
        {
            new Movement { RowDelta = 1, ColumnDelta = 0 },
            new Movement { RowDelta = -1, ColumnDelta = 0 },
            new Movement { RowDelta = 0, ColumnDelta = 1 },
            new Movement { RowDelta = 0, ColumnDelta = -1 },
        };
        private IEnumerable<SquareReference> GetAvailableMoves_Rook(Board board, Square square)
        {
            var start = square.Reference;
            var piece = square.Piece;

            var opponentColor = piece.Color == Color.Black ? Color.White : Color.Black;

            var moves = RookMovements
                            .SelectMany(
                                movement =>
                                    board.MovesUntilPiece(
                                        start: start,
                                        rowDelta: movement.RowDelta,
                                        columnDelta: movement.ColumnDelta,
                                        opponentColor: opponentColor
                                    )
                                )
                            .ToList();
            return moves;
        }
        private static Movement[] KnightMovements = new[]
        {
            new Movement { RowDelta = 2, ColumnDelta = 1 },
            new Movement { RowDelta = 2, ColumnDelta = -1 },
            new Movement { RowDelta = 1, ColumnDelta = 2 },
            new Movement { RowDelta = 1, ColumnDelta = -2 },
            new Movement { RowDelta = -1, ColumnDelta = 2 },
            new Movement { RowDelta = -1, ColumnDelta = -2 },
            new Movement { RowDelta = -2, ColumnDelta = 1 },
            new Movement { RowDelta = -2, ColumnDelta = -1 },
        };
        private IEnumerable<SquareReference> GetAvailableMoves_Knight(Board board, Square square)
        {
            var start = square.Reference;
            var piece = square.Piece;

            var ownColor = piece.Color;

            var moves = KnightMovements
                            .Select(m => start.Move(m.RowDelta, m.ColumnDelta))
                            .WhereNotNull()
                            .Where(m => board[m].Piece.Color != ownColor);
            return moves;
        }

        private IEnumerable<SquareReference> GetAvailableMoves_Bishop(Board board, Square square)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<SquareReference> GetAvailableMoves_Queen(Board board, Square square)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<SquareReference> GetAvailableMoves_King(Board board, Square square)
        {
            throw new NotImplementedException();
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

    public static class MoveExtensions
    {
        public static SquareReference? Move(this SquareReference start, int rowDelta, int columnDelta)
        {
            int newRow = start.Row + rowDelta;
            int newColumn = start.Column + columnDelta;

            if (newRow < 0 || newRow > 7
                || newColumn < 0 || newColumn > 7)
            {
                return null;
            }
            return SquareReference.FromRowColumn(newRow, newColumn);
        }
        public static IEnumerable<SquareReference> MovesUntilPiece(
            this Board board,
            SquareReference start,
            int rowDelta,
            int columnDelta,
            Color opponentColor)
        {
            SquareReference? move = start;
            while (true)
            {
                move = move.Value.Move(rowDelta, columnDelta);
                if (move == null)
                {
                    yield break;
                }
                var piece = board[move.Value].Piece;
                if (piece.Color == opponentColor)
                {
                    yield return move.Value;
                    yield break;
                }
                else if (piece.Color == Color.Empty)
                {
                    yield return move.Value;
                    // yield and continue
                }
                else
                {
                    // own color
                    yield break;
                }
            }
        }

    }
}
