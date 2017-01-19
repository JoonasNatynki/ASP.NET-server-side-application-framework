using GameWebApi.Models;

namespace GameWebApi.MaintenanceProcessing
{
    public interface IPlayerMaintenanceProcessor
    {
        Player[] GetAll();
        Player Get(string id);
        Player Create(NewPlayer newPlayer);
        Player[] MinScore(int score);
        void AddItem(Data.Item item, string playerId);
        void BanPlayer(string playerId, bool ban);
    }
}