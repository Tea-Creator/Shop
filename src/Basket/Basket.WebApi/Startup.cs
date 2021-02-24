using Basket.Application.IntegrationEvents.EventHandlers;
using Basket.Application.IntegrationEvents.Events;
using Basket.Application.Repositories;
using Basket.Application.Services;
using Basket.Infrastructure.Data;

using EventBus;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using RabbitMQ.Client;

using RabbitMQEventBus;

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
            services.AddScoped<BasketService>();

            services.AddSingleton<ISubscriptionManager, InMemoryEventBusSubscriptionManager>();

            services.AddSingleton(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = Configuration["EventBus:Host"],
                    UserName = Configuration["EventBus:UserName"],
                    Password = Configuration["EventBus:Password"],
                    Port = 5672,
                    DispatchConsumersAsync = true
                };

                return new RmqConnection(factory, sp.GetRequiredService<ILogger<RmqConnection>>());
            });

            services.AddSingleton<IEventBus, RmqEventBus>(sp =>
            {
                return new RmqEventBus(
                    sp.GetRequiredService<ISubscriptionManager>(),
                    sp.GetRequiredService<IServiceScopeFactory>(),
                    sp.GetRequiredService<RmqConnection>(),
                    sp.GetRequiredService<ILogger<RmqEventBus>>(),
                    "Basket");
            });

            services.AddScoped<CheckoutSucceedEventHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<CheckoutSucceedEvent, CheckoutSucceedEventHandler>();

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
