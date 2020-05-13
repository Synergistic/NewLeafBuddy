using Microsoft.WindowsAzure.Storage.Table;
using NewLeaf.Services.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewLeaf.Services.Interface
{
    public interface IStorageService
    {
        Task AddOrUpdate(string tableName, ITableEntity newEntity);
        Task<T> GetByName<T>(string tableName, string itemName, string partitionKey);
        Task<List<ItemEntity>> GetAllItems();
        Task<List<TownEntity>> GetAllTowns();
        Task DeleteByName(string tableName, string itemName, string partitionKey);
    }
}
