using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PairedBackend.Domain.Entities;
using PairedBackend.Domain.Enums;

namespace PairedBackend.Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SenderId).IsRequired();
        builder.Property(x => x.ChatId).IsRequired();
        builder.Property(x => x.SentDate).IsRequired();
        
        builder.HasMany(x => x.Reads)
               .WithOne()
               .HasForeignKey(x => x.MessageId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(x => x.Value, vo =>
        {
            vo.Property(x => x.Value)
              .HasColumnName("Value")
              .IsRequired();

            vo.Property(x => x.ContainsLink)
              .HasColumnName("ContainsLink");

            vo.Property(x => x.MusicPlatform)
            .HasColumnName("MusicPlatform")
            .HasConversion(
                v => v.ToString(),
                v => (MusicPlatform)Enum.Parse(typeof(MusicPlatform), v)
                );
        });
    }
}
