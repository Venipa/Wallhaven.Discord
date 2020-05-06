using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Wallhaven.Discord.Core
{
    public class CommandHandler
    {
        private readonly WallhavenClient _client;
        private readonly CommandService _commands;

        public CommandHandler(WallhavenClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync(IServiceProvider service = null)
        {
            _client.MessageReceived += HandleCommandAsync;

            await this._commands.AddModulesAsync(Assembly.GetExecutingAssembly(), service);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasStringPrefix(_client.DefaultPrefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new WallhavenCommandContext(_client, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: this._client.ServiceContainer);
        }
    }
}
