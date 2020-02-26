// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBeInternal
// ReSharper disable UnusedAutoPropertyAccessor.Global
using Microsoft.EntityFrameworkCore;

namespace DaresGame.Bot.Web.Models.Data
{
    public class DaresGameDbContext : DbContext
    {
        public DaresGameDbContext(DbContextOptions<DaresGameDbContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
    }
}