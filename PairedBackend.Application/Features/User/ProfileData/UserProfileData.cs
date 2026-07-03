
using PairedBackend.Domain.Enums;

namespace PairedBackend.Application.Features.User.ProfileData;

public record UserProfileData
(
    string UserName,
    string Email,
    string PhoneNumber,
    bool TwoFactorEnabled,
    string FirstName,
    string LastName,
    List<MusicPlatform> UsedPlatforms
);
