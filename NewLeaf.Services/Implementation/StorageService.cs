using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewLeaf.Services.Implementation
{
    public class StorageService :IStorageService
    {
        private CloudTable AuthTable(string tableName)
        {
            string accountName = "btcnotistorage";
            string accountKey = "7cSbmcyVEcdYPqaw5kGc24AGQxiNNqDxwE8cgacA89GDoHYJ2p0K1Ql9/EqzRoXLLiile4ajngF+tXX6LIrFAw==";
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudTableClient client = account.CreateCloudTableClient();

                CloudTable table = client.GetTableReference(tableName);

                return table;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddOrUpdate(AnimalCrossingItemEntity newEntity)
        {
            var table = AuthTable("AnimalCrossingItemPrices");
            TableOperation operation = TableOperation.InsertOrMerge(newEntity);
            await table.ExecuteAsync(operation);
        }


        public async Task<AnimalCrossingItemEntity> GetItemByName(string itemName)
        {
            itemName = itemName.ToLowerInvariant();
            var table = AuthTable("AnimalCrossingItemPrices");
            TableOperation entity = TableOperation.Retrieve<AnimalCrossingItemEntity>("AnimalCrossingItemPrices", itemName);
            var result = await table.ExecuteAsync(entity);
            if(result?.Result != null)
            {
                return ((AnimalCrossingItemEntity)result.Result);
            }
            return null;
        }

        public async Task<List<AnimalCrossingItemEntity>> GetAllItems()
        {

            var table = AuthTable("AnimalCrossingItemPrices");
            var entities = await table.ExecuteQuerySegmentedAsync(new TableQuery<AnimalCrossingItemEntity>(), null);
            return entities.ToList();
        }

        public async Task DeleteItem(string itemName)
        {

            var table = AuthTable("AnimalCrossingItemPrices");

            var entity = new AnimalCrossingItemEntity
            {
                PartitionKey = "AnimalCrossItemPrices",
                RowKey = itemName,
                ETag = "*"
            };

            TableOperation deleteOperation = TableOperation.Delete(entity);
            await table.ExecuteAsync(deleteOperation);
        }

    }
}
