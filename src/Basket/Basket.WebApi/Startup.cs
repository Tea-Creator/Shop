using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Basket.Application.Repositories;
using Basket.Infrastructure.Data;

using EventBus;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using StackExchange.Redis;

namespace Basket.WebApi
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

            services.AddSingleton((sp) =>
            {
                return ConnectionMultiplexer
                    .Connect(ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true));
            });

            services.AddTransient<RedisConnection>();
            services.AddTransient<IBasketRepository, RedisBasketRepository>();

            services.Configure<RabbitMqConnectionOptions>(Configuration.GetSection("Rabbit"));

            services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionManager>();
            services.AddSingleton<RabbitMqConnection>();
            services.AddSingleton<IEventBus, RabbitMqEventBus>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
