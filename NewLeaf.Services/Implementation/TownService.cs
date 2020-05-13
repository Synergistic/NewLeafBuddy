using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System;
using System.Collections.Generic;
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

        public async Task<TownEntity> CreateNewTown(string userName, string townName, string mayorName, DateTime createdDate, int nativeFruit)
        {
            var existingTown = await this.TownExists(userName, townName);
            if (existingTown != null) return existingTown;
            var newTown = new TownEntity()
            {
                Name = townName,
                MayorName = mayorName,
                OwnerUsername = userName,
                Created = createdDate,
                NativeFruit = nativeFruit,
                TurnipPrices = new List<Tuple<int, int>>(),
                PartitionKey = userName,
                RowKey = townName
            };
            await StorageService.AddOrUpdate("Towns", newTown);
            return newTown;
        }
        public async Task<List<TownEntity>> GetMyTowns(string userName)
        {
            return await StorageService.GetMyTowns(userName);
        }
        private async Task<TownEntity> TownExists(string userName, string townName)
        {
            return await StorageService.GetByName<TownEntity>("Towns", townName, userName);
        }
    }
}
