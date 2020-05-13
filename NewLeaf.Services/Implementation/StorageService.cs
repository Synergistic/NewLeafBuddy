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
        private CloudTable AuthTable(string tableName = "Items")
        {
            string accountName = "acnlapistorage";
            string accountKey = "Zzk/IAagFg98xoJtAEFNVPo7Al9sejrtPemuPPqlEmC24Kr+REJgsP8PLXRv2UHFVTOmnPysAuORCngBOSDg8w==";
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

        public async Task AddOrUpdate(string tableName, ITableEntity newEntity)
        {
            var table = AuthTable(tableName);
            TableOperation operation = TableOperation.InsertOrMerge(newEntity);
            await table.ExecuteAsync(operation);
        }

        public async Task<T> GetByName<T>(string tableName, string itemName, string partitionKey)
        {
            itemName = itemName.ToLowerInvariant();
            var table = AuthTable(tableName);
            TableOperation entity = TableOperation.Retrieve<ITableEntity>(partitionKey, itemName);
            var result = await table.ExecuteAsync(entity);
            if(result?.Result != null)
            {
                return ((T)result.Result);
            }
            return default(T);
        }

        public async Task<List<ItemEntity>> GetAllItems()
        {
            var table = AuthTable("Items");
            var entities = await table.ExecuteQuerySegmentedAsync(new TableQuery<ItemEntity>(), null);
            return entities.ToList();
        }


        public async Task<List<TownEntity>> GetMyTowns(string userName)
        {
            var table = AuthTable("Towns");
            var tableQuery = new TableQuery<TownEntity>().Where(TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.GreaterThanOrEqual,
                userName
              ));
            var entities = await table.ExecuteQuerySegmentedAsync(tableQuery, null);
            return entities.ToList();
        }

        public async Task DeleteByName(string tableName, string itemName, string partitionKey)
        {

            var table = AuthTable(tableName);
            var entity = new ItemEntity
            {
                PartitionKey = partitionKey,
                RowKey = itemName,
                ETag = "*"
            };

            TableOperation deleteOperation = TableOperation.Delete(entity);
            await table.ExecuteAsync(deleteOperation);
        }

    }
}
