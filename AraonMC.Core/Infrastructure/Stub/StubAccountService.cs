using System.Collections.ObjectModel;
using AraonMC.Core.Application.Ports;
using AraonMC.Core.Domain.Entities;
using AraonMC.Core.Domain.Enums;

namespace AraonMC.Core.Infrastructure.Stub;

/// <summary>Stub <see cref="IAccountService"/> with hardcoded mock accounts; not wired in.</summary>
public sealed class StubAccountService : IAccountService
{
    private readonly ObservableCollection<MinecraftAccount> _accounts =
    [
        new()
        {
            Uuid = "00000000-0000-1000-8000-000000000001",
            Username = "SaltWood_233",
            AccountType = AccountType.Microsoft,
            IsOnline = true,
            IsActive = true,
            AvatarKey = "S",
        },
        new()
        {
            Uuid = "00000000-0000-1000-8000-000000000002",
            Username = "Steve",
            AccountType = AccountType.Offline,
            IsOnline = false,
            IsActive = false,
            AvatarKey = "St",
        },
    ];

    public ObservableCollection<MinecraftAccount> Accounts => _accounts;

    public MinecraftAccount? GetActive() => _accounts.FirstOrDefault(a => a.IsActive);

    public Task<MinecraftAccount> LoginMicrosoftAsync(CancellationToken ct = default) =>
        throw new NotImplementedException("Microsoft login backend is not implemented in the stub.");

    public Task<MinecraftAccount> AddOfflineAsync(string username, CancellationToken ct = default) =>
        throw new NotImplementedException("Offline account creation is not implemented in the stub.");

    public Task SetActiveAsync(MinecraftAccount account, CancellationToken ct = default) =>
        throw new NotImplementedException("Account switching is not implemented in the stub.");

    public Task RemoveAsync(MinecraftAccount account, CancellationToken ct = default) =>
        throw new NotImplementedException("Account removal is not implemented in the stub.");

    public Task<string?> GetAccessTokenAsync(MinecraftAccount account, CancellationToken ct = default) =>
        throw new NotImplementedException("Token retrieval is not implemented in the stub.");
}
