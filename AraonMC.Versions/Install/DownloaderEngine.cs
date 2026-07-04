using Downloader;

namespace AraonMC.Versions.Install;

/// <summary>基于 Downloader 库的传输引擎：大文件分块并行，小文件单块。</summary>
public sealed class DownloaderEngine : IDownloadEngine
{
    public async Task DownloadAsync(Uri url, string targetPath, IProgress<EngineProgress>? progress, CancellationToken ct)
    {
        var config = new DownloadConfiguration
        {
            ChunkCount = 8,
            ParallelDownload = true,
            MaxTryAgainOnFailover = 3,
            MinimumSizeOfChunking = 1024 * 1024, // <1MB 不分块
        };

        var ds = new DownloadService(config);
        ds.DownloadProgressChanged += (_, e) =>
        {
            var received = (long)(e.ProgressPercentage / 100.0 * e.TotalBytesToReceive);
            progress?.Report(new EngineProgress(received, e.TotalBytesToReceive, e.BytesPerSecondSpeed));
        };

        using var reg = ct.Register(() => ds.CancelAsync());
        try
        {
            await ds.DownloadFileTaskAsync(url.OriginalString, targetPath).ConfigureAwait(false);
        }
        catch (Exception ex) when (ct.IsCancellationRequested)
        {
            throw new OperationCanceledException(ex.Message, ex, ct);
        }
    }
}
