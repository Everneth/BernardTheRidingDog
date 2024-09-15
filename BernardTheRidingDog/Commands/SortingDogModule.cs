using BernardTheRidingDog.Services;
using Discord;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BernardTheRidingDog.Commands
{
    [Group("sorting", "Rather pay for a sorter, His Majesty has instead trained a highly skilled St. Bernard.")]
    public class SortingDogModule : InteractionModuleBase<SocketInteractionContext>
    {
        private IServiceProvider _services;
        private SortingDogService _sortingDog;
        public SortingDogModule(IServiceProvider services) 
        {
            _services = services;
            _sortingDog = services.GetRequiredService<SortingDogService>();
        }

        [SlashCommand("request", "Request Bernard to drag you to your new, but random, Royal House.")]
        public async Task SortingRequest()
        {
            var user = Context.Guild.GetUser(Context.User.Id);
            if (user.Roles.Where(x => x.Name == "Citizen").Any())
            {
                var result = await _sortingDog.RequestSortingAsync(this.Context.User, this.Context.Guild);
                await RespondAsync(text: $"{result.Message}", ephemeral: true);
            }
            else
                await RespondAsync(text: $"You must be a citizen to be sorted! Bad dog!", ephemeral: true);
        }
        [SlashCommand("totals", "Request role totals.")]
        public async Task RoleTotals()
        {
            var result = _sortingDog.RoleTotals(this.Context.User, this.Context.Guild);
            await RespondAsync(text: $"{result.Message}", ephemeral: true);
        }
    }
}
