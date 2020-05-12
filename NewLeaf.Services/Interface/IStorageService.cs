using NewLeaf.Services.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewLeaf.Services.Interface
{
    public interface IStorageService
    {
        Task AddOrUpdate(AnimalCrossingItemEntity newEntity);
        Task<AnimalCrossingItemEntity> GetItemByName(string itemName);
        Task<List<AnimalCrossingItemEntity>> GetAllItems();
        Task DeleteItem(string itemName);
    }
}
