using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PairedBackend.Infractructure.Identity;

namespace PairedBackend.Infractructure.Persistence.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserId)
               .IsRequired();
        builder.Property(x => x.CreatedAt)
               .IsRequired();
        builder.Property(x => x.LastActiveAt)
               .IsRequired();
        builder.Property(x => x.IsRevoked)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(x => x.Device)
               .HasMaxLength(200);
        builder.Property(x => x.OS)
               .HasMaxLength(100);
        builder.Property(x => x.Client)
               .HasMaxLength(100);
        builder.Property(x => x.IPAddress)
               .HasMaxLength(50);
        builder.Property(x => x.Location)
               .HasMaxLength(200);
        builder.Property(x => x.RefreshToken)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x=>x.RefreshToken).IsUnique();
    }
}
