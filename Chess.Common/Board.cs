using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Common
{
    public class Board
    {
        public Board()
        {
        }

        public static Board CreateEmptyBoard()
        {
            return new Board
            {
                Squares = new BoardSquare[8][]
                {
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                }
            };
        }
        public static Board CreateStartingBoard()
        {
            return new Board
            {
                Squares = new BoardSquare[8][]
                {
                    (new [] { 2, 3, 4, 5, 6, 4, 3, 2 }).Select(i=>new BoardSquare(Color.Black, (Piece)i)).ToArray(),
                    Enumerable.Range(1,8).Select(_=>new BoardSquare(Color.Black, Piece.Pawn)).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>BoardSquare.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>new BoardSquare(Color.White, Piece.Pawn)).ToArray(),
                    (new [] { 2, 3, 4, 5, 6, 4, 3, 2 }).Select(i=>new BoardSquare(Color.White, (Piece)i)).ToArray()
                }
            };
        }
        public static Board Parse(string board)
        {
            var rows = board.Split('\r', '\n').Where(r => !string.IsNullOrEmpty(r)).ToArray();
            if (rows.Length != 8)
            {
                throw new ArgumentException($"Should be 8 rows! (got {rows.Length})");
            }
            var boardRows = new List<BoardSquare[]>();
            for (int rowIndex = 0; rowIndex < 8; rowIndex++)
            {
                var row = rows[rowIndex];
                var boardRow = new List<BoardSquare>();
                if (row.Length != (8 * 3) - 1)
                {
                    throw new ArgumentException($"Rows must be 23 chars. Row {rowIndex} was {row.Length}");
                }
                for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                {
                    string squareString = row.Substring(columnIndex * 3, 2);
                    boardRow.Add(ParseSquare(squareString));
                }
                boardRows.Add(boardRow.ToArray());
            }
            return new Board { Squares = boardRows.ToArray() };
        }
        private static BoardSquare ParseSquare(string squareString)
        {
            int colorValue = Array.IndexOf(ColorMap, squareString[0]);
            if (colorValue < 0)
            {
                throw new ArgumentException($"Invalid colour value in square {squareString}");
            }
            int pieceValue = Array.IndexOf(PieceMap, squareString[1]);
            if (pieceValue < 0)
            {
                throw new ArgumentException($"Invalid piece value in square {squareString}");
            }
            return new BoardSquare((Color)colorValue, (Piece)pieceValue);
        }

        public BoardSquare[][] Squares { get; set; }

        public string Dump()
        {
            // output the board with pieces as p (pawn), r (rook), k (knight), b (bishop), q (queen), K (king)
            // and colours B (black), W ( white)
            // Starting board is then
            //  Br Bk Bb Bq BK Bb Bk Br
            //  Bp Bp Bp Bp Bp Bp Bp Bp 
            // [ 6 rows of spaces ]
            //  Wp Wp Wp Wp Wp Wp Wp Wp 
            //  Wr Wk Wb Wq WK Wb Wk Wr

            StringBuilder buf = new StringBuilder((8 * 3) * 8);
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    var square = Squares[row][column];
                    buf.Append(DumpColor(square));
                    buf.Append(DumpPiece(square));
                    if (column < 7)
                    {
                        buf.Append(" ");
                    }
                }
                buf.AppendLine();
            }
            return buf.ToString();
        }

        private static readonly char[] ColorMap = new[] { ' ', 'W', 'B' };
        private char DumpColor(BoardSquare square)
        {
            return ColorMap[(int)square.Color];
        }
        private static readonly char[] PieceMap = new[] { ' ', 'p', 'r', 'k', 'b', 'q', 'K' };
        private char DumpPiece(BoardSquare square)
        {
            return PieceMap[(int)square.Piece];
        }
    }
    public struct BoardSquare
    {
        public static BoardSquare Empty = new BoardSquare(Color.Empty, Piece.Empty);
        public BoardSquare(Color color, Piece piece)
        {
            Color = color;
            Piece = piece;
        }
        public Color Color { get; private set; }
        public Piece Piece { get; private set; }
    }
    public enum Color
    {
        Empty = 0,
        White = 1,
        Black = 2
    }
    public enum Piece
    {
        Empty = 0,
        Pawn = 1,
        Rook = 2,
        Knight = 3,
        Bishop = 4,
        Queen = 5,
        King = 6
    }
}
