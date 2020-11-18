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
                dbStore.Version = 7;

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "Tokens",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{Name="value", KeyPath = "value", Auto=false}
                    }
                });

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "LedgerItems",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true, Unique=true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{Name="name", KeyPath = "name", Auto=false},
                        new IndexSpec{Name="category", KeyPath = "category", Auto=false}
                    }
                });
                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "LedgerAccounts",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true, Unique = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{Name="code", KeyPath = "code", Auto=false},
                        new IndexSpec{Name="title", KeyPath = "title", Auto=false},
                        new IndexSpec{Name="category", KeyPath = "category", Auto=false},
                        new IndexSpec{Name="phone", KeyPath = "phone", Auto=false},
                        new IndexSpec{Name="description", KeyPath = "description", Auto=false}
                    }
                });
                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "LedgerActivites",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true, Unique = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{Name="accountId", KeyPath = "accountId", Auto=false},
                        new IndexSpec{Name="itemId", KeyPath = "itemId", Auto=false},
                        new IndexSpec{Name="transactionType", KeyPath = "transactionType", Auto=false},
                        new IndexSpec{Name="value", KeyPath = "value", Auto=false},
                        new IndexSpec{Name="code", KeyPath = "code", Auto=false},
                        new IndexSpec{Name="activityTime", KeyPath = "activityTime", Auto=false}
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
