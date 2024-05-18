using System.ComponentModel.DataAnnotations;

namespace NetStone.Common.Enums;

[Flags]
public enum FreeCompanyFocus
{
    None = 0,
    [Display(Name = "Role Playing")] RolePlay = 1,
    Leveling = 1 << 1,
    Casual = 1 << 2,
    Hardcore = 1 << 3,
    Dungeons = 1 << 4,
    Guildhests = 1 << 5,
    Trials = 1 << 6,
    Raids = 1 << 7,
    PvP = 1 << 8
}