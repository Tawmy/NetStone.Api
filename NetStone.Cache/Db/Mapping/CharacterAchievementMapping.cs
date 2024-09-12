using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterAchievementMapping : IEntityTypeConfiguration<CharacterAchievement>
{
    public void Configure(EntityTypeBuilder<CharacterAchievement> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CharacterLodestoneId, x.AchievementId }).IsUnique();
        builder.Property(x => x.CharacterLodestoneId).HasMaxLength(10);
        builder.Property(x => x.Name).HasMaxLength(63);

        builder.HasOne(x => x.Character)
            .WithMany(y => y.Achievements)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}