using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NewLeaf.Services.Implementation;
using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;

namespace NewLeaf.ResetTurnips
{
    public class SundayTurnipReset
    {

        private readonly ITownService TownService;
        private readonly IHistoricalTurnipService HistoricalTurnipService;


        public SundayTurnipReset(ITownService townService, IHistoricalTurnipService historicalTurnipService)
        {
            this.TownService = townService;
            this.HistoricalTurnipService = historicalTurnipService;
        }


        [FunctionName("SundayTurnipReset")]
        public async Task Run([TimerTrigger("0 5 0 * * SUN")]TimerInfo myTimer, ILogger log)
        {
            var allTowns = await TownService.GetAllTowns();
            foreach(var town in allTowns)
            {
                await HistoricalTurnipService.SaveTurnipData(town);
                var newTown = new TownEntity()
                {
                    PartitionKey = town.PartitionKey,
                    RowKey = town.RowKey,
                     MayorName = town.MayorName,
                     OwnerUsername = town.OwnerUsername,
                     Created = town.Created,
                     ETag = town.ETag,
                     Name = town.Name,
                     NativeFruit = town.NativeFruit,
                     Timestamp = town.Timestamp,
                    TurnipPrices = "0.0.0.0.0.0.0.0.0.0.0.0.0.0",
                    TurnipsOwned = 0
                };
                await TownService.UpsertTown(newTown);
            }
        }
    }
}
