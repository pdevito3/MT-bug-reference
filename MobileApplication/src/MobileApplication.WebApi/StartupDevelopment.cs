namespace MobileApplication.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MobileApplication.Infrastructure;
    using MobileApplication.Infrastructure.Seeders;
    using MobileApplication.Infrastructure.Contexts;
    using MobileApplication.WebApi.Extensions;
    using Serilog;
    using MassTransit;
    using RabbitMQ.Client;
    using Messages;

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
            services.AddCorsService("MobileApplicationCorsPolicy");
            services.AddInfrastructure(_config, _env);

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Send<IRideTypeRequested>(e =>
                    {
                        // use ride typoe for the routing key
                        e.UseRoutingKeyFormatter(context => context.Message.RideType.ToString());
                    });
                    cfg.Message<IRideTypeRequested>(e => e.SetEntityName("ridetype")); // name of exchange
                    cfg.Publish<IRideTypeRequested>(e => e.ExchangeType = ExchangeType.Direct); // exchange type

                    cfg.Send<ISubmitOrder>(x =>
                    {
                        // use customerType for the routing key
                        x.UseRoutingKeyFormatter(context => context.Message.CustomerType.ToString());
                    });
                    //Keeping in mind that the default exchange config for your published type will be the full typename of your message
                    //we explicitly specify which exchange the message will be published to. So it lines up with the exchange we are binding our
                    //consumers too.
                    cfg.Message<ISubmitOrder>(x => x.SetEntityName("submitorder"));
                    //Also if your publishing your message: because publishing a message will, by default, send it to a fanout queue.
                    //We specify that we are sending it to a direct queue instead. In order for the routingkeys to take effect.
                    cfg.Publish<ISubmitOrder>(x => x.ExchangeType = ExchangeType.Direct);
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

            // Entity Context - Do Not Delete

            using (var context = app.ApplicationServices.GetService<MobileApplicationDbContext>())
            {
                context.Database.EnsureCreated();

                // MobileApplicationDbContext Seeders
                RideRequestSeeder.SeedSampleRideRequestData(app.ApplicationServices.GetService<MobileApplicationDbContext>());
            }

            app.UseCors("MobileApplicationCorsPolicy");

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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