using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using GameWebApi.MaintenanceProcessing;
using GameWebApi.Models;

namespace GameWebApi.Controllers
{
    [System.Web.Http.RoutePrefix("api/players/{playerId}/items")]

    public class ItemsController : ApiController
    {
        private readonly IItemsMaintenanceProcessor _maintenanceProcessor;

        public ItemsController(IItemsMaintenanceProcessor maintenanceProcessor)
        {
            _maintenanceProcessor = maintenanceProcessor;
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public Item Create(string playerId, NewItem newItem)
        {
            // We get the claimer's ID
            var claimerId = ((ClaimsPrincipal)HttpContext.Current.User).FindFirst(ClaimTypes.NameIdentifier).Value;

            // We compare the claimer's ID and the item's owner ID
            if (claimerId == playerId)
            {
                return _maintenanceProcessor.Create(playerId, newItem);
            }

            // IDs did not match
            throw new HttpException((int)HttpStatusCode.Unauthorized, "Claimer's ID does not match item's owner ID");
        }

        /// <summary>
        /// Returns all the items associated with the player
        /// </summary>
        [HttpGet]
        [Route("")]
        public Item[] GetItems(string playerId)
        {
            return _maintenanceProcessor.GetAll(playerId);
        }
    }
}