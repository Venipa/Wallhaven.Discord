using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wallhaven.Discord.Services
{
    public class BaseService
    {
        protected readonly WallhavenClient _client;
        public BaseService(WallhavenClient client)
        {
            this._client = client;
        }
        public string Name { get; protected set; }
        public string getName() => this.Name ?? this.GetType()?.Name;
        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }
    }
}
