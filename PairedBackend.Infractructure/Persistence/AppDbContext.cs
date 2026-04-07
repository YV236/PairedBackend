using Microsoft.EntityFrameworkCore;
using PairedBackend.Domain.Entities;
using PairedBackend.Infractructure.Identity;

namespace PairedBackend.Infractructure.Persistence;

public class AppDbContext : DbContext
{
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<Friendship> Friendships { get; set; }

    DbSet<UserSession> UserSessions { get; set; }

    DbSet<Chat> Chats { get; set; }
    DbSet<ChatParticipant> ChatParticipants { get; set; }
    DbSet<Message> Messages { get; set; }
    DbSet<MessageRead> MessageReads { get; set; }
}
