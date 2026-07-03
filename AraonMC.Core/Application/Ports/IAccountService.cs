using System.Collections.ObjectModel;
using AraonMC.Core.Domain.Entities;

namespace AraonMC.Core.Application.Ports;

/// <summary>Account login / identity management (application port).</summary>
public interface IAccountService
{
    /// <summary>
    /// Live, service-owned account list — bind directly; both the Accounts page and the top-bar
    /// switcher share this instance.
    /// </summary>
    ObservableCollection<MinecraftAccount> Accounts { get; }

    MinecraftAccount? GetActive();

    Task<MinecraftAccount> LoginMicrosoftAsync(CancellationToken ct = default);

    Task<MinecraftAccount> AddOfflineAsync(string username, CancellationToken ct = default);

    Task SetActiveAsync(MinecraftAccount account, CancellationToken ct = default);

    Task RemoveAsync(MinecraftAccount account, CancellationToken ct = default);

    /// <summary>A live Minecraft access token for launching, or null if re-authentication is needed.</summary>
    Task<string?> GetAccessTokenAsync(MinecraftAccount account, CancellationToken ct = default);
}
