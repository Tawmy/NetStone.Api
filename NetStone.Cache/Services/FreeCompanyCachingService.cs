using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Interfaces;
using NetStone.Common.DTOs.FreeCompany;
using NetStone.Model.Parseables.FreeCompany;

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

            // TODO save related entities
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
}