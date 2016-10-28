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
    //    - Promotion!
    //  - Rook
    //    - castling!! (need to track whether pieces have moved in Game. Add tests for this ;-) )
    //       From: https://en.wikipedia.org/wiki/Castling
    //
    //          Castling is permissible if and only if all of the following conditions hold(Schiller 2003:19):
    //              1. The king and the chosen rook are on the player's first rank.[3]
    //              2. Neither the king nor the chosen rook has previously moved.
    //              3. There are no pieces between the king and the chosen rook.
    //              4. The king is not currently in check.
    //              5. The king does not pass through a square that is attacked by an enemy piece.[4]
    //              6. The king does not end up in check. (True of any legal move.)
    //  - Draws (https://en.wikipedia.org/wiki/Chess#End_of_the_game)
    //    - Draw by agreement – draws are most commonly reached by mutual agreement between the players.The correct procedure is to verbally offer the draw, make a move, then start the opponent's clock. Traditionally players have been allowed to agree a draw at any time in the game, occasionally even without playing a move; in recent years efforts have been made to discourage short draws, for example by forbidding draw offers before move thirty.
    //    - Stalemate – the player whose turn it is to move is not in check, but has no legal move.
    //    - Threefold repetition of a position – this most commonly occurs when neither side is able to avoid repeating moves without incurring a disadvantage.In this situation, either player may claim a draw; this requires the players to keep a valid written record of the game so that the claim may be verified by the arbiter if challenged.The three occurrences of the position need not occur on consecutive moves for a claim to be valid.FIDE rules make no mention of perpetual check; this is merely a specific type of draw by threefold repetition.
    //    - The fifty-move rule – if during the previous 50 moves no pawn has been moved and no capture has been made, either player may claim a draw, as for the threefold-repetition rule. There are in fact several known endgames where it is theoretically possible to force a mate but which require more than 50 moves before the pawn move or capture is made; examples include some endgames with two knights against a pawn and some pawnless endgames such as queen against two bishops. These endings are rare, however, and few players study them in detail, so the fifty-move rule is considered practical for over the board play. Some correspondence chess organizations allow exceptions to the fifty-move rule.[note 2]
    //    - Fivefold repetition of a position, similar to the threefold-repetition rule, but in this case no player needs to claim the draw for the game to be drawn.This rule took effect on 1 July 2014. It establishes that there is a theoretical upper bound on the length of lawful chess games.
    //    - The seventy-five-move rule, similar to the fifty-move rule; however, if the final move in the sequence resulted in checkmate, this takes precedence. As for the fivefold-repetition rule, this applies independently of claims by the players.The rule also took effect on 1 July 2014 and also establishes, independently, an upper bound on the game length
    //    - Insufficient material – a player may claim a draw if their opponent has insufficient material to checkmate, for example if the player has only the king left and the opponent has only the king and a bishop.Such a claim is only valid if checkmate is impossible.Under the revised rule that took effect on 1 July 2009, which only refers to the impossibility of reaching checkmate without explicitly relating this to the players' material, the game is ended immediately in a draw, not requiring a claim by a player.
    //         - insufficient material (https://en.wikipedia.org/wiki/Glossary_of_chess#Insufficient_material) : An endgame scenario in which all pawns have been captured, and one side has only its king remaining while the other has only its king, a king plus a knight, or a king plus a bishop.A king plus bishop versus a king plus bishop with the bishops on the same color is also a draw, since neither side can checkmate, regardless of play.Situations where checkmate is possible only if the inferior side blunders are covered by the fifty-move rule.See Draw (chess)#Draws in all games.


    public class GameMoves_PawnTests
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


        private string _board;
        private void GivenBoard(string board)
        {
            _board = board;
        }
        private List<SquareReference[]> _moves = new List<SquareReference[]>();
        private void AndMove(SquareReference from, SquareReference to)
        {
            _moves.Add(new[] { from, to });
        }
        private SquareReference _selectedSquare;
        private void WhenSelectedPieceIs(string selectedSquare)
        {
            _selectedSquare = selectedSquare;
        }
        private Color? _currentTurn = null;
        private void WithCurrentTurn(Color currentTurn)
        {
            _currentTurn = currentTurn;
        }
        private void ThenMovesAre(params string[] moves)
        {
            // order expected moves to simplify comparison
            var expectedMoves = moves
                                    .OrderBy(m => m)
                                    .Select(m => (SquareReference)m)
                                    .ToList();

            // create game (parse board and apply moves)
            var board = Board.Parse(_board);
            if (_currentTurn == null)
            {
                if (_moves.Count > 0)
                {
                    throw new ArgumentException("If setting moves from starting board you must set the starting color"); // can't replay the moves until we set up the game...

                }
                var selectedPiece = board[_selectedSquare].Piece;
                if (selectedPiece.Color == Color.Empty)
                {
                    throw new ArgumentException("Selected square must contain a piece");
                }
                _currentTurn = selectedPiece.Color;
            }

            var game = new Game("testgame", _currentTurn.Value, board);
            Console.WriteLine("Foo");
            foreach (var move in _moves)
            {
                game = game.MakeMove(move[0], move[1]);
            }

            Console.WriteLine("board:");
            Console.WriteLine(game.Board.Dump());
            // Assert that actual moves match expectation
            var actualMoves = game
                                .GetAvailableMoves(_selectedSquare)
                                .OrderBy(s => s.ToString())
                                .ToList();

            Assert.Equal(expectedMoves, actualMoves);
        }
    }
}
