using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using GameWebApi.Data;
using GameWebApi.Models;
using Item = GameWebApi.Models.Item;

namespace GameWebApi.MaintenanceProcessing
{
    public class ItemsMaintenanceProcessor : IItemsMaintenanceProcessor
    {
        private readonly IRepository _repository;

        public ItemsMaintenanceProcessor(IRepository repository)
        {
            _repository = repository;
        }

        
        public Item Create(string playerId, NewItem newItem)
        {

                //Create new Data.Item from NewItem object
                var item = new Data.Item()
                {
                    //Server creates the id
                    Id = Guid.NewGuid().ToString(),
                    Price = newItem.Price
                };

                //Delegate creation forward to query processor
                item = _repository.CreateItem(playerId, item);

                //Convert from Data.Item to Models.Item
                var itemModel = CreateItemModelFromItemData(item);
                return itemModel;
        }

        public Item[] GetAll(string playerId)
        {
            var itemDatas = _repository.GetAllItems(playerId);
            var itemModels = new List<Item>();

            //Go through all the Data.Item objects and convert them to Models.Item objects
            foreach (var item in itemDatas)
            {
                var itemModel = CreateItemModelFromItemData(item);
                itemModels.Add(itemModel);
            }
            return itemModels.ToArray();
        }

        /// <summary>
        /// This function converts from Data.Item to Models.Item so that it can be
        /// returned to the client
        /// </summary>
        private static Item CreateItemModelFromItemData(Data.Item item)
        {
            var itemModel = new Item()
            {
                Id = item.Id,
                Price = item.Price
            };
            return itemModel;
        }
    }
}