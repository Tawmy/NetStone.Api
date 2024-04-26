using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterFreeCompanyMapping : IEntityTypeConfiguration<CharacterFreeCompany>
{
    public void Configure(EntityTypeBuilder<CharacterFreeCompany> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Character)
            .WithOne(y => y.FreeCompany)
            .HasForeignKey<CharacterFreeCompany>(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.LodestoneId).HasMaxLength(31);

        builder.Property(x => x.Name).HasMaxLength(31);
        builder.Property(x => x.Link).HasMaxLength(255);

        builder.Property(x => x.TopLayer).HasMaxLength(255);
        builder.Property(x => x.MiddleLayer).HasMaxLength(255);
        builder.Property(x => x.BottomLayer).HasMaxLength(255);
    }
}