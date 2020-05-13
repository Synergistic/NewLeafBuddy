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
        public async Task AddPriceForItem(string itemName, int price)
        {
            var strippedItemName = itemName.StripWhitespace();
            if (await this.ItemExists(strippedItemName))
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
                RowKey = strippedItemName,
                PartitionKey = strippedItemName
            });
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

        private async Task<bool> ItemExists(string itemName)
        {
            var itemEntity = await StorageService.GetByName<ItemEntity>("Items", itemName, itemName);
            return itemEntity != null;
        }


    }
}
