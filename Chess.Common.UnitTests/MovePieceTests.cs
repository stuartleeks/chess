﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.UnitTests
{
    public class MovePieceTests
    {
        [Fact]
        public void MovePawnToEmptySquare()
        {
            //                             0  1  2  3  4  5 6  7
            const string startingBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //0
                                         "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //1
                                         "                       \r\n" + //2
                                         "                       \r\n" + //3
                                         "                       \r\n" + //4
                                         "                       \r\n" + //5
                                         "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //6
                                         "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";  //7
            var board = Board.Parse(startingBoard);
            var game = new Game("test", Color.White, board);
            game = game.MakeMove(new[] {6, 4 }, new[] {5, 4 });



            string boardDump = game.Board.Dump();
            const string expectedBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //0
                                         "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //1
                                         "                       \r\n" + //2
                                         "                       \r\n" + //3
                                         "                       \r\n" + //4
                                         "            Wp         \r\n" + //5
                                         "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //6
                                         "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";  //7

            Assert.Equal(expectedBoard, boardDump);
        }

        [Fact]
        public void MovePawnsToEmptySquares()
        {
            //                             a  b  c  d  e  f  g  h
            const string startingBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //8
                                         "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //7
                                         "                       \r\n" + //6
                                         "                       \r\n" + //5
                                         "                       \r\n" + //4
                                         "                       \r\n" + //3
                                         "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //2
                                         "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";  //1
            var board = Board.Parse(startingBoard);
            var game = new Game("test", Color.White, board);
            game = game.MakeMove("e2", "e3");
            game = game.MakeMove("d7", "d6");


            string boardDump = game.Board.Dump();
            const string expectedBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //0
                                         "Bp Bp Bp    Bp Bp Bp Bp\r\n" + //1
                                         "         Bp            \r\n" + //2
                                         "                       \r\n" + //3
                                         "                       \r\n" + //4
                                         "            Wp         \r\n" + //5
                                         "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //6
                                         "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";  //7

            Assert.Equal(expectedBoard, boardDump);
        }
        [Fact]
        public void MovePawnToTakePiece()
        {
            //                             0  1  2  3  4  5 6  7
            const string startingBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //0
                                         "Bp Bp Bp    Bp Bp Bp Bp\r\n" + //1
                                         "                       \r\n" + //2
                                         "         Bp            \r\n" + //3
                                         "            Wp         \r\n" + //4
                                         "                       \r\n" + //5
                                         "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //6
                                         "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";  //7
            var board = Board.Parse(startingBoard);
            board = board.MovePiece(new[] { 4, 4 }, new[] { 3, 3 });



            string boardDump = board.Dump();
            const string expectedBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" + //0
                                         "Bp Bp Bp    Bp Bp Bp Bp\r\n" + //1
                                         "                       \r\n" + //2
                                         "         Wp            \r\n" + //3
                                         "                       \r\n" + //4
                                         "                       \r\n" + //5
                                         "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //6
                                         "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";  //7

            Assert.Equal(expectedBoard, boardDump);
        }

    }
}
