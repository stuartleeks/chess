using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chess.Common.Tests
{
    public class MovePieceTests
    {
        [Fact]
        public void MovePawnFromStart()
        {
            //                             0  1  2  3  4  5 6  7
            const string startingBoard = "Br Bk Bb Bq BK Bb Bk Br\r\n" + //0
                                         "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //1
                                         "                       \r\n" + //2
                                         "                       \r\n" + //3
                                         "                       \r\n" + //4
                                         "                       \r\n" + //5
                                         "Wp Wp Wp Wp Wp Wp Wp Wp\r\n" + //6
                                         "Wr Wk Wb Wq WK Wb Wk Wr\r\n";  //7
            var board = Board.Parse(startingBoard);
            board.MovePiece(new[] {6, 4 }, new[] {5, 4 });



            string boardDump = board.Dump();
            const string expectedBoard = "Br Bk Bb Bq BK Bb Bk Br\r\n" + //0
                                         "Bp Bp Bp Bp Bp Bp Bp Bp\r\n" + //1
                                         "                       \r\n" + //2
                                         "                       \r\n" + //3
                                         "                       \r\n" + //4
                                         "            Wp         \r\n" + //5
                                         "Wp Wp Wp Wp    Wp Wp Wp\r\n" + //6
                                         "Wr Wk Wb Wq WK Wb Wk Wr\r\n";  //7

            Assert.Equal(expectedBoard, boardDump);
        }
    }
}
