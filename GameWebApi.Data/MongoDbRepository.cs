using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameWebApi.Data
{
    public class MongoDbRepository : IRepository
    {
        private readonly IMongoCollection<Player> _collection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017"); // The URL can be empty, if database is in the machine
            var database = mongoClient.GetDatabase("game");
            _collection = database.GetCollection<Player>("players");
        }

        public Player CreatePlayer(Player player)
        {
            player.Id = ObjectId.GenerateNewId().ToString();
            _collection.InsertOne(player);
            return player;
        }

        /// <summary>
        /// Create filter for players that matches the player id
        /// FirstOrDefault returns the first match or null if there was no match
        /// </summary>
        public Player GetPlayer(string playerId)
        {
           // var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
            //IFindFluent<Player, Player> players = _collection.Find(filter);
            return _collection.Find(player => player.Id == playerId).FirstOrDefault();
        }

        public Player[] GetAllPlayers()
        {
            var list = _collection.Find(x => true).ToList();
            return list.ToArray();
        }

        /// <summary>
        /// Example on how to use filters and updates
        /// </summary>
        public void UpdateAllPlayerScoresTo10()
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Empty;
            var update = Builders<Player>.Update.Set(player => player.Score, 10);
            _collection.UpdateOne(filter, update);
        }

        public Item CreateItem(string playerId, Item item)
        {
            Data.Item itemi = new Data.Item()
            {
                Id = item.Id,
                Price = item.Price
            };

            var filter = Builders<Player>.Filter.Eq(e => e.Id, playerId);
            var update = Builders<Player>.Update.Push<Item>(e => e.ItemsList, itemi);
            _collection.FindOneAndUpdate(filter, update);

            return itemi;
        }

        public Item GetItem(string playerId, string itemId)
        {
            Data.Player player = GetPlayer(playerId);
            return player.ItemsList.Find(x => x.Id == itemId);
        }

        public Item[] GetAllItems(string playerId)
        {
            Data.Player player = GetPlayer(playerId);
            var itemit = new List<Item>();
            itemit = player.ItemsList;
            return itemit.ToArray();
        }

        public Player[] MinScore(int score)
        {
            var count = _collection.Count(new BsonDocument());
            var list = _collection.Find(x => x.Score > score).SortBy(d => d.Score).Limit((int)count).ToList();

            // Get top 10 players in descending order
            //var list = _collection.Find(x => true).SortByDescending(d => d.Score).Limit(10).ToList();
            return list.ToArray();
        }

        public void AddItem(Item item, string playerId)
        {
            var filter = Builders<Player>.Filter.Eq(e => e.Id, playerId);
            var update = Builders<Player>.Update.Push<Item>(e => e.ItemsList, item);
            _collection.FindOneAndUpdate(filter, update);
        }

        public void BanPlayer(string playerId, bool ban)
        {
            var filter = Builders<Player>.Filter.Eq(e => e.Id, playerId);
            var update = Builders<Player>.Update.Set(e => e.Banned, ban);
            _collection.FindOneAndUpdate(filter, update);
        }
    }
}