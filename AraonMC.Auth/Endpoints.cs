namespace AraonMC.Auth;

internal static class Endpoints
{
    public const string DeviceCode = "https://login.microsoftonline.com/consumers/oauth2/v2.0/devicecode";

    public const string OAuthToken = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";

    public const string RefreshToken = "https://login.live.com/oauth20_token.srf";

    public const string XboxLiveAuthenticate = "https://user.auth.xboxlive.com/user/authenticate";

    public const string XstsAuthorize = "https://xsts.auth.xboxlive.com/xsts/authorize";

    public const string MinecraftLoginWithXbox =
        "https://api.minecraftservices.com/authentication/login_with_xbox";

    public const string Entitlements = "https://api.minecraftservices.com/entitlements/mcstore";

    public const string Profile = "https://api.minecraftservices.com/minecraft/profile";

    public const string Scope = "XboxLive.signin offline_access";

    public const string Tenant = "/consumers";
}
