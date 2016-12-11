using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AnalyseVisitorsTool.Data;
using AnalyseVisitorsTool.Models;
using AnalyseVisitorsTool.Services;
using AnalyseVisitorsTool.Repositories;
using AnalyseVisitorsTool.Abstract;
using Hangfire;
using Hangfire.PostgreSql;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace AnalyseVisitorsTool
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("psqlConnection")))
                .AddDbContext<LogDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("psqlConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var sb = new NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("psqlConnection"))
            {
                Pooling = false 
            };
            var storage = new PostgreSqlStorage(sb.ConnectionString);
            services.AddHangfire(x => x.UseStorage(storage));
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IServerLogService, ServerLogService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddScoped<IServerLogRepository, ServerLogRepository>();
            services.AddScoped<IIPLocationRepository, IPLocationRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, 
        IServerLogService serverlogservice, IHttpContextAccessor httpcontext, SignInManager<ApplicationUser> signinmanager)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();
            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            app.UseHangfireDashboard("/jobs", new DashboardOptions {
                Authorization = new[] { new HangfireAuthorizationFilter(signinmanager, httpcontext) }
            });
            app.UseHangfireServer(new BackgroundJobServerOptions(),null, new PostgreSqlStorage(Configuration.GetConnectionString("psqlConnection")));
            //RecurringJob.AddOrUpdate(() => serverlogservice.BuildServerLogDatabaseEntries(), Cron.Daily);
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
