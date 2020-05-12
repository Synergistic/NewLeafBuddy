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
        private const string TableName = "Items";
        private CloudTable AuthTable()
        {
            string accountName = "acnlapistorage";
            string accountKey = "Zzk/IAagFg98xoJtAEFNVPo7Al9sejrtPemuPPqlEmC24Kr+REJgsP8PLXRv2UHFVTOmnPysAuORCngBOSDg8w==";
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);

                CloudTableClient client = account.CreateCloudTableClient();

                CloudTable table = client.GetTableReference(TableName);

                return table;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddOrUpdate(AnimalCrossingItemEntity newEntity)
        {
            var table = AuthTable();
            TableOperation operation = TableOperation.InsertOrMerge(newEntity);
            await table.ExecuteAsync(operation);
        }


        public async Task<AnimalCrossingItemEntity> GetItemByName(string itemName)
        {
            itemName = itemName.ToLowerInvariant();
            var table = AuthTable();
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

            var table = AuthTable();
            var entities = await table.ExecuteQuerySegmentedAsync(new TableQuery<AnimalCrossingItemEntity>(), null);
            return entities.ToList();
        }

        public async Task DeleteItem(string itemName)
        {

            var table = AuthTable();

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
