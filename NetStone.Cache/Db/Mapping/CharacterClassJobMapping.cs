using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class CharacterClassJobMapping : IEntityTypeConfiguration<CharacterClassJob>
{
    public void Configure(EntityTypeBuilder<CharacterClassJob> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Character)
            .WithMany(y => y.CharacterClassJobs)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}