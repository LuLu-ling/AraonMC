namespace AraonMC.Core.Domain.Enums;

/// <summary>How an account authenticates; determines the secret material in <see cref="Domain.Entities.StoredAccount"/>.</summary>
public enum AccountType
{
    /// <summary>Microsoft online ("正版验证"); six-step device-code flow, backed by an OAuth refresh token.</summary>
    Microsoft,

    /// <summary>authlib-injector / Yggdrasil ("第三方认证", e.g. LittleSkin). Not yet implemented.</summary>
    ThirdParty,

    /// <summary>Offline ("离线认证"); UUID derived from the username, no token.</summary>
    Offline
}
