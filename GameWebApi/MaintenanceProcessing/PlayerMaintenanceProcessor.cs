using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using GameWebApi.Common.Security;
using GameWebApi.Data;
using GameWebApi.Models;
using Player = GameWebApi.Models.Player;

namespace GameWebApi.MaintenanceProcessing
{
    public class PlayerMaintenanceProcessor : IPlayerMaintenanceProcessor
    {
        private readonly IRepository _repository;
        private readonly IBasicSecurityService _securityService;

        public PlayerMaintenanceProcessor(IRepository repository, IBasicSecurityService securityService)
        {
            _repository = repository;
            _securityService = securityService;
        }

        public Player[] GetAll()
        {
            Data.Player[] playerDatas = _repository.GetAllPlayers();

            var playerModels = new List<Player>();
            foreach (var playerData in playerDatas)
            {
                Player playerModel = CreatePlayerModelFromPlayerData(playerData);
                playerModels.Add(playerModel);
            }
            return playerModels.ToArray();
        }

        public Player Get(string id)
        {
            Data.Player playerData = _repository.GetPlayer(id);
            Player playerModel = CreatePlayerModelFromPlayerData(playerData);
            return playerModel;
        }

        public Player Create(NewPlayer newPlayer)
        {
            var playerData = CreatePlayerDataFromNewPlayer(newPlayer);

            playerData = _repository.CreatePlayer(playerData);

            return CreatePlayerModelFromPlayerData(playerData);
        }

        private Data.Player CreatePlayerDataFromNewPlayer(NewPlayer newPlayer)
        {
            var player = new Data.Player()
            {
                Name = newPlayer.Name,
                Score = newPlayer.Score,
                RolesList = newPlayer.Roles.ToList()
            };

            // Use the created Data.Player with this function in _securityService and spit out a new object that is the Data.Player that has the salt and hash added into its variables.
            player = _securityService.AddSaltAndPasswordHash(player, newPlayer.PassWord);

            return player;
        }

        private static Player CreatePlayerModelFromPlayerData(Data.Player player)
        {
            var playerModel = new Player()
            {
                Id = player.Id,
                Name = player.Name,
                Score = player.Score,

            };
            return playerModel;
        }
        public Player[] MinScore(int score)
        {
            var playerList = _repository.MinScore(score);
            var playerModeList = new Player[] {};
            for (var x = 0; x < playerList.Count(); x++)
            {
                playerModeList[x] = CreatePlayerModelFromPlayerData(playerList[x]);
            }
            return playerModeList;
        }

        public void AddItem(Data.Item item, string playerId)
        {
            _repository.AddItem(item, playerId);
        }

        public void BanPlayer(string playerId, bool ban)
        {
            _repository.BanPlayer(playerId, ban);
        }
    }
}