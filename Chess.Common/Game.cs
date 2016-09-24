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

        public Game(string id, Color currentTurn, Board board, List<Move> moves = null)
        {
            Id = id;
            CurrentTurn = currentTurn;
            Board = board;
            _moves = moves ?? new List<Move>();
        }


        public static Game CreateStartingGame()
        {
            return new Game(
                GenerateId(),
                Color.White,
                Common.Board.CreateStartingBoard()
            );

        }

        public Game Clone()
        {
            return new Game(
                Id,
                CurrentTurn,
                Board.Clone(),
                _moves.Select(m => m.Clone()).ToList()
            );
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
            var initialMoves = GetAvailableMoves_NoCheckTest(Board, from);

            var square = Board[from];
            var piece = square.Piece;
            var opponentColor = piece.Color == Color.Black ? Color.White : Color.Black;


            // filter out moves that result in check
            // brute force approach ;-)
            // for each move, check whether there are any moves for any opponent piece after that
            // which would end on the current player's king's square
            // if so then we have a move that puts the player into check, so filter out
            var nonCheckMoves = initialMoves.Where(move =>
            {
                var newBoard = Board.Clone();
                newBoard.MovePiece(from: square.Reference, to: move);
                var kingSquare = newBoard.FindPiece(piece.Color, PieceType.King).Value;

                var opponentPieceReferences = Board.AllSquares()
                                        .Where(s => s.Piece.Color == opponentColor)
                                        .Select(s => s.Reference);

                return !opponentPieceReferences.Any(
                        squareReference => GetAvailableMoves_NoCheckTest(newBoard, squareReference)
                                                    .Any(end => end == kingSquare.Reference)
                    );
             });

            return nonCheckMoves;
        }



        private IEnumerable<SquareReference> GetAvailableMoves_NoCheckTest(Board board, SquareReference from)
        {
            var square = board[from];
            switch (square.Piece.PieceType)
            {
                case PieceType.Pawn:
                    return GetAvailableMoves_Pawn(board, square);
                case PieceType.Rook:
                    return GetAvailableMoves_Rook(board, square);
                case PieceType.Knight:
                    return GetAvailableMoves_Knight(board, square);
                case PieceType.Bishop:
                    return GetAvailableMoves_Bishop(board, square);
                case PieceType.Queen:
                    return GetAvailableMoves_Queen(board, square);
                case PieceType.King:
                    return GetAvailableMoves_King(board, square);
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

        private static Movement[] BishopMovements = new[]
        {
            new Movement { RowDelta = 1, ColumnDelta = 1 },
            new Movement { RowDelta = 1, ColumnDelta = -1 },
            new Movement { RowDelta = -1, ColumnDelta = 1 },
            new Movement { RowDelta = -1, ColumnDelta = -1 },

        };
        private IEnumerable<SquareReference> GetAvailableMoves_Bishop(Board board, Square square)
        {
            var start = square.Reference;
            var piece = square.Piece;

            var opponentColor = piece.Color == Color.Black ? Color.White : Color.Black;

            var moves = BishopMovements
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


        private static Movement[] QueenMovements = new[]
        {
            new Movement { RowDelta = 1, ColumnDelta = 1 },
            new Movement { RowDelta = 1, ColumnDelta = -1 },
            new Movement { RowDelta = -1, ColumnDelta = 1 },
            new Movement { RowDelta = -1, ColumnDelta = -1 },
            new Movement { RowDelta = 1, ColumnDelta = 0 },
            new Movement { RowDelta = -1, ColumnDelta = 0 },
            new Movement { RowDelta = 0, ColumnDelta = 1 },
            new Movement { RowDelta = 0, ColumnDelta = -1 },
        };
        private IEnumerable<SquareReference> GetAvailableMoves_Queen(Board board, Square square)
        {
            var start = square.Reference;
            var piece = square.Piece;

            var opponentColor = piece.Color == Color.Black ? Color.White : Color.Black;

            var moves = QueenMovements
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

        private static Movement[] KingMovements = new[]
        {
            new Movement { RowDelta = 1, ColumnDelta = 1 },
            new Movement { RowDelta = 1, ColumnDelta = -1 },
            new Movement { RowDelta = -1, ColumnDelta = 1 },
            new Movement { RowDelta = -1, ColumnDelta = -1 },
            new Movement { RowDelta = 1, ColumnDelta = 0 },
            new Movement { RowDelta = -1, ColumnDelta = 0 },
            new Movement { RowDelta = 0, ColumnDelta = 1 },
            new Movement { RowDelta = 0, ColumnDelta = -1 },
        };
        private IEnumerable<SquareReference> GetAvailableMoves_King(Board board, Square square)
        {
            var start = square.Reference;
            var piece = square.Piece;

            var ownColor = piece.Color;

            var moves = KingMovements
                            .Select(m => start.Move(m.RowDelta, m.ColumnDelta))
                            .WhereNotNull()
                            .Where(m => board[m].Piece.Color != ownColor);
            return moves;
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
