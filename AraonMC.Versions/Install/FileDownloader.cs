using System.Security.Cryptography;

namespace AraonMC.Versions.Install;

/// <summary>单个下载请求。</summary>
public sealed class DownloadRequest
{
    public required Uri Url { get; init; }
    public required string TargetPath { get; init; }

    /// <summary>期望的 sha1；提供时用于校验与"已存在则跳过"判定。</summary>
    public string? Sha1 { get; init; }
}

/// <summary>
/// 并发文件下载器：sha1 校验、原子写入（.tmp → move）、已存在且校验通过则跳过。
/// </summary>
public sealed class FileDownloader
{
    private readonly HttpClient _http;
    private readonly int _maxConcurrency;

    public FileDownloader(HttpClient http, int maxConcurrency = 8)
    {
        _http = http;
        _maxConcurrency = maxConcurrency;
    }

    public async Task DownloadAllAsync(
        IReadOnlyList<DownloadRequest> requests,
        InstallPhase phase,
        IProgress<InstallProgress>? progress,
        CancellationToken ct = default)
    {
        var done = 0;
        var total = requests.Count;
        await Parallel.ForEachAsync(
            requests,
            new ParallelOptions { MaxDegreeOfParallelism = _maxConcurrency, CancellationToken = ct },
            async (req, token) =>
            {
                await DownloadOneAsync(req, token).ConfigureAwait(false);
                var d = Interlocked.Increment(ref done);
                progress?.Report(new InstallProgress(phase, d, total, Path.GetFileName(req.TargetPath)));
            }).ConfigureAwait(false);
    }

    private async Task DownloadOneAsync(DownloadRequest req, CancellationToken ct)
    {
        var dir = Path.GetDirectoryName(req.TargetPath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        if (req.Sha1 is not null && File.Exists(req.TargetPath) && Sha1Matches(req.TargetPath, req.Sha1))
            return;

        var tmp = req.TargetPath + ".tmp";
        using var resp = await _http.GetAsync(req.Url, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();

        await using (var fs = File.Create(tmp))
            await resp.Content.CopyToAsync(fs, ct).ConfigureAwait(false);

        if (req.Sha1 is not null && !Sha1Matches(tmp, req.Sha1))
            throw new InstallException($"sha1 mismatch for {req.Url}", req.Url, req.TargetPath);

        File.Move(tmp, req.TargetPath, overwrite: true);
    }

    private static bool Sha1Matches(string path, string expected)
    {
        using var sha = SHA1.Create();
        using var stream = File.OpenRead(path);
        var hash = sha.ComputeHash(stream);
        return string.Equals(Convert.ToHexString(hash), expected, StringComparison.OrdinalIgnoreCase);
    }
}
