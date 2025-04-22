using System;
using System.IO;
using Core.Common;
using Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DineGO_Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            // Cấu hình ApiSettings
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<ApiSettings>>().Value);

            services.AddHttpClient<ApiService>();

            // Session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();

            // MVC
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();

            // Middleware bắt lỗi toàn cục
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (UnauthorizedAccessException)
                {
                    context.Response.Redirect("/Auth/Login");
                }
                catch (Exception ex)
                {
                    context.Session.SetString("ErrorMessage", ex.Message);
                    context.Response.Redirect("/Home/Error");
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
