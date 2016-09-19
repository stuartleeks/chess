using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Common
{
    public class Game
    {

    }

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
                    if (column<7)
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
