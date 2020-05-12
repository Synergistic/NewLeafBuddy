using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewLeaf.Services.Implementation
{
    public class ItemsService : IItemsService
    {
        public ItemsService(IStorageService storageService)
        {
            this.StorageService = storageService;
        }

        protected IStorageService StorageService { get; }
        public async Task AddPriceForItem(string itemName, int price)
        {
            if (await this.ItemExists(itemName))
            {
                return;
            }

            await StorageService.AddOrUpdate(
                "Items",
            new ItemEntity()
            {
                Price = price,
                Name = itemName,
                Id = 0,
                RowKey = itemName,
                PartitionKey = "AnimalCrossItemPrices"
            });
        }

        public async Task<List<ItemEntity>> GetAllItems()
        {
            return await StorageService.GetAllItems();
        }

        public async Task RemoveItemByName(string itemName)
        {
            await StorageService.DeleteByName("Items", itemName, "AnimalCrossingItemPrices");
        }

        private async Task<bool> ItemExists(string itemName)
        {
            var itemEntity = await StorageService.GetByName<ItemEntity>("Items", itemName, "AnimalCrossingItemPrices");
            return itemEntity != null;
        }


    }
}
