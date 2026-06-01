using Microsoft.AspNetCore.Identity;
using PairedBackend.Domain.Enums;

namespace PairedBackend.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public List<MusicPlatform> UsedPlatforms { get; set; } = [];
}
