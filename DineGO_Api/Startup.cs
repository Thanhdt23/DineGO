using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Core.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DineGO_Api.Data;
using Microsoft.EntityFrameworkCore;
using DineGO_Api.Repository;

namespace DineGO_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //ConnectDB
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DineGO_Api", Version = "v1" });
            });
            services.AddSingleton<TokenService>();
            services.AddSingleton<HashService>();
            // Added: Đăng ký Distributed Memory Cache cho session
            services.AddDistributedMemoryCache();
            // Added: Đăng ký Session
            services.AddSession(options =>
            {
                options.IdleTimeout = System.TimeSpan.FromMinutes(30); // Session timeout sau 30 phút
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            services.AddAuthorization();
            services.AddHttpContextAccessor();

            //Add dependency here
            services.AddScoped<RestaurantDAO>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<CategoryDAO>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<BlogDAO>();
            services.AddScoped<IBlogRepositoy, BlogRepository>();
            services.AddScoped<CustomerDAO>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IMailSenderRepository, MailSenderRepository>();
            services.AddScoped<RestaurantOwnerDAO>();  
            services.AddScoped<IRestaurantOwnerRepository, RestaurantOwnerRepository>();
            services.AddScoped<ReservationDAO>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IRestaurantOwnerRepository, RestaurantOwnerRepository>();
            services.AddScoped<RestaurantOwnerDAO>();  


            services.AddScoped<PaymentDAO>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DineGO_Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
