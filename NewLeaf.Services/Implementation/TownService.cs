using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewLeaf.Services.Implementation
{
    public class TownService : ITownService
    {
        public TownService(IStorageService storageService)
        {
            this.StorageService = storageService;
        }

        protected IStorageService StorageService { get; }
        
        public async Task CreateNewTown(string userName, string townName, string mayorName, DateTime createdDate, int nativeFruit)
        {
            if (await this.TownExists(userName, townName))
            {
                return;
            }
            await StorageService.AddOrUpdate(
                "Towns",
            new TownEntity()
            {
                Name = townName,
                MayorName = mayorName,
                OwnerUsername = userName,
                Created = createdDate,
                NativeFruit = nativeFruit,
                TurnipPrices = new List<Tuple<int, int>>()
            });
        }
        //public async Task AddPriceForItem(string itemName, int price)
        //{
        //    if (await this.ItemExists(itemName))
        //    {
        //        return;
        //    }

        //    await StorageService.AddOrUpdate(
        //        "Items",
        //    new ItemEntity()
        //    {
        //        Price = price,
        //        Name = itemName,
        //        Id = 0,
        //        RowKey = itemName,
        //        PartitionKey = "AnimalCrossItemPrices"
        //    });
        //}

        public async Task<List<TownEntity>> GetAllItems()
        {
            return await StorageService.GetAllTowns();
        }

        //public async Task RemoveItemByName(string itemName)
        //{
        //    await StorageService.DeleteByName("Items", itemName, "AnimalCrossingItemPrices");
        //}

        private async Task<bool> TownExists(string userName, string townName)
        {
            var itemEntity = await StorageService.GetByName<TownEntity>("Towns", townName, userName);
            return itemEntity != null;
        }
    }
}
