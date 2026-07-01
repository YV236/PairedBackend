
namespace PairedBackend.Contracts.User;

public class LoginUserRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Device { get; set; } = string.Empty;

    public string IpAddress { get; set; } = string.Empty;
}
