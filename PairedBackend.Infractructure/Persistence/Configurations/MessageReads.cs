using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PairedBackend.Domain.Entities;

namespace PairedBackend.Infractructure.Persistence.Configurations;

public class MessageReads : IEntityTypeConfiguration<MessageRead>
{
    public void Configure(EntityTypeBuilder<MessageRead> builder)
    {
        builder.HasKey(mr => new { mr.MessageId, mr.UserId });

        builder.Property(mr => mr.UserId)
            .IsRequired();
        builder.Property(mr => mr.MessageId)
            .IsRequired();

        builder.Property(mr => mr.ReadAt)
            .IsRequired();
    }
}
