using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chess.Web.Models.Game;

namespace Chess.Web.Controllers
{
    [Route("")]
    public class GameController : Controller
    {
        // temporary hack while working out the rendering!
        static Common.Game TheGame = Common.Game.CreateStartingGame();

        [Route("", Name ="StartMove")]
        public IActionResult ChoosePiece()
        {
            var game = TheGame;

            var model = MapToChoosePieceModel(game);
            return View("ShowGame", model);
        }
        [Route("move/{pieceSquareRef}", Name = "ChooseEndPosition")]
        public IActionResult ChooseEndPosition(string pieceSquareRef) // eg b3
        {
            var game = TheGame;
            var pieceReference = (Common.SquareReference)pieceSquareRef;

            var model = MapToChooseEndPositionModel(game, pieceReference);
            return View("ShowGame", model);
        }
        [Route("move/{pieceSquareRef}/{endPosition}", Name = "Confirm")]
        public IActionResult Confirm(string pieceSquareRef, string endPosition)
        {
            var game = TheGame.Clone(); // clone to avoid modifying local state (in-memory game!)
            var pieceReference = (Common.SquareReference)pieceSquareRef;
            var endPositionReference = (Common.SquareReference)endPosition;

            // Move the piece (will be without saving when we implement persistence)
            game.Board.MovePiece(pieceReference, endPositionReference);
            var model = MapToConfirmModel(game, pieceReference, endPositionReference);
            return View(model);
        }


        static readonly string[] SquareColors = new[] { "white", "black" };
        private GameModel MapToChoosePieceModel(Common.Game game)
        {
            return new GameModel
            {
                Board = new Board
                {
                    Squares = game.Board.Squares
                                .Select((row, rowIndex) =>
                                    row.Select((square, columnIndex) => {
                                        bool canSelect = square.Piece.Color == game.CurrentTurn; // highlight current player's pieces
                                        string squareRef = square.Reference.ToString();
                                        return new BoardSquare
                                        {
                                            PieceImage = ImageNameFromPiece(square.Piece),
                                            PieceName = square.Piece.Color + " " + square.Piece.PieceType,
                                            SquareColour = SquareColors[(rowIndex + columnIndex) % 2],
                                            CanSelect = canSelect,
                                            SelectUrl = canSelect
                                                            ? Url.RouteUrl("ChooseEndPosition", new { pieceSquareRef = squareRef })
                                                            : null,
                                            ReferenceString = squareRef
                                        };
                                    })
                                    .ToArray()
                                ).ToArray()
                }
            };
        }

        private GameModel MapToChooseEndPositionModel(Common.Game game, Common.SquareReference selectedSquareReference)
        {
            //Common.SquareReference[] availableMoves = game.GetAvailableMoves();
            return new GameModel
            {
                Board = new Board
                {
                    Squares = game.Board.Squares
                                .Select((row, rowIndex) =>
                                    row.Select((square, columnIndex) =>
                                    {
                                        // highlight any space or opponent piece
                                        // TODO add proper available move calculation!!
                                        bool canSelect = square.Piece.Color != game.CurrentTurn;
                                        string squareRef = square.Reference.ToString();
                                        string selectUrl = canSelect
                                                            ? Url.RouteUrl("Confirm", new { pieceSquareRef = selectedSquareReference.ToString(), endPosition = squareRef })
                                                            : null;
                                        return new BoardSquare
                                        {
                                            PieceImage = ImageNameFromPiece(square.Piece),
                                            PieceName = square.Piece.Color + " " + square.Piece.PieceType,
                                            SquareColour = SquareColors[(rowIndex + columnIndex) % 2],
                                            CanSelect = square.Piece.Color != game.CurrentTurn,
                                            Reference = square.Reference,
                                            SelectUrl = selectUrl,
                                            ReferenceString = squareRef
                                        };
                                    })
                                    .ToArray()
                                ).ToArray(),
                    SelectedSquare = selectedSquareReference
                }
            };
        }
        private GameModel MapToConfirmModel(
            Common.Game game,
            Common.SquareReference selectedSquareReference,
            Common.SquareReference endPosition)
        {
            //Common.SquareReference[] availableMoves = game.GetAvailableMoves();
            return new GameModel
            {
                Board = new Board
                {
                    Squares = game.Board.Squares
                                .Select((row, rowIndex) =>
                                    row.Select((square, columnIndex) =>
                                    {
                                        // highlight any space or opponent piece
                                        // TODO add proper available move calculation!!
                                        bool canSelect = square.Piece.Color != game.CurrentTurn;
                                        string squareRef = square.Reference.ToString();
                                        return new BoardSquare
                                        {
                                            PieceImage = ImageNameFromPiece(square.Piece),
                                            PieceName = square.Piece.Color + " " + square.Piece.PieceType,
                                            SquareColour = SquareColors[(rowIndex + columnIndex) % 2],
                                            CanSelect = false,
                                            Reference = square.Reference,
                                            ReferenceString = squareRef
                                        };
                                    })
                                    .ToArray()
                                ).ToArray(),
                    SelectedSquare = selectedSquareReference
                }
            };
        }
        static readonly Dictionary<Common.Color, char> ImageColors = new Dictionary<Common.Color, char>
        {
            {Common.Color.Black, 'd' },
            {Common.Color.White, 'l' },
        };
        static readonly Dictionary<Common.PieceType, char> ImagePieceTypes = new Dictionary<Common.PieceType, char>
        {
            {Common.PieceType.Pawn, 'p' },
            {Common.PieceType.Rook, 'r' },
            {Common.PieceType.Knight, 'n' },
            {Common.PieceType.Bishop, 'b' },
            {Common.PieceType.Queen, 'q' },
            {Common.PieceType.King, 'k' },
        };
        private string ImageNameFromPiece(Common.Piece piece)
        {
            if (piece.PieceType == Common.PieceType.Empty)
            {
                return null;
            }
            char lightDark = ImageColors[piece.Color];
            char pieceType = ImagePieceTypes[piece.PieceType];
            return $"200px-Chess_{pieceType}{lightDark}t45.svg.png";
        }
    }
}
