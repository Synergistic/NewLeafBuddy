using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NewLeaf.Services.Implementation
{
    public class HistoricalTurnipService : IHistoricalTurnipService
    {
        IStorageService StorageService;
        public HistoricalTurnipService(IStorageService storageService)
        {
            this.StorageService = storageService;
        }

        public async Task<TurnipEntity> SaveTurnipData(TownEntity town)
        {
            if (town.TurnipPrices == null || town.TurnipPrices.Length <= 0 || town.TurnipPrices.All(s => s < 20)) return null;
            var turnipRecord = new TurnipEntity()
            {
                MaxProfit = this.CalculateMaxProfit(town.TurnipPrices, town.TurnipsOwned),
                StartDate = DateTime.Now.AddDays(-7),
                TurnipPrices = town.TurnipPrices,
                TurnipsOwned = town.TurnipsOwned,
                PartitionKey = town.OwnerUsername,
                RowKey = $"{DateTime.Now.AddDays(-7).Month}.{DateTime.Now.AddDays(-7).Day}.{DateTime.Now.AddDays(-7).Year}.{town.Name.ToLowerInvariant()}"
            };
            await this.StorageService.AddOrUpdate("Turnips", turnipRecord);
            return turnipRecord;
        }

        private int CalculateMaxProfit(string priceString, int quantityOwned)
        {
            var prices = priceString.Split('.').Select(s => int.Parse(s)).ToList();
            if (quantityOwned == 0) return 0;
            var buyPrice = prices[0];
            if (buyPrice == 0) return 0;
            var totalPurchasePrice = buyPrice * quantityOwned;
            var maxProfit = 0;
            for(int i = 2; i <prices.Count(); i++)
            {
                var totalSalePrice = prices[i] * quantityOwned;
                if(totalSalePrice - totalPurchasePrice > maxProfit)
                {
                    maxProfit = totalSalePrice - totalPurchasePrice;
                }
            }
            return maxProfit;
        }
    }
}
