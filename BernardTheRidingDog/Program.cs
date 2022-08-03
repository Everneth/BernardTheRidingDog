using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using BernardTheRidingDog.Models;
using BernardTheRidingDog.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BernardTheRidingDog
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private ConfigService _config;
        private DataService _data;
        private BotConfig _token;
        private DatabaseService _database;
        private HttpClientService _restClient;

        public static void Main()
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using var services = ConfigureServices();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<ConfigService>();
            _data = services.GetRequiredService<DataService>();
            _database = services.GetRequiredService<DatabaseService>();
            _restClient = services.GetRequiredService<HttpClientService>();

            _client.Log += Log;
            _client.Ready += OnReady;
            await _client.SetGameAsync("the door", type: ActivityType.Watching);

            await _client.LoginAsync(TokenType.Bot, _config.BotConfig.Token);
            await _client.StartAsync();

            await services.GetRequiredService<CommandHandlerService>().InstallCommandsAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            // cache all members of the Everneth discord
            await _client.GetGuild(_config.BotConfig.GuildId).DownloadUsersAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(
                    new DiscordSocketConfig
                    {
                        AlwaysDownloadUsers = true,
                        MessageCacheSize = 3000,
                        GatewayIntents = GatewayIntents.All
                    }))
                .AddSingleton<InteractionService>()
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<ConfigService>()
                .AddSingleton<DataService>()
                .AddSingleton<DatabaseService>()
                .AddSingleton<HttpClientService>()
                .BuildServiceProvider();
        }
    }
}
