using Chess.Web.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common = Chess.Common;

namespace Chess.Web.Controllers
{
    [RoutePrefix("")]
    public class GameController : Controller
    {
        [Route("")]
        public ActionResult ShowGame()
        {
            var game = new Common.Game
            {
                Board = Common.Board.CreateStartingBoard()
            };

            var model = new GameModel
            {
                // TODO - remove Game property
                Game = game,
                Board = MapToModel(game.Board)
            };
            return View(model);
        }

        static readonly string[] SquareColors = new[] { "white", "black" };
        private Board MapToModel(Common.Board board)
        {
            
            return new Board
            {
                Squares = board.Squares
                                .Select((row, rowIndex) =>
                                    row.Select((square, columnIndex) => new BoardSquare
                                    {
                                        PieceImage = ImageNameFromPiece(square),
                                        PieceName = square.Color + " " + square.PieceType,
                                        SquareColour = SquareColors[(rowIndex + columnIndex) % 2]  
                                    })
                                    .ToArray()
                                ).ToArray()
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
