using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BernardTheRidingDog.Services
{
    public class TamagotchiService
    {
        private DiscordSocketClient _client;
        private IServiceProvider _services;
        private Timer _timer;

        public TamagotchiService(IServiceProvider services)
        {
            _services = services;
            _client = services.GetRequiredService<DiscordSocketClient>();
            //_timer = new Timer()
        }

        public void BernardWakeUp(DiscordSocketClient client)
        {
            // https://i.imgur.com/8pgg5ww.png
            EmbedBuilder eb = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder
                {
                    IconUrl = "https://i.imgur.com/8pgg5ww.png",
                    Name = "Bernard the Riding Dog"
                },
                Title = "Bernard is now on duty.",
                Color = new Color(77, 151, 55)
            };
            client.GetGuild(177976693942779904L).GetTextChannel(514625957328584715L).SendMessageAsync(embed: eb.Build());
        }
    }
}
