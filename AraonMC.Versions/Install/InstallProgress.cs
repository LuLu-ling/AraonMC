namespace AraonMC.Versions.Install;

public enum InstallPhase
{
    ClientJson,
    ClientJar,
    Libraries,
    Assets,
    Natives,
}

/// <summary>安装进度：当前阶段、已完成/总数、当前文件名。</summary>
public sealed record InstallProgress(InstallPhase Phase, int Done, int Total, string? CurrentFile);
