using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Common
{
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

        public static IEnumerable<Square> AllSquares(this Board board)
        {
            return board.Squares
                        .SelectMany(row => row);
        }
        public static IEnumerable<Square> FindPieces(this Board board, Color color, PieceType piece)
        {
            return board.AllSquares()
                        .Where(square =>
                                    square.Piece.Color == color
                                    && square.Piece.PieceType == piece);
        }
        public static Square? FindPiece(this Board board, Color color, PieceType piece)
        {
            return board.FindPieces(color, piece)
                .SingleOrDefault();
        }

    }
}
