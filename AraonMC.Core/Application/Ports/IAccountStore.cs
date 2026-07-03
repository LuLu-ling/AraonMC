using AraonMC.Core.Domain.Entities;

namespace AraonMC.Core.Application.Ports;

/// <summary>
/// Persistence port for account records — separate from the scalar TOML config (records are a
/// collection of secret-bearing rows needing atomic whole-record writes).
/// </summary>
public interface IAccountStore
{
    IReadOnlyList<StoredAccount> Load();

    void Save(IReadOnlyList<StoredAccount> accounts);
}
