using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterAttributesMapping : IEntityTypeConfiguration<CharacterAttributes>
{
    public void Configure(EntityTypeBuilder<CharacterAttributes> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Character)
            .WithOne(y => y.Attributes)
            .HasForeignKey<CharacterAttributes>(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.MpGpCpParameterName).HasMaxLength(2);
    }
}