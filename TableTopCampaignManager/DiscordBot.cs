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
        private async Task InitializeClient()
        {
            _client = new DiscordSocketClient();
            _client.Ready += InitializeCommands;
            _client.Log += LogUserMessage;
            var token = Environment.GetEnvironmentVariable("token");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
        private Task LogUserMessage(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private async Task InitializeCommands()
        {
            ulong guildId = Convert.ToUInt64(Environment.GetEnvironmentVariable("guildid"));

            var rollCommand = new SlashCommandBuilder()
                .WithName("roll")
                .WithDescription("Rolls a DnD die (with a value between 4 and 20) one or more times.")
                .AddOption("dice", ApplicationCommandOptionType.Number, "To roll a 17 die 3 times, input '3d17'", isRequired: true);

            try
            {
                await _client.Rest.CreateGuildCommand(rollCommand.Build(), guildId);
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception, Formatting.Indented);
                Console.WriteLine(json);
            }
        }
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "roll":
                    await HandleRollCommand(command);
                    break;
            }
        }
        private static int[] CalculateRoll(string userInput)
        {
            Random random = new Random();
            int numberOfRolls = Int32.Parse(userInput.Split('d')[0]);
            int dieValue = Int32.Parse(userInput.Split('d')[1]);
            int[] calculatedValues = new int[numberOfRolls];
            for (int i = 0; i < numberOfRolls; i++)
            {
                calculatedValues[i] = random.Next(1, dieValue+1);
            }
            return calculatedValues;
        }
    }
}