using System;
using System.Globalization;
using System.IO;
using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.Security.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DashboardMainDemo {
    public class Startup {
        readonly string dataDirectoryPath;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment) {
            dataDirectoryPath = Path.Combine(hostingEnvironment.ContentRootPath, "App_Data");
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectoryPath);

            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            AccessSettings.StaticResources.TrySetRules(new DirectoryAccessRule(AccessPermission.Allow, new string[] { hostingEnvironment.WebRootPath }));
            DashboardExportSettings.CompatibilityMode = DashboardExportCompatibilityMode.Restricted;
        }

        public void ConfigureServices(IServiceCollection services) {
            services
                .AddResponseCompression()
                .AddMvc()
                .AddDefaultDashboardController((configurator, serviceProvider) => {
                    configurator.SetDashboardStorage(serviceProvider.GetService<SessionDashboardStorage>());
                    //configurator.SetDashboardStorage(new DashboardFileStorage(Path.Combine(dataDirectoryPath, "Dashboards"))); // Uncomment this line to use the App_Data/Dashboards folder to store dashboards.

                    DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
                    dataSourceStorage.RegisterDataSource(DataSourceGenerator.CreateNWindDataSource().SaveToXml());
                    configurator.SetDataSourceStorage(dataSourceStorage);
                    configurator.SetConnectionStringsProvider(serviceProvider.GetService<SessionConnectionStringsStorage>());

                    configurator.DataLoading += (s, e) => {
                        DataLoader.LoadData(e);
                    };
                });

            services.AddDevExpressControls();

            services
                .AddDistributedMemoryCache()
                .AddSession();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<SessionDashboardStorage>();
            services.AddTransient<SessionConnectionStringsStorage>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseSession();
            app.UseDevExpressControls();
            app.UseMvc(routes => {
                routes.MapDashboardRoute();
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
