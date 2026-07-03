namespace AraonMC.Core.Domain.Entities;

/// <summary>
/// A real, on-disk Minecraft installation located at <see cref="Path"/>. This is the identity
/// used by the config system — per-instance settings are keyed by this absolute path. It will
/// grow to carry more launch-relevant state. Distinct from <see cref="GameInstance"/>, which is
/// the frontend display DTO.
/// </summary>
public sealed class MinecraftInstance
{
    /// <summary>Absolute filesystem path of the instance's <c>.minecraft</c> directory.</summary>
    public string Path { get; set; } = string.Empty;
}
