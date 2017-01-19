using System.Security.Claims;
using System.Web;
using System.Web.Http;
using GameWebApi.Models;

namespace GameWebApi.MaintenanceProcessing
{
    public interface IItemsMaintenanceProcessor
    {
        Item Create(string playerId, NewItem item);
        Item[] GetAll(string playerId);
    }
}