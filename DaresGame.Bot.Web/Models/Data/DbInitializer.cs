using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DaresGame.Bot.Web.Models.Data
{
    internal static class DbInitializer
    {
        public static void Initialize(DaresGameDbContext db)
        {
            db.Database.EnsureCreated();

            // Look for any decks.
            if (db.Decks.Any())
            {
                return; // DB has been seeded
            }

            foreach (string path in Directory.EnumerateFiles(Path))
            {
                InitializeDeck(path, db);
            }
        }

        private static void InitializeDeck(string path, DaresGameDbContext db)
        {
            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 2)
            {
                throw new Exception("Incorrect deck");
            }

            string tag = lines[0];

            var cards = new List<Card>();
            for (int i = 1; i < lines.Length; ++i)
            {
                string line = lines[i];
                Card card = InitializeCard(line, db);
                cards.Add(card);
            }
            db.SaveChanges();

            var deck = new Deck
            {
                Tag = tag,
                Cards = cards
            };
            db.Decks.Add(deck);
            db.SaveChanges();
        }

        private static Card InitializeCard(string line, DaresGameDbContext db)
        {
            string[] chunks = line.Split(';');

            if (chunks.Length != 3)
            {
                throw new Exception($"Incorrect card: {line}");
            }

            if (!int.TryParse(chunks[0], out int players))
            {
                throw new Exception($"Incorrect card: {line}");
            }

            if (!int.TryParse(chunks[1], out int partnersToAssign))
            {
                throw new Exception($"Incorrect card: {line}");
            }

            var card = new Card
            {
                Description = chunks[2],
                Players = players,
                PartnersToAssign = partnersToAssign
            };
            db.Cards.Add(card);
            return card;
        }

        private const string Path = "C:/Code/Decks";
    }
}