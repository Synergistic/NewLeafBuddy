using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewLeaf.Services.Implementation
{
    public class AnimalCrossingStorageService : IAnimalCrossingStorageService
    {
        public AnimalCrossingStorageService(IStorageService storageService)
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
            new AnimalCrossingItemEntity()
            {
                Price = price,
                Name = itemName,
                Id = 0,
                RowKey = itemName,
                PartitionKey = "AnimalCrossItemPrices"
            });
        }

        public async Task<List<AnimalCrossingItemEntity>> GetAllItems()
        {
            return await StorageService.GetAllItems();
        }

        public async Task RemoveItemByName(string itemName)
        {
            await StorageService.DeleteItem(itemName);
        }

        private async Task<bool> ItemExists(string item)
        {
            var itemEntity = await StorageService.GetItemByName(item);
            return itemEntity != null;
        }


    }
}
