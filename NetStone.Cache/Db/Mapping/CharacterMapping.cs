using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterMapping : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LodestoneId).HasMaxLength(10);
        builder.HasIndex(x => x.LodestoneId).IsUnique();

        builder.Property(x => x.ActiveClassJobIcon).HasMaxLength(127);

        builder.Property(x => x.Avatar).HasMaxLength(255);

        builder.Property(x => x.Bio).HasMaxLength(3000);

        builder.Property(x => x.GrandCompanyRank).HasMaxLength(63);

        builder.Property(x => x.GuardianDeityName).HasMaxLength(63);
        builder.Property(x => x.GuardianDeityIcon).HasMaxLength(127);

        builder.Property(x => x.Name).HasMaxLength(21);
        builder.Property(x => x.Nameday).HasMaxLength(63);

        builder.Property(x => x.Portrait).HasMaxLength(255);

        builder.Property(x => x.PvpTeam).HasMaxLength(31);

        builder.Property(x => x.RaceClanGender).HasMaxLength(31);

        builder.Property(x => x.Server).HasMaxLength(31);

        builder.Property(x => x.Title).HasMaxLength(63);

        builder.Property(x => x.TownName).HasMaxLength(31);
        builder.Property(x => x.TownIcon).HasMaxLength(255);
    }
}