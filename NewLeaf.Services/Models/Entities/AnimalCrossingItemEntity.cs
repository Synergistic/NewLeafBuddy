using Microsoft.WindowsAzure.Storage.Table;

namespace NewLeaf.Services.Models.Entities
{
    public class AnimalCrossingItemEntity: TableEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int BonusPrice { get; set; }
        public int ExoticPrice { get; set; }
        public int Id { get; set; }
    }
}
