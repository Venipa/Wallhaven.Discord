using System;
using System.Threading.Tasks;

namespace Wallhaven.Discord
{
    class App
    {
        static void Main(string[] args)
        {
            var client = new WallhavenClient();
            client.Login().Wait();
            client.StartAsync();
            Task.Delay(-1).Wait();
        }
    }
}
