using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System.Collections.Generic;
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
        public async Task<ItemEntity> AddPriceForItem(string itemName, int price)
        {
            var strippedItemName = itemName.StripWhitespace();
            var existingItem = await this.ItemExists(strippedItemName);
            if(existingItem != null)
            {
                return existingItem;
            }
            var newItem = new ItemEntity()
            {
                Price = price,
                Name = itemName,
                Id = 0,
                RowKey = strippedItemName,
                PartitionKey = strippedItemName
            };
            await StorageService.AddOrUpdate("Items", newItem);
            return newItem;
        }

        public async Task<List<ItemEntity>> GetAllItems()
        {
            return await StorageService.GetAllItems();
        }

        public async Task RemoveItemByName(string itemName)
        {
            var strippedItemName = itemName.StripWhitespace();
            await StorageService.DeleteByName("Items", strippedItemName, strippedItemName);
        }

        private async Task<ItemEntity?> ItemExists(string itemName)
        {
            return await StorageService.GetByName<ItemEntity>("Items", itemName, itemName);            
        }
    }
}
