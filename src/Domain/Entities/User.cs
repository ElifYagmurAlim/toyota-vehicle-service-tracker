using VehicleServiceTracker.Domain.Common;

namespace VehicleServiceTracker.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string? FullName { get; private set; }

    private User() { }

    public User(string username, string passwordHash, string? fullName = null)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Kullanıcı adı zorunludur.", nameof(username));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Şifre zorunludur.", nameof(passwordHash));

        Username = username.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        FullName = fullName?.Trim();
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Şifre boş olamaz.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}