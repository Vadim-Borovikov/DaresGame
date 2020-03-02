using System.Collections.Generic;
using DaresGame.Logic;

namespace DaresGame.Bot.Web.Models
{
    public class Settings
    {
        internal readonly int InitialPlayersAmount;
        internal readonly float InitialChoiceChance;
        internal readonly IReadOnlyCollection<Deck> Decks;

        internal Settings(int initialPlayersAmount, float initialChoiceChance, IReadOnlyCollection<Deck> decks)
        {
            InitialPlayersAmount = initialPlayersAmount;
            InitialChoiceChance = initialChoiceChance;
            Decks = decks;
        }
    }
}