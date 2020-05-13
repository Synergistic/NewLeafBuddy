using NewLeaf.Services.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewLeaf.Services.Interface
{
    public interface ITownService
    {
        Task<List<TownEntity>> GetMyTowns(string userName);
        Task<TownEntity> CreateNewTown(string userName, string townName, string mayorName, DateTime createdDate, int nativeFruit);
        Task<TownEntity> UpdateTurnipPrices(string userName, string townName, string turnipPrices);
    }
}
