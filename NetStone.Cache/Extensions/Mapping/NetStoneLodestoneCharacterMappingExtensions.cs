using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.Enums;
using NetStone.Common.Helpers;
using NetStone.Model.Parseables.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class NetStoneLodestoneCharacterMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(NetStoneLodestoneCharacterMappingExtensions));

    public static Character ToDb(this LodestoneCharacter source, string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        return new Character
        {
            LodestoneId = lodestoneId,
            ActiveClassJob = source.GetActiveClassJob(),
            ActiveClassJobLevel = (short)source.ActiveClassJobLevel,
            ActiveClassJobIcon = source.ActiveClassJobIcon,

            Avatar = source.Avatar?.ToString() ??
                     throw new InvalidOperationException($"{nameof(source.Avatar)} must not be null"),
            Bio = source.Bio,

            FreeCompany = source.FreeCompany?.ToDb(),

            GrandCompany = source.GetGrandCompany(),
            GrandCompanyRank = source.GrandCompanyRank,

            GuardianDeityName = source.GuardianDeityName,
            GuardianDeityIcon = source.GuardianDeityIcon?.ToString() ??
                                throw new InvalidOperationException(
                                    $"{nameof(source.GuardianDeityIcon)} must not be null"),

            Name = source.Name,
            Nameday = source.Nameday,

            Portrait = source.Portrait?.ToString() ??
                       throw new InvalidOperationException($"{nameof(source.Portrait)} must not be null"),

            PvpTeam = source.PvPTeam?.Name,

            Race = EnumHelper.ParseFromDisplayString<Race>(source.Race),
            Tribe = EnumHelper.ParseFromDisplayString<Tribe>(source.Tribe),
            Gender = GetGender(source.Gender),

            Server = source.Server,

            Title = source.Title,

            TownName = source.TownName,
            TownIcon = source.TownIcon?.ToString(),

            Attributes = source.Attributes.ToDb()
        };
    }

    public static void ToDb(this LodestoneCharacter source, Character target)
    {
        target.ActiveClassJob = source.GetActiveClassJob();
        target.ActiveClassJobLevel = (short)source.ActiveClassJobLevel;
        target.ActiveClassJobIcon = source.ActiveClassJobIcon;

        target.Avatar = source.Avatar?.ToString() ??
                        throw new InvalidOperationException($"{nameof(source.Avatar)} must not be null");
        target.Bio = source.Bio;

        if (source.FreeCompany is not null)
        {
            if (target.FreeCompany is not null)
            {
                source.FreeCompany.ToDb(target.FreeCompany);
            }
            else
            {
                target.FreeCompany = source.FreeCompany.ToDb();
            }
        }
        else
        {
            target.FreeCompany = null;
        }

        target.GrandCompany = source.GetGrandCompany();
        target.GrandCompanyRank = source.GrandCompanyRank;

        target.GuardianDeityName = source.GuardianDeityName;
        target.GuardianDeityIcon = source.GuardianDeityIcon?.ToString() ??
                                   throw new InvalidOperationException(
                                       $"{nameof(source.GuardianDeityIcon)} must not be null");

        target.Name = source.Name;
        target.Nameday = source.Nameday;

        target.Portrait = source.Portrait?.ToString() ??
                          throw new InvalidOperationException($"{nameof(source.Portrait)} must not be null");

        target.PvpTeam = source.PvPTeam?.Name;

        target.Race = EnumHelper.ParseFromDisplayString<Race>(source.Race);
        target.Tribe = EnumHelper.ParseFromDisplayString<Tribe>(source.Tribe);
        target.Gender = GetGender(source.Gender);

        target.Server = source.Server;

        target.Title = source.Title;

        target.Attributes = source.Attributes.ToDb(target.Attributes.Id);
    }

    private static GrandCompany GetGrandCompany(this LodestoneCharacter character)
    {
        return character.GrandCompanyName.ToLowerInvariant() switch
        {
            "maelstrom" => GrandCompany.Maelstrom,
            "adder" => GrandCompany.OrderOfTheTwinAdder,
            "flames" => GrandCompany.ImmortalFlames,
            _ => GrandCompany.NoAffiliation
        };
    }

    private static Gender GetGender(char genderChar)
    {
        return genderChar switch
        {
            LodestoneCharacter.MaleChar => Gender.Male,
            LodestoneCharacter.FemaleChar => Gender.Female,
            _ => throw new ArgumentOutOfRangeException(nameof(genderChar), genderChar, "Gender not recognized.")
        };
    }
}