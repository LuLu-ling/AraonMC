namespace AraonMC.Core.Config;

/// <summary>
/// The application port behind the source-generated <c>Config</c> facade. Reads and writes
/// individual scalar/enum key values, optionally scoped to a specific instance path.
/// </summary>
/// <remarks>
/// The concrete file-backed implementation lives in the presentation layer; tests inject an
/// in-memory implementation. <see cref="Config"/> (source-generated) delegates every accessor
/// to the single store installed via <c>Config.Initialize</c> at startup.
/// </remarks>
public interface IConfigStore
{
    /// <summary>Reads the value at <paramref name="path"/>, or returns <paramref name="defaultValue"/>.</summary>
    T Get<T>(ConfigScope scope, string path, T defaultValue, string? instancePath = null);

    /// <summary>Writes <paramref name="value"/> to <paramref name="path"/> (write-through, atomic).</summary>
    void Set<T>(ConfigScope scope, string path, T value, string? instancePath = null);
}
