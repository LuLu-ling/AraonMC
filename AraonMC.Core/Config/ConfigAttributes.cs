using System;

namespace AraonMC.Core.Config;

/// <summary>Marks the <c>Config</c> partial class as the root config catalog that the
/// source generator turns into a typed, observable facade.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ConfigCatalogAttribute : Attribute;

/// <summary>Marks a nested partial class of <c>Config</c> as a config section.</summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class SectionAttribute : Attribute
{
    /// <summary>Which scope the keys in this section belong to.</summary>
    public ConfigScope Scope { get; set; } = ConfigScope.Global;

    /// <summary>
    /// TOML table path for this section (e.g. <c>general</c>, <c>java</c>).
    /// Each key's full path is <c>{Section.Path}.{key}</c>.
    /// </summary>
    public string Path { get; set; } = "";
}

/// <summary>Marks a partial property as a config key.</summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class KeyAttribute : Attribute
{
    /// <summary>Override the TOML key name (defaults to snake_case of the property name).</summary>
    public string? Name { get; set; }

    /// <summary>Default value used when the key is absent or fails to parse. Must be a constant.</summary>
    public object? Default { get; set; }
}
