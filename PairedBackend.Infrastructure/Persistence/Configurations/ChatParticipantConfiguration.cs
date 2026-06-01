using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PairedBackend.Domain.Entities;

namespace PairedBackend.Infractructure.Persistence.Configurations;

internal class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.HasKey(cp => new { cp.ChatId, cp.UserId });

        builder.Property(cp => cp.ChatId).IsRequired();
        builder.Property(cp => cp.UserId).IsRequired();
        
        builder.Property(cp => cp.IsBlocked)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
