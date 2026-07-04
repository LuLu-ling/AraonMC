namespace AraonMC.Versions.Install;

public enum InstallPhase
{
    ClientJson,
    ClientJar,
    Libraries,
    Assets,
    Natives,
}

/// <summary>安装进度：阶段、字节（已接收/总）、文件（已完成/总数）、速度、当前文件。</summary>
public sealed record InstallProgress(
    InstallPhase Phase,
    long ReceivedBytes,
    long TotalBytes,
    int FilesDone,
    int FilesTotal,
    double BytesPerSecond,
    string? CurrentFile);
