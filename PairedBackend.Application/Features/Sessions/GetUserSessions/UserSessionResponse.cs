namespace PairedBackend.Application.Features.Sessions.GetUserSessions;

public record UserSessionResponse(
    Guid Id,
    string Device,
    string OS,
    string Client,
    string IpAddress,
    string Location,
    DateTime CreatedAt,
    DateTime LastActiveAt
);