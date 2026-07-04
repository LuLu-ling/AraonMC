namespace AraonMC.Versions.Install;

/// <summary>单文件传输引擎的进度。</summary>
public readonly record struct EngineProgress(long ReceivedBytes, long TotalBytes, double BytesPerSecond);

/// <summary>
/// 单文件下载传输抽象。生产实现 <see cref="DownloaderEngine"/> 用 Downloader 库（分块并行）；
/// 测试可注入 fake 绕过网络。
/// </summary>
public interface IDownloadEngine
{
    /// <summary>下载 <paramref name="url"/> 到 <paramref name="targetPath"/>（直接写入）。</summary>
    Task DownloadAsync(Uri url, string targetPath, IProgress<EngineProgress>? progress, CancellationToken ct);
}
