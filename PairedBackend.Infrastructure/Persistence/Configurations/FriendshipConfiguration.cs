using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PairedBackend.Domain.Entities;

namespace PairedBackend.Infrastructure.Persistence.Configurations;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(f => new { f.UserId, f.FriendId });

        builder.Property(f => f.UserId)
            .IsRequired();
        builder.Property(f => f.FriendId)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();
    }
}
