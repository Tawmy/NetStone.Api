using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;
using NetStone.Model.Parseables.FreeCompany.Members;

namespace NetStone.Cache.Services;

public class FreeCompanyCachingService(DatabaseContext context, IMapper mapper) : IFreeCompanyCachingService
{
    public async Task<FreeCompanyDto> CacheFreeCompanyAsync(LodestoneFreeCompany lodestoneFreeCompany)
    {
        var freeCompany = await context.FreeCompanies
            .SingleOrDefaultAsync(x => x.LodestoneId == lodestoneFreeCompany.Id);

        await using var transaction = await context.Database.BeginTransactionAsync();

        if (freeCompany != null)
        {
            mapper.Map(lodestoneFreeCompany, freeCompany);
            context.Entry(freeCompany).State = EntityState.Modified;
        }
        else
        {
            freeCompany = mapper.Map<FreeCompany>(lodestoneFreeCompany);
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
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }

        return mapper.Map<FreeCompanyDto>(freeCompany);
    }

    public async Task<FreeCompanyDto?> GetFreeCompanyAsync(int id)
    {
        var freeCompany = await context.FreeCompanies.SingleOrDefaultAsync(x => x.Id == id);
        return freeCompany != null ? mapper.Map<FreeCompanyDto>(freeCompany) : null;
    }

    public async Task<FreeCompanyDto?> GetFreeCompanyAsync(string lodestoneId)
    {
        var freeCompany = await context.FreeCompanies.SingleOrDefaultAsync(x => x.LodestoneId == lodestoneId);
        return freeCompany != null ? mapper.Map<FreeCompanyDto>(freeCompany) : null;
    }

    public async Task<ICollection<FreeCompanyMemberDto>> CacheFreeCompanyMembersAsync(string fcLodestoneId,
        ICollection<FreeCompanyMembersEntry> members)
    {
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

        List<FreeCompanyMember> newDbMembers = [];
        foreach (var newMember in newMembers)
        {
            var newDbMember = mapper.Map<FreeCompanyMember>(newMember);

            newDbMember.FreeCompanyLodestoneId = fcLodestoneId;

            if (freeCompany is not null)
            {
                newDbMember.FreeCompanyId = freeCompany.Id;
            }

            if (await context.Characters.FirstOrDefaultAsync(x => x.LodestoneId == newMember.Id) is { } character)
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
            mapper.Map(updatedMember, updatedDbMember);

            if (freeCompany is not null && updatedDbMember.FreeCompanyId is null)
            {
                updatedDbMember.FreeCompanyId = freeCompany.Id;
            }

            if (await context.Characters.FirstOrDefaultAsync(x => x.LodestoneId == updatedMember.Id) is { } character &&
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
        return allDbMembers.Select(mapper.Map<FreeCompanyMemberDto>).ToList();
    }

    public async Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)>
        GetFreeCompanyMembersAsync(int id)
    {
        var freeCompany = await context.FreeCompanies.Where(x =>
                x.Id == id)
            .Include(x => x.Members)
            .ThenInclude(x => x.FullCharacter)
            .FirstOrDefaultAsync();

        if (freeCompany is null)
        {
            return (new List<FreeCompanyMemberDto>(), null);
        }

        var memberDtos = freeCompany.Members.Select(mapper.Map<FreeCompanyMemberDto>);
        return (memberDtos.ToList(), freeCompany.FreeCompanyMembersUpdatedAt);
    }

    public async Task<(ICollection<FreeCompanyMemberDto> members, DateTime? lastUpdated)> GetFreeCompanyMembersAsync(
        string lodestoneId)
    {
        var members = await context.FreeCompanyMembers.Where(x =>
                x.FreeCompanyLodestoneId == lodestoneId)
            .Include(x => x.FullCharacter)
            .ToListAsync();

        var freeCompanyUpdatedAt = await context.FreeCompanies.Where(x =>
                x.LodestoneId == lodestoneId)
            .Select(x => x.FreeCompanyMembersUpdatedAt)
            .FirstOrDefaultAsync();

        var memberDtos = members.Select(mapper.Map<FreeCompanyMemberDto>);
        return (memberDtos.ToList(), freeCompanyUpdatedAt);
    }
}