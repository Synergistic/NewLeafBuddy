using NewLeaf.Services.Models.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewLeaf.Services;

namespace NewLeaf.Tests
{
    [TestClass]
    public class Tests
    {

        [TestMethod]
        public async Task MigrateToNewPartitionKey()
        {
            var oldTable = AuthTable("ItemsTemp");

            var allExistingItems = await oldTable.ExecuteQuerySegmentedAsync(new TableQuery<ItemEntity>(), null);

            var newTable = AuthTable("Items");

            foreach (var entity in allExistingItems.ToList())
            {
                entity.PartitionKey = entity.Name.StripWhitespace();
                entity.RowKey = entity.Name.StripWhitespace();
                TableOperation operation = TableOperation.InsertOrMerge(entity);
                await newTable.ExecuteAsync(operation);
            }

        }

        private CloudTable AuthTable(string tableName)
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
    }
}