using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetStone.Cache.Db.Models;

namespace NetStone.Cache.Db.Mapping;

public class FreeCompanyMemberMapping : IEntityTypeConfiguration<FreeCompanyMember>
{
    public void Configure(EntityTypeBuilder<FreeCompanyMember> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.CharacterLodestoneId, x.FreeCompanyLodestoneId }).IsUnique();

        builder.Property(x => x.CharacterLodestoneId).HasMaxLength(10);
        builder.Property(x => x.FreeCompanyLodestoneId).HasMaxLength(31);

        builder.HasOne(x => x.FullCharacter)
            .WithOne(y => y.FreeCompanyMembership)
            .HasForeignKey<FreeCompanyMember>(x => x.FullCharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FreeCompany)
            .WithMany(y => y.Members)
            .HasForeignKey(x => x.FreeCompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name).HasMaxLength(21);
        builder.Property(x => x.Rank).HasMaxLength(31);
        builder.Property(x => x.RankIcon).HasMaxLength(255);
        builder.Property(x => x.Server).HasMaxLength(31);
        builder.Property(x => x.DataCenter).HasMaxLength(31);
        builder.Property(x => x.Avatar).HasMaxLength(255);
    }
}