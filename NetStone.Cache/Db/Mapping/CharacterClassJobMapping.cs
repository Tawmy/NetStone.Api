using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterClassJobMapping : IEntityTypeConfiguration<CharacterClassJob>
{
    public void Configure(EntityTypeBuilder<CharacterClassJob> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CharacterLodestoneId, x.ClassJob }).IsUnique();
        builder.Property(x => x.CharacterLodestoneId).HasMaxLength(10);

        builder.HasOne(x => x.Character)
            .WithMany(y => y.CharacterClassJobs)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}