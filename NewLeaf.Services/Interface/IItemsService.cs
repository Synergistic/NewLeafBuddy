using NewLeaf.Services.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewLeaf.Services.Interface
{
    public interface IItemsService
    {
        Task<List<ItemEntity>> GetAllItems();
        Task<ItemEntity> AddPriceForItem(string itemName, int price);
        Task RemoveItemByName(string itemName);
    }
}
