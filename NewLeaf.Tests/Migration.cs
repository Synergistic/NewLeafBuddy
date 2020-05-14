using NewLeaf.Services.Models.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewLeaf.Services;
using Microsoft.Extensions.DependencyInjection;
using NewLeaf.Services.Interface;
using NewLeaf.Services.Implementation;
using System;

namespace NewLeaf.Tests
{
    [TestClass]
    public class Tests
    {

        private ServiceProvider serviceProvider { get; set; }
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddSingleton(typeof(ITownService), typeof(TownService));
            services.AddSingleton(typeof(IStorageService), typeof(StorageService));
            serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public async Task Thing() {
            var storageService = new StorageService();
            var townService = new TownService(storageService);

            var table = AuthTable("Towns");

            await townService.UpdateTurnipPrices("synergy.harmgmail.com", "test", "1.0.0.0.0.0.0.0.0.0.0.0.0.0");

        }

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