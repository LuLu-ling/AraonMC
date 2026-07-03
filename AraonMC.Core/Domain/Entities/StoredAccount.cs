using AraonMC.Core.Domain.Enums;

namespace AraonMC.Core.Domain.Entities;

/// <summary>Persistence shape of an account — carries secrets, so it stays out of the UI layer.</summary>
public sealed class StoredAccount
{
    public string Uuid { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    public string Username { get; set; } = string.Empty;

    /// <summary>Microsoft OAuth refresh token (Microsoft accounts only).</summary>
    public string? RefreshToken { get; set; }

    /// <summary>Third-party auth-server base URL (ThirdParty accounts only).</summary>
    public string? ServerUrl { get; set; }

    /// <summary>Raw <c>minecraft/profile</c> JSON (Microsoft accounts only).</summary>
    public string? ProfileJson { get; set; }
}
