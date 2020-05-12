using NewLeaf.Services.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewLeaf.Services.Interface
{
    public interface IAnimalCrossingStorageService
    {
        Task<List<AnimalCrossingItemEntity>> GetAllItems();
        Task AddPriceForItem(string itemName, int price);
        Task RemoveItemByName(string itemName);
    }
}
