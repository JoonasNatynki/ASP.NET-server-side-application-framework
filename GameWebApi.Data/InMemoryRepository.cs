using System;
using System.Collections.Generic;
using System.Linq;

namespace GameWebApi.Data
{
    public class InMemoryRepository : IRepository
    {
        private Dictionary<string, Player> _players = new Dictionary<string, Player>();

        public Player CreatePlayer(Player player)
        {
            _players.Add(player.Id, player);
            return player;
        }

        public Player GetPlayer(string playerId)
        {
            if (_players.ContainsKey(playerId))
            {
                return _players[playerId];
            }
            throw new ArgumentException("Player not found from memory");
        }

        public Player[] GetAllPlayers()
        {
            return _players.Values.ToArray();
        }

        public Item CreateItem(string playerId, Item item)
        {
            if (_players.ContainsKey(playerId))
            {
                Player player = _players[playerId];
                player.ItemsList.Add(item);
                return item;
            }
            throw new ArgumentException("Player not found from memory");
        }

        public Item GetItem(string playerId, string itemId)
        {
            if (_players.ContainsKey(playerId))
            {
                Player player = _players[playerId];
                Item item = player.ItemsList.FirstOrDefault(i => i.Id == itemId);
                if (item != null)
                {
                    return item;
                }
                throw new ArgumentException("Item was not found");
            }
            throw new ArgumentException("Player not found from memory");
        }


        public Item[] GetAllItems(string playerId)
        {
            if (_players.ContainsKey(playerId))
            {
                Player player = _players[playerId];
                return player.ItemsList.ToArray();
            }
            throw new ArgumentException("Player not found from memory");
        }

        public Player[] MinScore(int score)
        {
            throw new ArgumentException("Player not found from memory");
        }
        public void AddItem(Item item, string playerId)
        {
            throw new ArgumentException("Player not found from memory");
        }

        public void BanPlayer(string playerId, bool ban)
        {
            throw new ArgumentException("Player not found from memory");
        }
    }
}