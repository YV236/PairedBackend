namespace PairedBackend.Contracts.User;

public class RegisterUserRequest
{
    public required string Email { get; set; }

    public required string UserName { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}
