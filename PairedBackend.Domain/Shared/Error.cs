namespace PairedBackend.Domain.Shared;

public record Error(string Code, string Message, ErrorType ErrorType)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    public static readonly Error Unexpected = new("Unexpected", "An unexpected error occurred.", ErrorType.Unexpected);
}

public enum ErrorType
{
    None,
    Unexpected,
    NotFound,
    ValidationError,
    Unauthorized,
    Forbidden,
    Conflict
}