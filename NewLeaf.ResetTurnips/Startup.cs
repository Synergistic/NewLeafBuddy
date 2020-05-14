using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NewLeaf.Services.Implementation;
using NewLeaf.Services.Interface;

[assembly: FunctionsStartup(typeof(NewLeaf.ResetTurnips.Startup))]

namespace NewLeaf.ResetTurnips
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITownService, TownService>();
            builder.Services.AddSingleton<IStorageService, StorageService>();
            builder.Services.AddSingleton<IHistoricalTurnipService, HistoricalTurnipService>();

        }
    }
}
