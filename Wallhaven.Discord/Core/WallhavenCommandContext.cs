using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wallhaven.Discord.Core
{
    public class WallhavenCommandContext : SocketCommandContext
    {
        public new WallhavenClient Client { get; protected set; }
        public WallhavenCommandContext(WallhavenClient client, SocketUserMessage msg) : base(client, msg)
        {
            this.Client = client;
        }
    }
    public class WallhavenModuleBase : ModuleBase<WallhavenCommandContext>
    {
        public WallhavenModuleBase()
        {
        }
    }
}
