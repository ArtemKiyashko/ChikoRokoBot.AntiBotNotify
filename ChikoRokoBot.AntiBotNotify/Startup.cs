using Azure.Identity;
using Azure.Storage.Blobs;
using ChikoRokoBot.AntiBotNotify.Decorators;
using ChikoRokoBot.AntiBotNotify.Interfaces;
using ChikoRokoBot.AntiBotNotify.Managers;
using ChikoRokoBot.AntiBotNotify.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

[assembly: FunctionsStartup(typeof(ChikoRokoBot.AntiBotNotify.Startup))]
namespace ChikoRokoBot.AntiBotNotify
{
	public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;
        private AntiBotNotifyOptions _antiBotMonitorOptions = new();

        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<AntiBotNotifyOptions>(_functionConfig.GetSection(nameof(AntiBotNotifyOptions)));
            _functionConfig.GetSection(nameof(AntiBotNotifyOptions)).Bind(_antiBotMonitorOptions);

            builder.Services.AddScoped<ITelegramBotClient>(factory => new TelegramBotClient(_antiBotMonitorOptions.TelegramBotToken));

            builder.Services.AddAzureClients(clientBuilder => {
                clientBuilder.UseCredential(new DefaultAzureCredential());

                clientBuilder.AddBlobServiceClient(_antiBotMonitorOptions.StorageAccount);
            });

            builder.Services.AddScoped<BlobContainerClient>((factory) =>
            {
                var service = factory.GetRequiredService<BlobServiceClient>();
                var client = service.GetBlobContainerClient(_antiBotMonitorOptions.AntiBotPicturesContainer);
                return client;
            });

            builder.Services.AddScoped<AntiBotPictureManager>();
            builder.Services.AddScoped<IAntiBotPictureManager>((factory) =>
            {
                var decoratee = factory.GetRequiredService<AntiBotPictureManager>();
                var memoryCache = factory.GetRequiredService<IMemoryCache>();
                return new AntiBotPictureManagerCacheDecorator(memoryCache, decoratee);
            });
            builder.Services.AddMemoryCache();
        }
    }
}

