using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Extensions.Mapping;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.Services;

public class FreeCompanyCachingService(DatabaseContext context) : IFreeCompanyCachingService
{
    private static readonly ActivitySource ActivitySource = new(nameof(IFreeCompanyCachingService));

    public async Task<FreeCompanyDto> CacheFreeCompanyAsync(LodestoneFreeCompany lodestoneFreeCompany)
    {
        using var activity = ActivitySource.StartActivity();

        var freeCompany = await context.FreeCompanies
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneFreeCompany.Id);

        await using var transaction = await context.Database.BeginTransactionAsync();

        if (freeCompany is not null)
        {
            lodestoneFreeCompany.ToDb(freeCompany);
            context.Entry(freeCompany).State = EntityState.Modified;
        }
        else
        {
            freeCompany = lodestoneFreeCompany.ToDb();
            await context.FreeCompanies.AddAsync(freeCompany);
            await context.SaveChangesAsync();

            var members = await context.FreeCompanyMembers.Where(x =>
                    x.FreeCompanyLodestoneId == lodestoneFreeCompany.Id)
                .ToListAsync();
            members.ForEach(x => x.FreeCompanyId = freeCompany.Id);

            var characters = await context.Characters.Where(x =>
                    x.FreeCompany != null &&
                    x.FreeCompany.LodestoneId == lodestoneFreeCompany.Id)
                .ToListAsync();
            characters.ForEach(x => x.FullFreeCompanyId = freeCompany.Id);
        }

        freeCompany.FreeCompanyUpdatedAt = DateTime.UtcNow;

        try
        {
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return freeCompany.ToDto();
    }

    public async Task<FreeCompanyDto?> GetFreeCompanyAsync(int id)
    {
        using var activity = ActivitySource.StartActivity();

        var freeCompany = await context.FreeCompanies.SingleOrDefaultAsync(x => x.Id == id);
        return freeCompany?.ToDto();
    }

    public async Task<FreeCompanyDto?> GetFreeCompanyAsync(string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        var freeCompany = await context.FreeCompanies.SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);
        return freeCompany?.ToDto();
    }

    public async Task<FreeCompanyDto?> GetFreeCompanyAsync(string name, string world)
    {
        using var activity = ActivitySource.StartActivity();

        var freeCompany = await context.FreeCompanies
            .Where(x => // case insensitive search with ILIKE
                EF.Functions.ILike(x.Name, name) &&
                EF.Functions.ILike(x.World, world))
            .SingleOrDefaultAsync();

        return freeCompany?.ToDto();
    }

    public async Task<ICollection<FreeCompanyMemberDto>> CacheFreeCompanyMembersAsync(string fcLodestoneId,
        ICollection<FreeCompanyMembersEntry> members)
    {
        using var activity = ActivitySource.StartActivity();

        var freeCompany = await context.FreeCompanies.Where(x => x.LodestoneId == fcLodestoneId).FirstOrDefaultAsync();

        var dbMembers = await context.FreeCompanyMembers.Where(x =>
                x.FreeCompanyLodestoneId == fcLodestoneId)
            .ToListAsync();

        var newMembers = members.Where(x =>
                !dbMembers.Select(y =>
                    y.CharacterLodestoneId).Contains(x.Id))
            .ToList();

        var updatedMembers = members.Where(x =>
                dbMembers.Select(y =>
                    y.CharacterLodestoneId).Contains(x.Id))
            .ToList();

        var deletedMembers = dbMembers.Where(x =>
            !members.Select(y =>
                y.Id).Contains(x.CharacterLodestoneId));

        // queue full characters here once to avoid one db call per member
        var existingFullCharacters = await context.Characters.Where(x =>
                newMembers.Select(y => y.Id).Contains(x.LodestoneId) ||
                updatedMembers.Select(y => y.Id).Contains(x.LodestoneId))
            .Include(x => x.Attributes)
            .ToListAsync();

        List<FreeCompanyMember> newDbMembers = [];
        foreach (var newMember in newMembers)
        {
            var newDbMember = newMember.ToDb(fcLodestoneId);

            if (freeCompany is not null)
            {
                newDbMember.FreeCompanyId = freeCompany.Id;
            }

            if (existingFullCharacters.FirstOrDefault(x => x.LodestoneId == newMember.Id) is { } character)
            {
                newDbMember.FullCharacterId = character.Id;
            }

            await context.FreeCompanyMembers.AddAsync(newDbMember);
            newDbMembers.Add(newDbMember);
        }

        List<FreeCompanyMember> updatedDbMembers = [];
        foreach (var updatedMember in updatedMembers)
        {
            var updatedDbMember = dbMembers.First(x => x.CharacterLodestoneId == updatedMember.Id);
            updatedMember.ToDb(updatedDbMember);

            if (freeCompany is not null && updatedDbMember.FreeCompanyId is null)
            {
                updatedDbMember.FreeCompanyId = freeCompany.Id;
            }

            if (existingFullCharacters.FirstOrDefault(x => x.LodestoneId == updatedMember.Id) is { } character &&
                updatedDbMember.FullCharacterId is null)
            {
                updatedDbMember.FullCharacterId = character.Id;
            }

            context.Entry(updatedDbMember).State = EntityState.Modified;
            updatedDbMembers.Add(updatedDbMember);
        }

        context.FreeCompanyMembers.RemoveRange(deletedMembers);

        if (freeCompany is not null)
        {
            freeCompany.FreeCompanyMembersUpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        var allDbMembers = newDbMembers.Concat(updatedDbMembers);
        return allDbMembers.Select(x => x.ToDto()).ToList();
    }

    public async Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)>
        GetFreeCompanyMembersAsync(int id)
    {
        using var activity = ActivitySource.StartActivity();

        var freeCompany = await context.FreeCompanies.Where(x =>
                x.Id == id)
            .Include(x => x.Members)
            .ThenInclude(x => x.FullCharacter)
            .ThenInclude(x => x!.Attributes /* may be null, but will be translated correctly by efcore */)
            .FirstOrDefaultAsync();

        if (freeCompany is null)
        {
            return ([], null);
        }

        var memberDtos = freeCompany.Members.Select(x => x.ToDto());
        return (memberDtos.ToList(), freeCompany.FreeCompanyMembersUpdatedAt);
    }

    public async Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(
        string lodestoneId)
    {
        using var activity = ActivitySource.StartActivity();

        var members = await context.FreeCompanyMembers.Where(x =>
                x.FreeCompanyLodestoneId == lodestoneId)
            .Include(x => x.FullCharacter)
            .ThenInclude(x => x!.Attributes /* may be null, but will be translated correctly by efcore */)
            .ToListAsync();

        var freeCompanyUpdatedAt = await context.FreeCompanies.Where(x =>
                x.LodestoneId == lodestoneId)
            .Select(x => x.FreeCompanyMembersUpdatedAt)
            .FirstOrDefaultAsync();

        var memberDtos = members.Select(x => x.ToDto());
        return (memberDtos.ToList(), freeCompanyUpdatedAt);
    }
}