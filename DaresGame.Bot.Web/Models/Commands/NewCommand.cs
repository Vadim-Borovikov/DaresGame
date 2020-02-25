﻿using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace DaresGame.Bot.Web.Models.Commands
{
    internal class NewCommand : Command
    {
        internal override string Name => "new";
        internal override string Description => Caption.ToLowerInvariant();

        internal const string Caption = "Новая игра";

        private readonly GameLogic _gameLogic;

        public NewCommand(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
        }

        internal override bool Contains(Message message)
        {
            return (message.Type == MessageType.Text)
                && (message.Text.Contains(Name) || message.Text.Contains(Caption));
        }

        internal override Task ExecuteAsync(Message message, ITelegramBotClient client)
        {
            return _gameLogic.StartNewGameAsync(message.Chat);
        }
    }
}