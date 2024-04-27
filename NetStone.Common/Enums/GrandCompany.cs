using System.ComponentModel.DataAnnotations;

namespace NetStone.Common.Enums;

public enum GrandCompany
{
    [Display(Name = "")] NoAffiliation,

    [Display(Name = "The Maelstrom")] Maelstrom,

    [Display(Name = "The Order of the Twin Adder")]
    OrderOfTheTwinAdder,

    [Display(Name = "The Immortal Flames")]
    ImmortalFlames
}