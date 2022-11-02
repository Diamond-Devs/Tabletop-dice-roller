using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Discordbot
{
    class DieRoller
    {
        public static void Main(string[] args)
        => new DieRoller().InitializeClient().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        public async Task InitializeClient()
        {
            _client = new DiscordSocketClient();
            _client.Ready += Initialize_Commands;
            _client.Log += LogUserMessage;
            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
        private Task LogUserMessage(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        public async Task Initialize_Commands()
        {
            ulong guildId = Convert.ToUInt64(File.ReadAllText("guildid.txt"));

            var guildCommand = new SlashCommandBuilder()
                .WithName("roll")
                .WithDescription("Rolls a DnD die (with a value between 4 and 20) one or more times.")
                .AddOption("dice", ApplicationCommandOptionType.Number, "To roll a 17 die 3 times, input '3d17'", isRequired: true);

            try
            {
                await _client.Rest.CreateGuildCommand(guildCommand.Build(), guildId);
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
    }
}