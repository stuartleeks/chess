using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Common
{
    public class Board
    {
        public Piece[][] Squares { get; set; }

        public Board()
        {
        }

        public static Board CreateEmptyBoard()
        {
            return new Board
            {
                Squares = new Piece[8][]
                {
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                }
            };
        }


        public static Board CreateStartingBoard()
        {
            return new Board
            {
                Squares = new Piece[8][]
                {
                    (new [] { 2, 3, 4, 5, 6, 4, 3, 2 }).Select(i=>new Piece(Color.Black, (PieceType)i)).ToArray(),
                    Enumerable.Range(1,8).Select(_=>new Piece(Color.Black, PieceType.Pawn)).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>Piece.Empty).ToArray(),
                    Enumerable.Range(1,8).Select(_=>new Piece(Color.White, PieceType.Pawn)).ToArray(),
                    (new [] { 2, 3, 4, 5, 6, 4, 3, 2 }).Select(i=>new Piece(Color.White, (PieceType)i)).ToArray()
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
            var boardRows = new List<Piece[]>();
            for (int rowIndex = 0; rowIndex < 8; rowIndex++)
            {
                var row = rows[rowIndex];
                var boardRow = new List<Piece>();
                if (row.Length != (8 * 3) - 1)
                {
                    throw new ArgumentException($"Rows must be 23 chars. Row {rowIndex} was {row.Length}");
                }
                for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                {
                    string pieceString = row.Substring(columnIndex * 3, 2);
                    boardRow.Add(ParsePiece(pieceString));
                }
                boardRows.Add(boardRow.ToArray());
            }
            return new Board { Squares = boardRows.ToArray() };
        }
        private static Piece ParsePiece(string pieceString)
        {
            int colorValue = Array.IndexOf(ColorMap, pieceString[0]);
            if (colorValue < 0)
            {
                throw new ArgumentException($"Invalid colour value in square {pieceString}");
            }
            int pieceTypeValue = Array.IndexOf(PieceMap, pieceString[1]);
            if (pieceTypeValue < 0)
            {
                throw new ArgumentException($"Invalid pieceType value in square {pieceString}");
            }
            return new Piece((Color)colorValue, (PieceType)pieceTypeValue);
        }


        public void MovePiece(SquareReference from, SquareReference to)
        {
            var piece = Squares[from.Row][from.Column];
            Squares[from.Row][from.Column] = Piece.Empty;
            Squares[to.Row][to.Column] = piece;
        }
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
        private char DumpColor(Piece square)
        {
            return ColorMap[(int)square.Color];
        }
        private static readonly char[] PieceMap = new[] { ' ', 'p', 'r', 'k', 'b', 'q', 'K' };
        private char DumpPiece(Piece square)
        {
            return PieceMap[(int)square.PieceType];
        }
    }
    public struct SquareReference
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public static implicit operator SquareReference(int[] value)
        {
            if (value.Length != 2)
            {
                throw new ArgumentException("Array cast to SquareReference must have 2 elements");
            }
            if (value[0] < 0 || value[0] >= 8
                || value[1] < 0 || value[1] >= 8)
            {
                throw new ArgumentException("Array values must be between 0 and 8");
            }
            return new SquareReference
            {
                Row = value[0],
                Column = value[1]
            };
        }
    }
    public struct Piece
    {
        public static Piece Empty = new Piece(Color.Empty, PieceType.Empty);
        public Piece(Color color, PieceType pieceType)
        {
            Color = color;
            PieceType = pieceType;
        }
        public Color Color { get; private set; }
        public PieceType PieceType { get; private set; }
    }
    public enum Color
    {
        Empty = 0,
        White = 1,
        Black = 2
    }
    public enum PieceType
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
