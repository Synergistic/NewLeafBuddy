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
                TurnipPrices = "0.0.0.0.0.0.0.0.0.0.0.0.0.0",
                TurnipsOwned = 0,
                PartitionKey = userName.ToLowerInvariant(),
                RowKey = townName.ToLowerInvariant()
            };
            await StorageService.AddOrUpdate("Towns", newTown);
            return newTown;
        }

        public async Task<TownEntity> UpdateTurnips(string userName, string townName, string turnipPrices, int quantity)
        {
            var existingTown = await this.TownExists(userName, townName);
            if (existingTown == null) return null;
            existingTown.TurnipPrices = turnipPrices;
            existingTown.TurnipsOwned = quantity;
            await StorageService.AddOrUpdate("Towns", existingTown);
            return existingTown;
        }
        public async Task<List<TownEntity>> GetMyTowns(string userName)
        {
            return await StorageService.GetMyTowns(userName);
        }
        private async Task<TownEntity> TownExists(string userName, string townName)
        {
            return await StorageService.GetTownByName("Towns", townName, userName);
        }
    }
}
