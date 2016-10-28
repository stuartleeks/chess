using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.UnitTests
{
    public class GameMoves_PawnTests : GameMoves_TestBase
    {
        [Fact]
        public void StartingPosition_White()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e2");
            ThenMovesAre("e3", "e4");
        }
        [Fact]
        public void StartingPosition_Black()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e7");
            ThenMovesAre("e6", "e5");
        }
        [Fact]
        public void PawnAlreadyMoved()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "            Wp         \r\n" + //3
            "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e3");
            ThenMovesAre("e4");
        }

        [Fact]
        public void NoMoves()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp    Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "            Bp         \r\n" + //5
            "            Wp         \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e4");
            ThenMovesAre();
        }

        [Fact]
        public void CaptureOne()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp    Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "         Bp            \r\n" + //4
            "            Wp         \r\n" + //3
            "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e3");
            ThenMovesAre("d4", "e4");           // TODO - is pawn force to take?
        }

        [Fact]
        public void CaptureTwo()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp    Bp    Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "         Bp    Bp      \r\n" + //5
            "            Wp         \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e4");
            ThenMovesAre("d5", "e5", "f5");     // TODO - is pawn force to take?
        }

        [Fact]
        public void EnPassant_WhenPawnJustMovedPast_ThenPawnCanCaptureEnPassant()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );

            AndMove("e2", "e4");
            AndMove("a7", "a6");

            AndMove("e4", "e5");
            AndMove("d7", "d5"); //pawn moved two spaces past opponent pawn

            WithCurrentTurn(Color.White);
            WhenSelectedPieceIs("e5");
            ThenMovesAre("d6", "e6");
        }

        [Fact]
        public void EnPassant_WhenAnotherMoveHasBeenMade_ThenPawnCannotCaptureEnPassant()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            AndMove("e2", "e4");
            AndMove("a7", "a6");

            AndMove("e4", "e5");
            AndMove("d7", "d5"); //pawn moved two spaces past opponent pawn

            AndMove("h2", "h4");
            AndMove("h7", "h5");

            WithCurrentTurn(Color.White);
            WhenSelectedPieceIs("e5");
            ThenMovesAre("e6");
        }
    }
}
