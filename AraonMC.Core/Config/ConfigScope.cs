namespace AraonMC.Core.Config;

/// <summary>
/// The storage scope a config key belongs to. Determines which file the value
/// is read from / written to.
/// </summary>
public enum ConfigScope
{
    /// <summary>
    /// Shared application-wide settings, stored in the fixed OS config location
    /// (e.g. <c>%APPDATA%\AraonMC\config.toml</c>).
    /// </summary>
    Global,

    /// <summary>
    /// Per-instance overrides, keyed by the instance's absolute filesystem path
    /// (stored in <c>instances.toml</c>).
    /// </summary>
    Instance,
}
