using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db.Models;
using NetStone.Common.Enums;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace NetStone.Cache.Db;

public class DatabaseContext : DbContext, IDataProtectionKeyContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var conn = Environment.GetEnvironmentVariable(EnvironmentVariables.ConnectionString);
            optionsBuilder.UseNpgsql(conn, MapEnums).UseSnakeCaseNamingConvention();
            optionsBuilder.UseExceptionProcessor();
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly); // load models from assembly
        modelBuilder.UseIdentityAlwaysColumns(); // always generate identity column, do not allow user values

        base.OnModelCreating(modelBuilder);
    }

    private static void MapEnums(NpgsqlDbContextOptionsBuilder builder)
    {
        builder.MapEnum<ClassJob>();
        builder.MapEnum<GrandCompany>();
        builder.MapEnum<GearSlot>();
        builder.MapEnum<Race>();
        builder.MapEnum<Tribe>();
        builder.MapEnum<Gender>();
    }

    #region DbSets

    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

    internal DbSet<Character> Characters => Set<Character>();
    internal DbSet<CharacterFreeCompany> CharacterFreeCompanies => Set<CharacterFreeCompany>();
    internal DbSet<CharacterGear> CharacterGears => Set<CharacterGear>();
    internal DbSet<CharacterAttributes> CharacterAttributes => Set<CharacterAttributes>();
    internal DbSet<CharacterClassJob> CharacterClassJobs => Set<CharacterClassJob>();
    internal DbSet<CharacterMinion> CharacterMinions => Set<CharacterMinion>();
    internal DbSet<CharacterMount> CharacterMounts => Set<CharacterMount>();
    internal DbSet<CharacterAchievement> CharacterAchievements => Set<CharacterAchievement>();

    internal DbSet<FreeCompany> FreeCompanies => Set<FreeCompany>();
    internal DbSet<FreeCompanyMember> FreeCompanyMembers => Set<FreeCompanyMember>();

    #endregion
}