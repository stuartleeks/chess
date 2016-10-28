using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.UnitTests
{
    public class GameMoves_KingTests : GameMoves_TestBase
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
            "   Wp Wp    Wp Wp Wp Wp\r\n" + //3
            "         Wp            \r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e1");
            ThenMovesAre("e2", "f2");
        }

        [Fact]
        public void MovesBlockedByOwnPiece_Black()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "         Bp            \r\n" + //7
            "Bp Bp Bp    Bp Bp Bp Bp\r\n" + //6
            "                       \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e8");
            ThenMovesAre("e7", "f7");
        }

        [Fact]
        public void MultipleDirections()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "   Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                       \r\n" + //5
            "         Bp Wk         \r\n" + //4
            "                       \r\n" + //3
            "   Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "Wr Wn Wb Wq    Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("e4");
            ThenMovesAre("d5", "e5", "f5",
                "d4", "f4",
                "d3", "f3");
        }
    }
}
