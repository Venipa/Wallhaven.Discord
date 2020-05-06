using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Wallhaven.Discord.Config;

namespace Wallhaven.Discord.Services
{
    public class ConfigService : BaseService
    {
        public IConfiguration Config { get; private set; }
        public ConfigService(WallhavenClient client) : base(client)
        {
        }
        public T Get<T>(string key, T defaultValue = default(T))
        {
            var v = this.Config[key.Replace(".", ":")];
            return v != null ? (T)Convert.ChangeType(v, typeof(T)) : defaultValue;
        }
        public void Set<T>(string key, T value)
        {
            var v = this.Config[key.Replace(".", ":")] = Convert.ToString(value);
        }
        public override Task Initialize()
        {
            this.Config = new ConfigurationBuilder()
                .SetBasePath(WallhavenAssembly.GetPath())
                .SetFileLoadExceptionHandler((ex) =>
                {
                    Logger.Warning($"Failed to load config file {ex.Provider.Source.Path}");
                    Logger.CriticalError(ex.Exception);
                })
                .AddInMemoryCollection(new Dictionary<string, string>())
                .AddIniFile(WallhavenAssembly.Name + ".ini", false)
                .Build();
            return base.Initialize();
        }
    }
}
