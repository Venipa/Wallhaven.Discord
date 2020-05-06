using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wallhaven.API;
using Wallhaven.Discord.Core;
using Wallhaven.Discord.Services;
using Wallhaven.Discord.Extensions;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Dynamic;
using System.Drawing;

namespace Wallhaven.Discord.Commands
{
    [Name("Wallhaven"), RequireBotPermission(ChannelPermission.EmbedLinks | ChannelPermission.SendMessages), RequireContext(ContextType.Guild)]
    public class WallhavenModule : ModuleBase<WallhavenCommandContext>
    {
        WallhavenAPI wh;
        public WallhavenModule(ConfigService configService)
        {
            this.wh = new WallhavenAPI(configService.Get<string>("wallhaven.token"));
        }
        [Command("search", RunMode = RunMode.Async), Alias("s"), Remarks("search <args>")]
        public async Task Query([Remainder]string q)
        {
            var r = await this.wh.search(q, WallhavenCategory.Anime | WallhavenCategory.People | WallhavenCategory.General,
                (this.Context.Channel as SocketTextChannel).IsNsfw ? WallhavenPurity.SFW | WallhavenPurity.SKETCHY | WallhavenPurity.NSFW : WallhavenPurity.SFW,
                WallhavenSort.Relevance,
                WallhavenSortOrder.Descending);
            if (r?.Data == null || r.Data.Count == 0)
            {
                await this.ReplyAsync($"0 Wallpapers found for \"{q}\"");
                return;
            }
            var image = await this.wh.getById(r.Data[0].Id);
            var emb = new EmbedBuilder()
                .WithThumbnailUrl(image.Uploader.Avatar.FirstOrDefault(x => x.Key == "200px").Value?.ToString() ?? image.Uploader.Avatar.FirstOrDefault().Value.ToString())
                .AddField("Dimension", image.Resolution)
                .AddField("View Image", image.ShortUrl)
                .WithFooter("Created At: " + image.CreatedAt.ToString())
                .WithImageUrl(image.Path.ToString());
            if (!new string[] { "deleted", "banned" }.Contains(image.Uploader.Group?.ToLower()))
            {
                emb = emb
                    .WithAuthor(image.Uploader.Username, null, $"https://wallhaven.cc/user/{image.Uploader.Username}");
            }
            await this.Context.Channel.SendMessageAsync("", false, emb.Build());
            return;
        }
        [Command("wallpaper", RunMode = RunMode.Async), Alias("w"), Remarks("wallpaper <id>")]
        public async Task WallpaperById(string id)
        {
            var image = await this.wh.getById(id);
            if (image == null)
            {
                await this.ReplyAsync($"Wallpaper \"{id}\" not found");
                return;
            }
            if (image.Purity == WallhavenPurity.NSFW || image.Purity == WallhavenPurity.SKETCHY)
            {
                await this.ReplyAsync("This wallpaper contains nsfw/sketchy flags and may only be displayed in an NSFW channel.");
                return;
            }
            var emb = new EmbedBuilder()
                .WithThumbnailUrl(image.Uploader.Avatar.FirstOrDefault(x => x.Key == "200px").Value?.ToString() ?? image.Uploader.Avatar.FirstOrDefault().Value.ToString())
                .AddField("Dimension", image.Resolution)
                .AddField("View Image", image.ShortUrl)
                .WithFooter("Created At: " + image.CreatedAt.ToString())
                .WithImageUrl(image.Path.ToString());
            if (!new string[] { "deleted", "banned" }.Contains(image.Uploader.Group?.ToLower()))
            {
                emb = emb
                    .WithAuthor(image.Uploader.Username, null, $"https://wallhaven.cc/user/{image.Uploader.Username}");
            }
            await this.Context.Channel.SendMessageAsync("", false, emb.Build());
            return;
        }
        [Command("random", RunMode = RunMode.Async), Alias("r"), Remarks("random <(a)nime|(p)eople|(g)eneral>")]
        public async Task Random(WallhavenCategory category = WallhavenCategory.Anime | WallhavenCategory.People | WallhavenCategory.General)
        {
            var r = await this.wh.search(null, category,
                (this.Context.Channel as SocketTextChannel).IsNsfw ? WallhavenPurity.SFW | WallhavenPurity.SKETCHY | WallhavenPurity.NSFW : WallhavenPurity.SFW,
                WallhavenSort.Random,
                WallhavenSortOrder.Descending);
            if (r?.Data == null || r.Data.Count == 0)
            {
                await this.ReplyAsync($"Something went wrong");
                return;
            }
            var image = await this.wh.getById(r.Data[0].Id);
            var emb = new EmbedBuilder()
                .WithThumbnailUrl(image.Uploader.Avatar.FirstOrDefault(x => x.Key == "200px").Value?.ToString() ?? image.Uploader.Avatar.FirstOrDefault().Value.ToString())
                .AddField("Dimension", image.Resolution)
                .AddField("View Image", image.ShortUrl)
                .WithFooter("Created At: " + image.CreatedAt.ToString())
                .WithImageUrl(image.Path.ToString());
            if (!new string[] { "deleted", "banned" }.Contains(image.Uploader.Group?.ToLower()))
            {
                emb = emb
                    .WithAuthor(image.Uploader.Username, null, $"https://wallhaven.cc/user/{image.Uploader.Username}");
            }
            await this.Context.Channel.SendMessageAsync("", false, emb.Build());
            return;
        }
    }
}
