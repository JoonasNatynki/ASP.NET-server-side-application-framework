namespace GameWebApi.Data
{
    public interface IRepository
    {
        Player CreatePlayer(Player player);
        Player GetPlayer(string playerId);
        Player[] GetAllPlayers();
        Player[] MinScore(int score);

        Item CreateItem(string playerId, Item item);
        Item GetItem(string playerId, string itemId);
        Item[] GetAllItems(string playerId);
        void AddItem(Item item, string playerId);
        void BanPlayer(string playerId, bool ban);
    }
}