using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Common
{
    // TODO - consider supporting https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation
    public class Board
    {
        public Square[][] Squares { get; set; }

        public Board()
        {
        }

        public static Board CreateEmptyBoard()
        {
            return new Board
            {
                Squares = new Square[8][]
                {
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(0, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(1, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(2, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(3, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(4, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(5, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(6, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(7, i), Piece.Empty)).ToArray(),
                }
            };
        }


        public static Board CreateStartingBoard()
        {
            return new Board
            {
                Squares = new Square[8][]
                {
                    (new [] { 2, 3, 4, 5, 6, 4, 3, 2 })
                        .Select(
                            (i, index)=> Square.Create(
                                    SquareReference.FromRowColumn(0, index),
                                    new Piece(Color.Black, (PieceType)i)
                                )
                        ).ToArray(),
                    Enumerable.Range(0,8)
                        .Select(
                            i=> Square.Create(
                                    SquareReference.FromRowColumn(1,i),
                                    new Piece(Color.Black, PieceType.Pawn)
                                )
                        ).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(2, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(3, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(4, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8).Select(i => Square.Create(SquareReference.FromRowColumn(5, i), Piece.Empty)).ToArray(),
                    Enumerable.Range(0,8)
                        .Select(
                            i => Square.Create(
                                    SquareReference.FromRowColumn(6,i),
                                    new Piece(Color.White, PieceType.Pawn)
                                )
                        ).ToArray(),
                    (new [] { 2, 3, 4, 5, 6, 4, 3, 2 })
                        .Select(
                            (i, index) => Square.Create(
                                    SquareReference.FromRowColumn(7, index),
                                    new Piece(Color.White, (PieceType)i)
                            )
                        ).ToArray()
                }
            };
        }
        public static Board Parse(string board)
        {
            var rows     = board.Split('\r', '\n').Where(r => !string.IsNullOrEmpty(r)).ToArray();
            if (rows.Length != 8)
            {
                throw new ArgumentException($"Should be 8 rows! (got {rows.Length})");
            }
            var boardRows = new List<Square[]>();
            for (int rowIndex = 0; rowIndex < 8; rowIndex++)
            {
                var row = rows[rowIndex];
                var boardRow = new List<Square>();
                if (row.Length != (8 * 3) - 1)
                {
                    throw new ArgumentException($"Rows must be 23 chars. Row {rowIndex} was {row.Length}");
                }
                for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                {
                    string pieceString = row.Substring(columnIndex * 3, 2);
                    var square = Square.Create(
                            SquareReference.FromRowColumn(rowIndex, columnIndex),
                            ParsePiece(pieceString)
                        );
                    boardRow.Add(square);
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
            var piece = Squares[from.Row][from.Column].Piece;
            Squares[from.Row][from.Column].Piece = Piece.Empty;
            Squares[to.Row][to.Column].Piece = piece;
        }
        public string Dump()
        {
            // output the board with pieces as p (pawn), r (rook), n (knight), b (bishop), q (queen), k (king)
            // and colours B (black), W ( white)
            // Starting board is then
            //  Br Bn Bb Bq Bk Bb Bn Br
            //  Bp Bp Bp Bp Bp Bp Bp Bp 
            // [ 6 rows of spaces ]
            //  Wp Wp Wp Wp Wp Wp Wp Wp 
            //  Wr Wn Wb Wq Wk Wb Wn Wr

            StringBuilder buf = new StringBuilder((8 * 3) * 8);
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    var square = Squares[row][column];
                    buf.Append(DumpColor(square.Piece));
                    buf.Append(DumpPiece(square.Piece));
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
        private static readonly char[] PieceMap = new[] { ' ', 'p', 'r', 'n', 'b', 'q', 'k' };
        private char DumpPiece(Piece square)
        {
            return PieceMap[(int)square.PieceType];
        }
    }
    public struct SquareReference
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public static SquareReference FromRowColumn(int row, int column)
        {
            return new SquareReference { Row = row, Column = column };
        }

        private static char[] ColumnReferences = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
        public static implicit operator SquareReference(string value)
        {
            if (value.Length == 2)
            {
                char columnChar = value[0];
                char rowChar = value[1];
                int column = Array.IndexOf(ColumnReferences, columnChar);
                int row;

                if (int.TryParse(rowChar.ToString(), out row)
                    && column > 0 && column <= 8
                    && row > 0 && row <= 8)
                {
                    return new SquareReference { Row = 8 - row, Column = column };
                }
            }
            throw new ArgumentException("string format for SquareReference must be 'e4' etc");
        }
        public static implicit operator SquareReference(int[] value) // row, column (0-7)
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
        public override string ToString()
        {
            if (Column < 0 || Column >= 8)
            {
                throw new ArgumentException($"Column value '{Column}' is outside the allowed range");
            }
            return ColumnReferences[Column] + (8- Row).ToString();
        }
        public override bool Equals(Object obj)
        {
            return obj is SquareReference && this == (SquareReference)obj;
        }
        public override int GetHashCode()
        {
            return Row + 17 * Column;
        }
        public static bool operator ==(SquareReference x, SquareReference y)
        {
            return x.Row == y.Row && x.Column == y.Column;
        }
        public static bool operator !=(SquareReference x, SquareReference y)
        {
            return !(x == y);
        }
    }
    public struct Square
    {
        public SquareReference Reference { get; set; }
        public Piece Piece { get; set; }
        public static Square Create(SquareReference reference, Piece piece)
        {
            return new Square
            {
                Reference = reference,
                Piece = piece
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
