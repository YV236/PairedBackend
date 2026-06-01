using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PairedBackend.Domain.Entities;
using PairedBackend.Infrastructure.Identity;
using System.Reflection;

namespace PairedBackend.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Friendship> Friendships => Set<Friendship>();

    public DbSet<UserSession> UserSessions => Set<UserSession>();

    public DbSet<Chat> Chats => Set<Chat>();

    public DbSet<ChatParticipant> ChatParticipants => Set<ChatParticipant>();

    public DbSet<Message> Messages => Set<Message>();

    public DbSet<MessageRead> MessageReads => Set<MessageRead>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}
