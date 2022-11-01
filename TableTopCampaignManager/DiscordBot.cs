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
            _client.Ready += Client_Ready;
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
        public async Task Client_Ready()
        {
            ulong guildId = Convert.ToUInt64(File.ReadAllText("guildid.txt"));

            var guildCommand = new SlashCommandBuilder()
                .WithName("roll-die")
                .WithDescription("Rolls a DnD die with values between 4 and 20.")
                .AddOption("die-value", ApplicationCommandOptionType.Number, "The number of sides on the die", isRequired: true);

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