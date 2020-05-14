using NewLeaf.Services.Models.Entities;
using System.Threading.Tasks;

namespace NewLeaf.Services.Interface
{
    public interface IHistoricalTurnipService
    {
        Task<TurnipEntity> SaveTurnipData(TownEntity town);
    }
}
