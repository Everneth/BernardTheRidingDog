using BernardTheRidingDog.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace BernardTheRidingDog.Services
{
    public class SortingDogService
    {
        private readonly DiscordSocketClient _client;
        private List<ulong> _roles = new List<ulong> { 1254922764796694550, 1256721186700660818, 1256721448664301700 };
        private Dictionary<ulong, int> roleCount = new Dictionary<ulong, int>();
        private Dictionary<ulong, string> roleTotals = new Dictionary<ulong, string>();
        private const int GAP_THRESHOLD = 3;
        public SortingDogService(IServiceProvider services) 
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
        }

        public async Task<ActionResponse> RequestSortingAsync(SocketUser ctxUser, SocketGuild ctxGuild)
        {
            roleCount.Clear();
            var user = ctxGuild.GetUser(ctxUser.Id);
            //get count of roles and role name
            foreach (ulong role in _roles)
            {
                roleCount.Add(role, ctxGuild.GetRole(role).Members.Count());
            }
            //sort dict by role count
            var roleCountSorted = roleCount.OrderBy(x => x.Value).ToDictionary(x => x.Key, y => y.Value);
            //determine which houses we can use by checking gap threshold - default: 3
            roleCountSorted = ValidateHousesToUse(roleCountSorted);
            if (HasHouse(user))
                return new ActionResponse($"You have been sorted already.", false);
            else
            {
                int roleIndex = Random.Shared.Next(roleCountSorted.Count);
                await user.AddRoleAsync(roleCountSorted.ElementAt(roleIndex).Key);
                return new ActionResponse($"You have been sorted into {ctxGuild.GetRole(roleCountSorted.ElementAt(roleIndex).Key).Mention}!", true);
            }
        }

        private bool HasHouse(SocketGuildUser user)
        {
            return user.Roles.Any(x => _roles.Contains(x.Id));
        }

        private Dictionary<ulong, int> ValidateHousesToUse(Dictionary<ulong, int> roles)
        {
            bool housesValidated = false;
            int iteration = 0;
            while (!housesValidated)
            {
                int lowestRoleCount = 0;
                int highestRoleCount = 0;
                lowestRoleCount = roles.FirstOrDefault().Value;
                highestRoleCount = roles.LastOrDefault().Value;
                if(highestRoleCount - lowestRoleCount > GAP_THRESHOLD)
                {
                    roles.Remove(roles.LastOrDefault().Key);
                }
                ++iteration;
                if (iteration == roles.Count)
                    break;
            }
            return roles;
        }

        public ActionResponse RoleTotals(SocketUser ctxUser, SocketGuild ctxGuild)
        {
            roleCount.Clear();
            roleTotals.Clear();
            roleTotals.Add(1254922764796694550, "<:house_lion:1271575130283315265>");
            roleTotals.Add(1256721448664301700, "<:house_wolf:1271575148352503888>");
            roleTotals.Add(1256721186700660818, "<:house_bear:1271575068719190107>");
            var user = ctxGuild.GetUser(ctxUser.Id);
            //get count of roles and role name
            foreach (var key in roleTotals)
            {
                roleCount.Add(key.Key, ctxGuild.GetRole(key.Key).Members.Count());
            }
            StringBuilder sb = new StringBuilder();
            foreach(var role in roleTotals)
            {
                sb.Append($"{role.Value} {ctxGuild.GetRole(role.Key).Members.Count()}\n");
            }
            return new ActionResponse(sb.ToString(), true);
        }
    }
}
