using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using NetStone.Cache.Db.Models;
using NetStone.Cache.Interfaces;
using NetStone.Common.Enums;
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

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)" />
    /// <summary>
    ///     Saves all changes made in this context to the database.<br /><br />
    ///     First checks whether entity implements <see cref="IUpdatable" />.
    ///     If so, CreatedAt and UpdatedAt are set depending on whether the database entry is being created or modified.<br />
    ///     <br />
    ///     Overrides <see cref="DbContext.SaveChangesAsync(CancellationToken)" />.<br />
    ///     Overridden method is called once IUpdatable check is done.
    /// </summary>
    /// <remarks>
    ///     Always use this override instead of calling <see cref="DbContext.SaveChangesAsync(CancellationToken)" /> directly!
    /// </remarks>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now.ToUniversalTime();
        var entries = ChangeTracker.Entries().Where(x => x.Entity is IUpdatable).ToList();

        foreach (var createdEntry in entries.Where(x => x.State == EntityState.Added))
        {
            ((IUpdatable)createdEntry.Entity).CreatedAt = now;
        }

        foreach (var updatedEntry in entries.Where(x => x.State == EntityState.Modified))
        {
            ((IUpdatable)updatedEntry.Entity).UpdatedAt = now;
        }

        return base.SaveChangesAsync(cancellationToken);
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
        modelBuilder.HasPostgresEnum<ClassJob>();

        base.OnModelCreating(modelBuilder);
    }

    private void MapEnums()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ClassJob>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<GrandCompany>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<ClassJob>();
    }

    #region DbSets

    internal DbSet<Character> Characters => Set<Character>();
    internal DbSet<CharacterFreeCompany> CharacterFreeCompanies => Set<CharacterFreeCompany>();
    internal DbSet<CharacterGear> CharacterGears => Set<CharacterGear>();
    internal DbSet<CharacterAttributes> CharacterAttributes => Set<CharacterAttributes>();
    internal DbSet<CharacterClassJob> CharacterClassJobs => Set<CharacterClassJob>();

    #endregion
}