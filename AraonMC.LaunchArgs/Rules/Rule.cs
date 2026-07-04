namespace AraonMC.LaunchArgs.Rules;

public enum OperatingSystemKind
{
    Windows,
    Linux,
    OSX,
}

public enum RuleAction
{
    Allow,
    Disallow,
}

/// <summary>client.json 的条件规则，参数条目和库的平台过滤共用。</summary>
public sealed class Rule
{
    public RuleAction Action { get; set; }

    public OsCondition? Os { get; set; }

    public IReadOnlyDictionary<string, bool>? Features { get; set; }
}

public sealed class OsCondition
{
    public OperatingSystemKind? Name { get; set; }
    public string? Version { get; set; }
    public string? Arch { get; set; }
}
