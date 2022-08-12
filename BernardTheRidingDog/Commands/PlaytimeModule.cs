using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernardTheRidingDog.Commands
{
    public class PlaytimeModule : InteractionModuleBase<SocketInteractionContext>
    {
        public PlaytimeModule() { }

        [SlashCommand("pet", "Pet the dog will ya.")]
        public async Task PetTheDog()
        {
            await RespondAsync("Heck off mate");
        }

        [SlashCommand("fetch", "Play with the dog... he likes that.")]
        public async Task PlayFetch()
        {
            Random rng = new Random();
            List<string> toys = new List<string>() { "bone", "ball", "grenade" };
            string toy = toys[rng.Next(1, 3)];
            EmbedBuilder eb = new EmbedBuilder();

            eb.Title = $"{Context.User.Username}#{Context.User.Discriminator} threw a {toy}";
            eb.Description = "Test";
            await RespondAsync(embed: eb.Build());
        }
    }
}
