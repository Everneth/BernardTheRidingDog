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

        [SlashCommand("bark", "Heckin woof")]
        public async Task BarkTheDog()
        {
            await RespondAsync("No");
        }

        [SlashCommand("fetch", "Play with the dog... he likes that.")]
        public async Task PlayFetch()
        {
            Random rng = new Random();
            List<string> toys = new List<string>() { "bone", "ball", "grenade" };
            string toy = toys[rng.Next(3)];
            EmbedBuilder eb = new EmbedBuilder();

            eb.Title = $"{Context.User.Username} threw a {toy}";
            eb.Description = "He'll remember that...";
            await RespondAsync(embed: eb.Build());
        }
    }
}
