using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db.Models;
using NetStone.StaticData;
using Npgsql;

namespace NetStone.Cache.Db;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
        MapEnums();
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        MapEnums();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var conn = Environment.GetEnvironmentVariable(EnvironmentVariables.ConnectionString);
            optionsBuilder.UseNpgsql(conn).UseSnakeCaseNamingConvention();
            optionsBuilder.UseExceptionProcessor();
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly); // load models from assembly
        modelBuilder.UseIdentityAlwaysColumns(); // always generate identity column, do not allow user values

        modelBuilder.HasPostgresEnum<ClassJob>();
        modelBuilder.HasPostgresEnum<GrandCompany>();

        base.OnModelCreating(modelBuilder);
    }

    private void MapEnums()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ClassJob>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<GrandCompany>();
    }

    #region DbSets

    internal DbSet<Character> Characters => Set<Character>();
    internal DbSet<CharacterFreeCompany> CharacterFreeCompanies => Set<CharacterFreeCompany>();
    internal DbSet<CharacterGear> CharacterGears => Set<CharacterGear>();

    #endregion
}