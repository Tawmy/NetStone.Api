using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterMountMapping : IEntityTypeConfiguration<CharacterMount>
{
    public void Configure(EntityTypeBuilder<CharacterMount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CharacterLodestoneId, x.Name }).IsUnique();
        builder.Property(x => x.CharacterLodestoneId).HasMaxLength(10);
        builder.Property(x => x.Name).HasMaxLength(63);

        builder.HasOne(x => x.Character)
            .WithMany(y => y.Mounts)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}