using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common = Chess.Common;

namespace Chess.Web.Models.Game
{
    public class HomeModel
    {
        public string HostName { get; set; }
        public string BuildNumber { get; set; }
    }
}