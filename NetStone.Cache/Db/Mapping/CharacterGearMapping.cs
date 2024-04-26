using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterGearMapping : IEntityTypeConfiguration<CharacterGear>
{
    public void Configure(EntityTypeBuilder<CharacterGear> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CharacterId, x.Slot }).IsUnique();

        builder.HasOne(x => x.Character)
            .WithMany(y => y.Gear)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.ItemName).HasMaxLength(63);
        builder.Property(x => x.ItemDatabaseLink).HasMaxLength(255);
        builder.Property(x => x.StrippedItemName).HasMaxLength(63);
        builder.Property(x => x.GlamourDatabaseLink).HasMaxLength(255);
        builder.Property(x => x.GlamourName).HasMaxLength(63);
        builder.Property(x => x.CreatorName).HasMaxLength(21);
        builder.Property(x => x.Materia1).HasMaxLength(31);
        builder.Property(x => x.Materia2).HasMaxLength(31);
        builder.Property(x => x.Materia3).HasMaxLength(31);
        builder.Property(x => x.Materia4).HasMaxLength(31);
        builder.Property(x => x.Materia5).HasMaxLength(31);
    }
}