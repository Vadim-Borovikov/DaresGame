// ReSharper disable MemberCanBeInternal
// ReSharper disable UnusedMember.Global
namespace DaresGame.Bot.Web.Models.Data
{
    public class Card
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Players { get; set; }
        public int PartnersToAssign { get; set; }
    }
}