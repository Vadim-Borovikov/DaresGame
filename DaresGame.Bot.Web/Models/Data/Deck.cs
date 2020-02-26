// ReSharper disable MemberCanBeInternal
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
using System.Collections.Generic;

namespace DaresGame.Bot.Web.Models.Data
{
    public class Deck
    {
        public int Id { get; set; }
        public uint Order { get; set; }
        public string Tag { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }
}