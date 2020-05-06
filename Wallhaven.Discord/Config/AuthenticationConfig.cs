using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wallhaven.Discord.Config
{
    public class AuthenticationConfig
    {
        public string Token { get; set; }
        public TokenType Type { get; set; }
    }
}
