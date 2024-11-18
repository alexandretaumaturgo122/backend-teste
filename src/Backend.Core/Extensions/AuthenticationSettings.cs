namespace Backend.Core.Extensions;

public class AuthenticationSettings
{
    public int ExpirationTime { get; set; } = 2;
    public string Secret { get; set; } = string.Empty;
}