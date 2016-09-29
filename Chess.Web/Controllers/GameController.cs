using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chess.Web.Models.Game;
using Chess.Web.Services;

namespace Chess.Web.Controllers
{
    [Route("")]
    public class GameController : Controller
    {
        private readonly IGameStore _gameStore;

        public GameController(IGameStore gameStore)
        {
            _gameStore = gameStore;
        }

        [HttpGet("", Name ="Home")]
        public IActionResult Home()
        {
            // return Ok("Hello");
            return View();
        }

        [HttpPost("play/new")]
        public IActionResult StartNew()
        {
            var game = Common.Game.CreateStartingGame();
            _gameStore.Save(game);
            return RedirectToAction(nameof(ChoosePiece), new { gameId = game.Id });
        }

        [HttpGet("play/{gameId}")]
        public IActionResult ChoosePiece(string gameId)
        {
            var game = _gameStore.GetGame(gameId);

            var model = MapToChoosePieceModel(game);
            return View("ShowGame", model);
        }

        // TODO - enable binding for SquareReference type
        [HttpGet("play/{gameId}/{pieceSquareRef}")]
        public IActionResult ChooseEndPosition(string gameId, string pieceSquareRef) // eg b3
        {
            var game = _gameStore.GetGame(gameId);
            var pieceReference = (Common.SquareReference)pieceSquareRef;

            var model = MapToChooseEndPositionModel(game, pieceReference);
            return View("ShowGame", model);
        }

        [HttpGet("play/{gameId}/{pieceSquareRef}/{endPosition}")]
        public IActionResult Confirm(string gameId, string pieceSquareRef, string endPosition)
        {
            // Called to prompt user to confirm
            var game = _gameStore.GetGame(gameId);
            var pieceReference = (Common.SquareReference)pieceSquareRef;
            var endPositionReference = (Common.SquareReference)endPosition;

            // Move the piece on our copy
            game.Board.MovePiece(pieceReference, endPositionReference);
            var model = MapToConfirmModel(game, pieceReference, endPositionReference);
            return View(model);
        }

        [HttpPost("play/{gameId}/{pieceSquareRef}/{endPosition}")]
        public IActionResult Confirmed(string gameId, string pieceSquareRef, string endPosition)
        {
            // Called when user has confirmed
            var game = _gameStore.GetGame(gameId);
            var pieceReference = (Common.SquareReference)pieceSquareRef;
            var endPositionReference = (Common.SquareReference)endPosition;

            // Move the piece and save
            game.MakeMove(pieceReference, endPositionReference);
            _gameStore.Save(game);

            return RedirectToAction(nameof(ChoosePiece));
        }

        static readonly string[] SquareColors = new[] { "white", "black" };
        private GameModel MapToChoosePieceModel(Common.Game game)
        {
            return new GameModel
            {
                CurrentPlayer = game.CurrentTurn,
                Board = new Board
                {
                    Squares = game.Board.Squares
                                .Select((row, rowIndex) =>
                                    row.Select((square, columnIndex) => {
                                        // highlight current player's pieces with moves
                                        bool canSelect = square.Piece.Color == game.CurrentTurn
                                                            && game.GetAvailableMoves(square.Reference).Any();
                                        string squareRef = square.Reference.ToString();
                                        return new BoardSquare
                                        {
                                            PieceImage = ImageNameFromPiece(square.Piece),
                                            PieceName = square.Piece.Color + " " + square.Piece.PieceType,
                                            SquareColour = SquareColors[(rowIndex + columnIndex) % 2],
                                            CanSelect = canSelect,
                                            SelectUrl = canSelect
                                                            ? Url.Action(nameof(ChooseEndPosition), new { pieceSquareRef = squareRef })
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
            Common.SquareReference[] availableMoves = game.GetAvailableMoves(selectedSquareReference).ToArray();
            return new GameModel
            {
                CurrentPlayer = game.CurrentTurn,
                Board = new Board
                {
                    Squares = game.Board.Squares
                                .Select((row, rowIndex) =>
                                    row.Select((square, columnIndex) =>
                                    {
                                        bool canSelect = availableMoves.Contains(square.Reference);
                                        string squareRef = square.Reference.ToString();
                                        string selectUrl = canSelect
                                                            ? Url.Action(nameof(Confirm), new { pieceSquareRef = selectedSquareReference.ToString(), endPosition = squareRef })
                                                            : null;
                                        return new BoardSquare
                                        {
                                            PieceImage = ImageNameFromPiece(square.Piece),
                                            PieceName = square.Piece.Color + " " + square.Piece.PieceType,
                                            SquareColour = SquareColors[(rowIndex + columnIndex) % 2],
                                            CanSelect = canSelect,
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
                CurrentPlayer = game.CurrentTurn,
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
