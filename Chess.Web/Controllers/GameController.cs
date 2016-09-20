using Chess.Web.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chess.Web.Controllers
{
    [RoutePrefix("")]
    public class GameController : Controller
    {
        [Route("")]
        public ActionResult ShowGame()
        {
            var model = new GameModel
            {
                Game = new Common.Game
                {
                    Board = Common.Board.CreateStartingBoard()
                }
            };
            return View(model);
        }
    }
}
