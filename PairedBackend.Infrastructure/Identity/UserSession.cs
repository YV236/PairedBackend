namespace PairedBackend.Infrastructure.Identity;

public class UserSession
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

    public string Device { get; set; } = string.Empty;
    public string OS { get; set; } = string.Empty;
    public string Client { get; set; } = string.Empty;

    public string IPAddress { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime LastActiveAt { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public bool IsRevoked { get; set; }
}
