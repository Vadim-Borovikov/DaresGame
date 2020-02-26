using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DaresGame.Bot.Web.Models.Commands;
using DaresGame.Bot.Web.Models.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Card = DaresGame.Logic.Card;
using Deck = DaresGame.Logic.Deck;

namespace DaresGame.Bot.Web.Models.Services
{
    internal class BotService : IBotService, IHostedService
    {
        public TelegramBotClient Client { get; }
        public IReadOnlyList<Command> Commands { get; }
        public GameLogic GameLogic { get; }

        private readonly BotConfiguration _config;

        public BotService(IOptions<BotConfiguration> options, IServiceScopeFactory scopeFactory)
        {
            _config = options.Value;

            Client = new TelegramBotClient(_config.Token);

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DaresGameDbContext>();
                List<Deck> decks = db.Decks.Select(CreateDeck).ToList();
                GameLogic = new GameLogic(Client, _config.InitialPlayersAmount, _config.ChoiceChance, decks);
            }

            var commands = new List<Command>
            {
                new NewCommand(GameLogic),
                new DrawCommand(GameLogic)
            };

            Commands = commands.AsReadOnly();
            var startCommand = new StartCommand(Commands, _config.Host, GameLogic);

            commands.Insert(0, startCommand);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Client.SetWebhookAsync(_config.Url, cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Client.DeleteWebhookAsync(cancellationToken);

        private static Deck CreateDeck(Data.Deck data) => new Deck(data.Tag, data.Cards.Select(CreateCard));
        private static Card CreateCard(Data.Card data)
        {
            return new Card(data.Description, data.Players, data.PartnersToAssign);
        }
    }
}