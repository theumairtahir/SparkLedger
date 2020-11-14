using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechFlurry.SparkLedger.ClientServices.Infrastructure;
using TG.Blazor.IndexedDB;
//using Blazor.IndexedDB.Framework;

namespace TechFlurry.SparkLedger.PWA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddScoped<IIndexedDbFactory, IndexedDbFactory>();
            builder.Services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = "SparkLedgerDb"; //example name
                dbStore.Version = 1;

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Tokens",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{Name="value", KeyPath = "value", Auto=false}

                    }
                });
            });
            builder.Services.UseClientServices();
            await builder.Build().RunAsync();
        }
    }
}
