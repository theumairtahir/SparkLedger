using Microsoft.Extensions.DependencyInjection;
using TechFlurry.SparkLedger.ClientServices.Core;

namespace TechFlurry.SparkLedger.ClientServices.Infrastructure
{
    public static class Setup
    {
        public static void UseClientServices(this IServiceCollection services)
        {
            //set up client side services
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ILedgerItemService, LedgerItemService>();
            services.AddScoped<ILedgerAccountsService, LedgerAccountsService>();
        }
    }
}
