using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallhaven.Discord.Services;
using Wallhaven.Discord.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using Discord.Commands.Builders;
using Discord.Commands;
using System.Reflection;
using Wallhaven.Discord.Core;

namespace Wallhaven.Discord
{
    public class WallhavenClient : DiscordSocketClient
    {
        public IServiceContainer ServiceContainer;
        public string DefaultPrefix { get; private set; }
        public CommandHandler commandHandler { get; set; }
        public WallhavenClient()
        {
            Logger.Info("Initializing");
            this.Log += WallhavenClient_Log;
            this.ServiceContainer = new ServiceContainer();
            List<Task> serviceLoaded = new List<Task>();
            foreach(var service in WallhavenAssembly.GetClassesBasedOn<BaseService>().Select(x => Activator.CreateInstance(x, new object[] { this }) as BaseService))
            {
                this.ServiceContainer.AddService(service.GetType(), service);
                serviceLoaded.Add(service.Initialize());
            }
            if (serviceLoaded.Count > 0)
            {
                Task.WaitAll(serviceLoaded.ToArray());
            } else
            {
                Logger.Info("No Services loaded");
            }
            var cmdService = new CommandService(new CommandServiceConfig()
            {
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });
            this.DefaultPrefix = this.ServiceContainer.GetService<ConfigService>()?.Get<string>("bot.prefix") ?? "!";
            this.commandHandler = new CommandHandler(this, cmdService);
            commandHandler.InstallCommandsAsync(this.ServiceContainer).Wait();
            foreach (var cmd in cmdService.Commands.ToArray())
            {
                Logger.Info($"Loaded Command: {cmd.Name}");
            }
            Logger.Info($"Loaded {cmdService.Commands.Count()} Commands");
            foreach (var tr in cmdService.TypeReaders.ToArray())
            {
                Logger.Info($"Loaded Type Reader: {tr.Key.Name}");
            }
            Logger.Info($"Loaded {cmdService.TypeReaders.Count()} Type Readers");

            this.ServiceContainer.AddService(typeof(CommandService), cmdService);
        }

        private Task WallhavenClient_Log(LogMessage arg)
        {
            switch(arg.Severity)
            {
                case LogSeverity.Info:
                    return Task.Run(() => Logger.Info(arg.Message));
                case LogSeverity.Warning:
                    return Task.Run(() => Logger.Warning(arg.Message));
                case LogSeverity.Error:
                    return Task.Run(() => Logger.CriticalError(arg.Exception));
                default:
                    return Task.Run(() => Logger.Debug(arg.Message));
            }
        }

        public Task Login()
        {
            var type = this.ServiceContainer.GetService<ConfigService>().Get<string>("authentication.type");
            if (Enum.TryParse(typeof(TokenType), type, true, out var tokenType) && tokenType == null)
            {
                throw new InvalidEnumArgumentException("Invalid Token Type");
            }
            return this.LoginAsync((TokenType)tokenType, this.ServiceContainer.GetService<ConfigService>().Get<string>("authentication.token"));
        }
    }
}
