using System.Diagnostics;
using NetStone.Cache.Db.Models;
using NetStone.Common.DTOs.Character;

namespace NetStone.Cache.Extensions.Mapping;

public static class DbCharacterMappingExtensions
{
    private static readonly ActivitySource ActivitySource = new(nameof(DbCharacterMappingExtensions));

    public static CharacterDto ToDto(this Character character)
    {
        using var activity = ActivitySource.StartActivity();

        return new CharacterDto
        {
            Id = character.LodestoneId,
            Name = character.Name,
            Server = character.Server,
            Title = character.Title,
            Avatar = character.Avatar,
            Portrait = character.Portrait,
            Bio = character.Bio,
            Nameday = character.Nameday,

            ActiveClassJob = character.ActiveClassJob,
            ActiveClassJobLevel = character.ActiveClassJobLevel,
            ActiveClassJobIcon = character.ActiveClassJobIcon,

            GrandCompany = character.GrandCompany,
            GrandCompanyRank = character.GrandCompanyRank,

            FreeCompany = character.FreeCompany?.ToDto(),

            GuardianDeityIcon = character.GuardianDeityIcon,
            GuardianDeityName = character.GuardianDeityName,

            PvpTeam = character.PvpTeam,

            Race = character.Race,
            Tribe = character.Tribe,
            Gender = character.Gender,

            TownName = character.TownName,
            TownIcon = character.TownIcon,

            Gear = character.Gear.Select(x => x.ToDto()).ToList(),

            Attributes = character.Attributes.ToDictionary(character.ActiveClassJob),

            LastUpdated = character.CharacterUpdatedAt
        };
    }
}