namespace TaxiDispatcher.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TaxiDispatcher.Infrastructure;
    using TaxiDispatcher.WebApi.Extensions;
    using Serilog;
    using MassTransit;
    using Messages;
    using RabbitMQ.Client;
    using TaxiDispatcher.WebApi.Features;

    public class StartupDevelopment
    {
        public IConfiguration _config { get; }
        public IWebHostEnvironment _env { get; }

        public StartupDevelopment(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsService("TaxiDispatcherCorsPolicy");
            services.AddInfrastructure(_config, _env);

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("small-vehicles", x =>
                    {
                        x.ConfigureConsumeTopology = false;

                        x.Consumer<DispatchRideByType>();

                        x.Bind("ridetype", s =>
                        {
                            s.RoutingKey = "SMALL";
                            s.ExchangeType = ExchangeType.Direct;
                        });
                        //x.Bind<IRideTypeRequested>();
                    });

                    cfg.ReceiveEndpoint("priority-orders", x =>
                    {
                        x.ConfigureConsumeTopology = false;

                        x.Consumer<OrderConsumer>();

                        x.Bind("submitorder", s =>
                        {
                            s.RoutingKey = "PRIORITY";
                            s.ExchangeType = ExchangeType.Direct;
                        });
                    });
                });
            });
            services.AddMassTransitHostedService();

            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddWebApiServices();
            services.AddHealthChecks();

            // Dynamic Services
            services.AddSwaggerExtension(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseCors("TaxiDispatcherCorsPolicy");

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });

            // Dynamic App
            app.UseSwaggerExtension(_config);
        }
    }
}