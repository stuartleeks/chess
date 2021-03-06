﻿using Chess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.UnitTests
{
    public class BoardInitializationTests
    {
        [Fact]
        public void DumpEmptyBoard()
        {
            var board = Board.CreateEmptyBoard();

            string boardDump = board.Dump();

            //                                 xx xx xx xx xx xx xx xx
            const string expectedEmptyBoard = "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n";

            Assert.Equal(expectedEmptyBoard, boardDump);
        }


        [Fact]
        public void DumpStartingBoard()
        {
            var board = Board.CreateStartingBoard();

            string boardDump = board.Dump();

            //                                    xx xx xx xx xx xx xx xx
            const string expectedStartingBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" +
                                                 "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" +
                                                 "                       \r\n" +
                                                 "                       \r\n" +
                                                 "                       \r\n" +
                                                 "                       \r\n" +
                                                 "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" +
                                                 "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";

            Assert.Equal(expectedStartingBoard, boardDump);
        }


        [Fact]
        public void ParsingBoard()
        {
            //                            xx xx xx xx xx xx xx xx
            const string expectedBoard = "Br Bn Bb Bq Bk Bb Bn Br\r\n" +
                                         "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" +
                                         "                       \r\n" +
                                         "                       \r\n" +
                                         "                       \r\n" +
                                         "                       \r\n" +
                                         "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" +
                                         "Wr Wn Wb Wq Wk Wb Wn Wr\r\n";

            var board = Board.Parse(expectedBoard);

            string boardDump = board.Dump();

            Assert.Equal(expectedBoard, boardDump);
        }
    }
}
