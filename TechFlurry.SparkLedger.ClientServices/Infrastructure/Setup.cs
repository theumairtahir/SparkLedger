using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using TechFlurry.SparkLedger.ClientServices.Core;
using TG.Blazor.IndexedDB;

namespace TechFlurry.SparkLedger.ClientServices.Infrastructure
{
    public static class Setup
    {
        public static void UseClientServices(this IServiceCollection services)
        {
            //set up local db
            services.AddIndexedDB(dbStore =>
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
            //set up client side services
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ILedgerItemService, LedgerItemService>();
            services.AddScoped<ILedgerAccountsService, LedgerAccountsService>();
        }
    }
}
