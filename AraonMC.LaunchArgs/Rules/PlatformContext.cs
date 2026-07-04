using System.Runtime.InteropServices;

namespace AraonMC.LaunchArgs.Rules;

/// <summary>当前运行平台快照，用于判定 OS 条件。可显式构造用于测试。</summary>
public sealed class PlatformContext
{
    public OperatingSystemKind OperatingSystem { get; init; }
    public string Version { get; init; } = string.Empty;
    public string Arch { get; init; } = string.Empty;

    public static PlatformContext Current { get; } = Detect();

    private static PlatformContext Detect()
    {
        var os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OperatingSystemKind.Windows
               : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? OperatingSystemKind.OSX
               : OperatingSystemKind.Linux;

        var arch = RuntimeInformation.OSArchitecture switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "x64",
            Architecture.Arm => "arm",
            Architecture.Arm64 => "arm64",
            _ => RuntimeInformation.OSArchitecture.ToString().ToLowerInvariant(),
        };

        var ver = os == OperatingSystemKind.Windows
            ? Environment.OSVersion.Version.ToString()
            : "0"; // TODO: macOS / Linux 真实版本探测

        return new PlatformContext { OperatingSystem = os, Arch = arch, Version = ver };
    }
}
