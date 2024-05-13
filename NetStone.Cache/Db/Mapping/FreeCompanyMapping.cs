using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class FreeCompanyMapping : IEntityTypeConfiguration<FreeCompany>
{
    public void Configure(EntityTypeBuilder<FreeCompany> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LodestoneId).HasMaxLength(31);
        builder.HasIndex(x => x.LodestoneId).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(20);
        builder.Property(x => x.Slogan).HasMaxLength(255);
        builder.Property(x => x.Tag).HasMaxLength(7);

        builder.Property(x => x.World).HasMaxLength(31);

        builder.Property(x => x.CrestTop).HasMaxLength(255);
        builder.Property(x => x.CrestMiddle).HasMaxLength(255);
        builder.Property(x => x.CrestBottom).HasMaxLength(255);

        builder.Property(x => x.Recruitment).HasMaxLength(31);
        builder.Property(x => x.ActiveState).HasMaxLength(31);

        builder.Property(x => x.EstateName).HasMaxLength(31);
        builder.Property(x => x.EstateGreeting).HasMaxLength(255);
        builder.Property(x => x.EstatePlot).HasMaxLength(63);
    }
}