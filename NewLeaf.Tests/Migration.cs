using NewLeaf.Services.Models.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewLeaf.Tests
{
    [TestClass]
    public class Tests
    {


        [TestMethod]
        public async Task MigrateToNewAzureAsync()
        {
            var oldTable = AuthTable("AnimalCrossingItemPrices", true);

            var allExistingItems = await oldTable.ExecuteQuerySegmentedAsync(new TableQuery<AnimalCrossingItemEntity>(), null);

            var newTable = AuthTable("Items", false);

            foreach (var entity in allExistingItems.ToList())
            {
                TableOperation operation = TableOperation.InsertOrMerge(entity);
                await newTable.ExecuteAsync(operation);
            }

        }

        private CloudTable AuthTable(string tableName, bool useOld)
        {
            string accountName = "btcnotistorage";
            string accountKey = "7cSbmcyVEcdYPqaw5kGc24AGQxiNNqDxwE8cgacA89GDoHYJ2p0K1Ql9/EqzRoXLLiile4ajngF+tXX6LIrFAw==";
            if (!useOld)
            {
                accountName = "acnlapistorage";
                accountKey = "Zzk/IAagFg98xoJtAEFNVPo7Al9sejrtPemuPPqlEmC24Kr+REJgsP8PLXRv2UHFVTOmnPysAuORCngBOSDg8w==";
            }
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