using Chess.Common;

namespace Chess.Web.Services
{
    public interface IGameStore
    {
        Game GetGame(string gameId);
        void Save(Game game);
    }
}