using Knab.Cryptocurrency.Domain.Interfaces;
using Knab.Cryptocurrency.Domain.Settings;
using Knab.Cryptocurrency.Infrastructure.Clients;
using Knab.Cryptocurrency.Infrastructure.Services;
using Knab.Cryptocurrency.Presentation.Middlewares;
using Serilog;
using System.Reflection;

namespace Knab.Cryptocurrency.Presentation.Extensions
{
    internal static class ApplicationExtensions
    {
        private const string ApplicationAssemblyName = "Knab.Cryptocurrency.Application";

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load(ApplicationAssemblyName)));
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            services.AddSwaggerGen();

            services.AddHttpClient<CryptoCurrencyClient>()
                .AddTypedClient((httpClient, provider) =>
                {
                    var apiSettings = configuration.GetSection("AppSettings:CryptoCurrencyApi").Get<ApiSettings>();
                    return new CryptoCurrencyClient(httpClient, apiSettings);
                });

            services.AddHttpClient<FiatCurrencyClient>()
                .AddTypedClient((httpClient, provider) =>
                {
                    var apiSettings = configuration.GetSection("AppSettings:FiatCurrencyApi").Get<ApiSettings>();
                    return new FiatCurrencyClient(httpClient, apiSettings);
                });

            services.AddScoped<ICryptoCurrencyService, CryptoCurrencyService>();
            services.AddScoped<IFiatCurrencyService, FiatCurrencyService>();
        }

        public static void ConfigurePipeline(this WebApplication app)
        {
            app.UseDeveloperExceptionPage();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.MapControllers();
        }

        public static void RegisterLogger(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());
        }
    }
}
