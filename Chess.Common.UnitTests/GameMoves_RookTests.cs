using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.UnitTests
{
    // TODO - test notes
    //  - All moves
    //    - need to test whether it leaves the king in check - if so not allowed!
    //  - Pawn
    //    - en passant (Look up!!)
    //    - take piece diagonally
    //  - Rook
    //    - castling!! (need to track whether pieces have moved in Game. Add tests for this ;-) )
    //    - forwards/backwards/sideways as many spaces up to  own piece, or up to and including opponent's piece
    //  - Knight
    //    -  L-shaped move into empty square or square with opponent's piece
    //  - Bishop
    //    - diagonally as many spaces up to own piece, or up to and including opponent piece
    //  - Queen
    //    - forwards/backwards/sideways as many spaces up to  own piece, or up to and including opponent's piece
    //    - diagonally as many spaces up to own piece, or up to and including opponent's piece
    //  - King
    //    - any direction one space into empty square or square with opponent's piece
    //


    public class GameMoves_RookTests
    {
        [Fact]
        public void ForwardMovesBlockedByOwnPiece_White()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "Wp                     \r\n" + //4
            "                       \r\n" + //3
            "   Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("a1");
            ThenMovesAre("a2", "a3");
        }

        [Fact]
        public void ForwardMovesBlockedByOwnPiece_Black()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "   Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "Bp                     \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("a8");
            ThenMovesAre("a7", "a6");
        }

        [Fact]
        public void TakeOpposition()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "   Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("a1");
            ThenMovesAre("a2", "a3", "a4", "a5", "a6", "a7");
        }

        [Fact]
        public void MultipleDirections()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "         Wr            \r\n" + //4
            "                       \r\n" + //3
            "   Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "   Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("d4");
            ThenMovesAre("d5", "d6", "d7",
                "d3",
                "c4", "b4", "a4",
                "e4", "f4", "g4", "h4");
        }

        private string _board;
        private void GivenBoard(string board)
        {
            _board = board;
        }
        private SquareReference _selectedSquare;
        private void WhenSelectedPieceIs(string selectedSquare)
        {
            _selectedSquare = selectedSquare;
        }
        private void ThenMovesAre(params string[] moves)
        {
            var expectedMoves = moves
                                    .OrderBy(m=>m)
                                    .Select(m => (SquareReference)m)
                                    .ToList();

            var board = Board.Parse(_board);
            var selectedPiece = board[_selectedSquare].Piece;
            if (selectedPiece.Color == Color.Empty)
            {
                throw new ArgumentException("Selected starting square must contain a piece");
            }
            var game = new Game("testgame", selectedPiece.Color, board);

            var actualMoves = game
                                .GetAvailableMoves(_selectedSquare)
                                .OrderBy(s=>s.ToString())
                                .ToList();

            Assert.Equal(expectedMoves, actualMoves);
        }
    }
}
