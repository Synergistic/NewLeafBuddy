using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewLeaf.Services.Models.Entities
{
    public class TurnipEntity : TableEntity
    {
        public string TurnipPrices { get; set; }
        public int TurnipsOwned { get; set; }
        public int MaxProfit { get; set; }
        public DateTime StartDate { get; set; }
    }
}
