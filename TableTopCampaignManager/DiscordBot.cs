using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
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
            _client.SlashCommandExecuted += SlashCommandHandler;
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
                .WithDescription("Rolls a DnD die (with a value between 4 and 20) between 1 to 10 times.")
                .AddOption("dice", ApplicationCommandOptionType.String, "To roll a 17 die 3 times, input '3d17'", isRequired: true);

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
        private static int[] CalculateRoll(string userInput, SocketSlashCommand command)
        {
            Random random = new Random();
            int maxRolls = 10;
            int numberOfRolls = Int32.Parse(userInput.Split('d')[0]);
            int dieValue = Int32.Parse(userInput.Split('d')[1]);
            int[] calculatedValues = new int[numberOfRolls];
            if (numberOfRolls > maxRolls)
            {
                command.RespondAsync(text: $"That's {numberOfRolls} rolls! 10 is the maximum.");
                return null;
            }
            else
            {
                for (int i = 0; i < numberOfRolls; i++)
                {
                    calculatedValues[i] = random.Next(1, dieValue + 1);
                }
                return calculatedValues;
            }
        }
        private async Task HandleRollCommand(SocketSlashCommand command)
        {
            var discordUserInputString = CalculateRoll(command.Data.Options.First().Value.ToString(),command);
            var discordUserName = command.User.Username;
            if (discordUserInputString != null)
            {
                string rollValues = $"{discordUserName}'s roll values are: ";
                int sumValues = 0;
                for (int i = 0; i < discordUserInputString.Length; i++)
                {
                    rollValues = rollValues + discordUserInputString[i].ToString() + ", ";
                    sumValues += discordUserInputString[i];
                }
                await command.RespondAsync(text: rollValues.Remove(rollValues.Length-2) + System.Environment.NewLine + $"Result: {sumValues}.");
            }
        }
    }
}