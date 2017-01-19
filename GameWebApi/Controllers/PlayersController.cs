using System;
using System.Diagnostics;
using System.Web.Http;
using System.Web.Security;
using GameWebApi.MaintenanceProcessing;
using GameWebApi.Models;
using GameWebApi.Web.Common;
using log4net;

namespace GameWebApi.Controllers
{
    [RoutePrefix("api/players")]
    public class PlayersController : ApiController
    {
        private readonly ILog Log = LogManager.GetLogger("PlayersController");
        private readonly IPlayerMaintenanceProcessor _maintenanceProcessor;

        public PlayersController(IPlayerMaintenanceProcessor maintenanceProcessor)
        {
            _maintenanceProcessor = maintenanceProcessor;
        }

        [HttpGet]
        [Route("")]
        public Player[] GetAll()
        {
            return _maintenanceProcessor.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public Player GetById(string id)
        {
            Trace.WriteLine("Getting player with id " +id);
            Log.Info("Getting player with id " +id);
            return _maintenanceProcessor.Get(id);
        }

        [HttpPost]
        [Route("")]
        public Player CreatePlayer(NewPlayer newPlayer)
        {
            return _maintenanceProcessor.Create(newPlayer);
        }

        [HttpPut]
        [Route("{id}")]
        public Player ModifyPlayer(string id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("minScore={score}")]
        public Player[] GetPlayersWithMinScore(int score)
        {
            return _maintenanceProcessor.MinScore(score);
        }

        // Only admins can (un)ban players
        [HttpPut]
        [Route("{playerId}")]
        [Authorize(Roles ="admin")]
        public void BanPlayer(string playerId, [FromUri] bool banned)
        {
            _maintenanceProcessor.BanPlayer(playerId, banned);
        }
    }
}