
using PairedBackend.Domain.Enums;

namespace PairedBackend.Contracts.Responses.User;

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

