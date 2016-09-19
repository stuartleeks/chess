using Chess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.Tests
{
    public class BoardTests
    {
        [Fact]
        public void DumpEmptyBoard()
        {
            var board = Board.CreateEmptyBoard();

            string boardDump = board.Dump();

            Console.WriteLine(board);

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

            Console.WriteLine(board);

            //                                 xx xx xx xx xx xx xx xx
            const string expectedEmptyBoard = "Br Bk Bb Bq BK Bb Bk Br\r\n" +
                                              "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "                       \r\n" +
                                              "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" +
                                              "Wr Wk Wb Wq WK Wb Wk Wr\r\n";

            Assert.Equal(expectedEmptyBoard, boardDump);
        }
    }
}
