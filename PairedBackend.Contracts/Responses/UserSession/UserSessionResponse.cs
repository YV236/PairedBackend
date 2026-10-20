namespace PairedBackend.Contracts.Responses.UserSession;

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
