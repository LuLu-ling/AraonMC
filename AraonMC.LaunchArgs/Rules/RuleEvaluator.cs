using System.Text.RegularExpressions;

namespace AraonMC.LaunchArgs.Rules;

/// <summary>评估一组规则在当前平台下是否成立。</summary>
public sealed class RuleEvaluator
{
    private readonly PlatformContext _platform;

    public RuleEvaluator(PlatformContext? platform = null) => _platform = platform ?? PlatformContext.Current;

    /// <summary>规则为空则恒包含；否则按顺序，命中的 Allow/Disallow 决定结果。</summary>
    public bool Applies(IReadOnlyList<Rule>? rules, IReadOnlySet<string> activeFeatures)
    {
        if (rules is null || rules.Count == 0) return true;

        var result = false;
        foreach (var rule in rules)
        {
            if (!MatchesConditions(rule, activeFeatures)) continue;
            result = rule.Action == RuleAction.Allow;
        }
        return result;
    }

    private bool MatchesConditions(Rule rule, IReadOnlySet<string> activeFeatures)
    {
        if (rule.Os is { } os && !MatchesOs(os)) return false;

        if (rule.Features is { } features)
        {
            foreach (var (key, required) in features)
                if (required && !activeFeatures.Contains(key)) return false;
        }

        return true;
    }

    private bool MatchesOs(OsCondition os)
    {
        if (os.Name is { } name && name != _platform.OperatingSystem) return false;

        if (!string.IsNullOrEmpty(os.Version))
        {
            try
            {
                if (!Regex.IsMatch(_platform.Version, os.Version)) return false;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        if (!string.IsNullOrEmpty(os.Arch) &&
            !string.Equals(os.Arch, _platform.Arch, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}
