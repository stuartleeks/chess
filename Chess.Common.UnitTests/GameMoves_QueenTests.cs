using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.UnitTests
{
    public class GameMoves_QueenTests : GameMoves_TestBase
    {
        [Fact]
        public void MovesBlockedByOwnPiece_White()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //3
            "                       \r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("d1");
            ThenMovesAre("c2", "d2", "e2");
        }

        [Fact]
        public void MovesBlockedByOwnPiece_Black()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "                       \r\n" + //7
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("d8");
            ThenMovesAre("c7", "d7", "e7");
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
            "         Wq            \r\n" + //4
            "                       \r\n" + //3
            "   Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb    Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("d4");
            ThenMovesAre("e5", "f6", "g7",
                "c5", "b6", "a7",
                "c3",
                "e3",
                "d5", "d6", "d7",
                "d3",
                "a4", "b4", "c4", "e4", "f4", "g4", "h4");
        }
    }
}
