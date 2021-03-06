﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.UnitTests
{
    public class GameMoves_CheckTests : GameMoves_TestBase
    {
        [Fact]
        public void ExposedCheck()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
            "                       \r\n" + //6
            "                     Wb\r\n" + //5
            "                       \r\n" + //4
            "         Wp            \r\n" + //3
            "Wp Wp Wp    Wp Wp Wp Wp\r\n" + //2
            "Wr Wn    Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("f7"); // can't move pawn as it would expose check
            ThenMovesAre();
        }

        [Fact]
        public void NotEscapingCheck()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp Bp Bp    Bp Bp\r\n" + //7
            "               Bp      \r\n" + //6
            "                     Wb\r\n" + //5
            "                       \r\n" + //4
            "         Wp            \r\n" + //3
            "Wp Wp Wp    Wp Wp Wp Wp\r\n" + //2
            "Wr Wn    Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("f6"); // in check, and not escaping with this piece
            ThenMovesAre();
        }

        [Fact]
        public void CaptureAttackingPiece()
        {
            GivenBoard(
            //a  b  c  d  e  f  g  h
            "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
            "Bp Bp Bp       Bp Bp Bp\r\n" + //7
            "         Bp            \r\n" + //6
            "            Wr         \r\n" + //5
            "                       \r\n" + //4
            "                       \r\n" + //3
            "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
            "   Wn Wb Wq Wk Wb Wn Wr\r\n"   //1
            );
            WhenSelectedPieceIs("d6"); // in check, capture attacking piece
            ThenMovesAre("e5");
        }
    }
}
