using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Wallhaven.Discord.Services
{
    internal class AppService : BaseService
    {
        public AppService(WallhavenClient client) : base(client)
        {
        }
        public override Task Initialize()
        {
            return base.Initialize();
        }
    }
}
