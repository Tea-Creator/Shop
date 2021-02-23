using EventBus;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Ordering.Application.IntegrationEvents.EventHandlers;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Repositories;
using Ordering.Application.Services;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Repositories;

using RabbitMQ.Client;

using RabbitMQEventBus;

namespace Ordering.WebApi
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

            services.AddDbContext<OrderContext>(opts =>
            {
                opts.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            services.AddScoped<IOrderRepository, EfOrderRepository>();

            services.AddScoped<OrderService>();

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
                    "Ordering");
            });

            services.AddScoped<BasketCheckoutEventHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<BasketCheckoutEvent, BasketCheckoutEventHandler>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.WebApi v1"));
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
