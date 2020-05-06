using Discord;
using Discord.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wallhaven.Discord.Services
{
    public class ActivityService : BaseService
    {
        public string DefaultActivity { get; set; }
        public ActivityService(WallhavenClient client) : base(client)
        {
        }
        public override Task Initialize()
        {
            this._client.SetActivityAsync(new DiscordActivity() { Details = $"{this._client.Guilds.Count} guilds", Flags = ActivityProperties.Instance, Type = ActivityType.Listening, Name = "wallhaven.cc" });
            return base.Initialize();
        }
    }
    public class DiscordActivity : IActivity
    {
        public string Name { get; set; }

        public ActivityType Type { get; set; }

        public ActivityProperties Flags { get; set; }

        public string Details { get; set; }
    }
}
