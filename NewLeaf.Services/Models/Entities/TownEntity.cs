using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace NewLeaf.Services.Models.Entities
{
    public class TownEntity : TableEntity
    {
        public string OwnerUsername { get; set; }
        public string Name { get; set; }
        public string MayorName { get; set; }
        public DateTime Created { get; set; }
        public int NativeFruit { get; set; }
        public List<Tuple<int, int>> TurnipPrices { get; set; }
    }
}
