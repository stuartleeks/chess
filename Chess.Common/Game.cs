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
        public bool CurrentPlayerInCheck { get { return IsInCheck(this, CurrentTurn); } }

        private List<Move> _moves;
        public IList<Move> Moves
        {
            get { return _moves.AsEnumerable().ToList(); }
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

        public Game MakeMove(SquareReference from, SquareReference to)
        {
            return MakeMove_Internal(from, to, suppressValidityCheck: false);
        }
        private Game MakeMove_Internal(SquareReference from, SquareReference to, bool suppressValidityCheck)
        {
            if (!suppressValidityCheck) // GetAvailable moves calls this, so this flag avoids StackOverflowException ;-)
            {
                if (Board[from].Piece.Color != CurrentTurn
                    || !GetAvailableMoves(from).Contains(to))
                {
                    throw new ArgumentException("Invalid move");
                }
            }

            var capturedPiece = Board[to].Piece;
            Square square = Board[from];
            var newGame = new Game(
                id: Id,
                currentTurn: (CurrentTurn == Color.Black) ? Color.White : Color.Black,
                board: Board.MovePiece(from, to),
                moves: _moves.Concat(new[] { new Move(DateTime.UtcNow, square.Piece, from, to, capturedPiece) }).ToList()
            );
            return newGame;
            // TODO - add a flag for whether the current player is in check
        }

        public IEnumerable<SquareReference> GetAvailableMoves(SquareReference from)
        {
            var initialMoves = GetAvailableMoves_NoCheckTest(this, from);

            var square = Board[from];
            var piece = square.Piece;


            // filter out moves that result in check
            // brute force approach ;-)
            // for each move, check whether there are any moves for any opponent piece after that
            // which would end on the current player's king's square
            // if so then we have a move that puts the player into check, so filter out
            var nonCheckMoves = initialMoves.Where(move =>
            {
                var newGame = MakeMove_Internal(from: square.Reference, to: move, suppressValidityCheck: true);

                return !IsInCheck(newGame, piece.Color);
            });

            return nonCheckMoves;
        }

        private bool IsInCheck(Game game, Color colorToTest)
        {
            var board = game.Board;
            var opponentColor = colorToTest == Color.Black ? Color.White : Color.Black;
            var kingSquare = board.FindPiece(colorToTest, PieceType.King).Value;

            var opponentPieceReferences = board.AllSquares()
                                    .Where(s => s.Piece.Color == opponentColor)
                                    .Select(s => s.Reference);

            return opponentPieceReferences.Any(
                    squareReference => GetAvailableMoves_NoCheckTest(game, squareReference)
                                                .Any(end => end == kingSquare.Reference)
                );
        }


        private IEnumerable<SquareReference> GetAvailableMoves_NoCheckTest(Game game, SquareReference from)
        {
            var square = game.Board[from];
            var pieceType = square.Piece.PieceType;
            switch (pieceType)
            {
                case PieceType.Pawn:
                    return GetAvailableMoves_Pawn(game, square);
                case PieceType.Rook:
                    return GetAvailableMoves_Rook(game, square);
                case PieceType.Knight:
                    return GetAvailableMoves_Knight(game, square);
                case PieceType.Bishop:
                    return GetAvailableMoves_Bishop(game, square);
                case PieceType.Queen:
                    return GetAvailableMoves_Queen(game, square);
                case PieceType.King:
                    return GetAvailableMoves_King(game, square);
                default:
                    throw new InvalidOperationException($"Unhandled piece type!! {pieceType}");
            }
        }

        private class Movement
        {
            public int RowDelta { get; set; }
            public int ColumnDelta { get; set; }
        }
        private static Movement[] PawnEnPassantMovements = new[]
        {
            new Movement { RowDelta = 1, ColumnDelta = 1 },
            new Movement { RowDelta = 1, ColumnDelta = -1 },
        };
        private IEnumerable<SquareReference> GetAvailableMoves_Pawn(Game game, Square square)
        {
            return GetAvailableMoves_Pawn_Inner(game, square).Distinct();
        }
        private IEnumerable<SquareReference> GetAvailableMoves_Pawn_Inner(Game game, Square square)
        {
            var board = game.Board;
            var start = square.Reference;
            var piece = square.Piece;

            var homeRow = piece.Color == Color.Black ? 1 : 6;
            var direction = piece.Color == Color.Black ? 1 : -1; // row 0 at top (black start)
            var opponentColor = piece.Color == Color.Black ? Color.White : Color.Black;
            var opponentHomeRow = opponentColor == Color.Black ? 1 : 6;

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

            if (square.Reference.Row == opponentHomeRow + -2 * direction) {
                // we're two rows away from the opponent home row, so consider en passant
                var enpassantMoves = PawnEnPassantMovements
                                            .Select(m => new {
                                                endLocation = start.Move(m.RowDelta * direction, m.ColumnDelta),
                                                enPassantPawnLocation = start.Move(0, m.ColumnDelta)
                                            })
                                            .Where(m =>
                                                m.endLocation != null
                                                    && m.enPassantPawnLocation != null
                                                    && board[m.enPassantPawnLocation.Value].Piece.Color == opponentColor
                                                    && board[m.enPassantPawnLocation.Value].Piece.PieceType == PieceType.Pawn
                                                    && (game.Moves.Count > 1
                                                            && game.Moves[game.Moves.Count - 1].End == m.enPassantPawnLocation // check that the last move was for the pawn that we're considering
                                                            && game.Moves[game.Moves.Count - 1].Start.Row == opponentHomeRow // piece started on its home row
                                                    )
                                            );
                foreach (var move in enpassantMoves)
                {
                    yield return move.endLocation.Value;
                }
            }
        }


        private static Movement[] RookMovements = new[]
        {
            new Movement { RowDelta = 1, ColumnDelta = 0 },
            new Movement { RowDelta = -1, ColumnDelta = 0 },
            new Movement { RowDelta = 0, ColumnDelta = 1 },
            new Movement { RowDelta = 0, ColumnDelta = -1 },
        };
        private IEnumerable<SquareReference> GetAvailableMoves_Rook(Game game, Square square)
        {
            var board = game.Board;
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
        private IEnumerable<SquareReference> GetAvailableMoves_Knight(Game game, Square square)
        {
            var board = game.Board;
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
        private IEnumerable<SquareReference> GetAvailableMoves_Bishop(Game game, Square square)
        {
            var board = game.Board;
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
        private IEnumerable<SquareReference> GetAvailableMoves_Queen(Game game, Square square)
        {
            var board = game.Board;
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
        private IEnumerable<SquareReference> GetAvailableMoves_King(Game game, Square square)
        {
            var board = game.Board;
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
        public Move(DateTime moveTimeUtc, Piece piece, SquareReference start, SquareReference end, Piece capturedPiece)
        {
            MoveTimeUtc = moveTimeUtc;
            Piece = piece;
            Start = start;
            End = end;
            CapturedPiece = capturedPiece;
        }

        public DateTime MoveTimeUtc { get; private set; }
        public Piece Piece { get; private set; }
        public SquareReference Start { get; private set; }
        public SquareReference End { get; private set; }
        public Piece CapturedPiece { get; private set; }

        public Move Clone()
        {
            return new Move(MoveTimeUtc, Piece, Start, End, CapturedPiece);
        }
    }


}
