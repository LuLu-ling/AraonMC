using System.Security.Cryptography;
using System.Text;
using AraonMC.Versions.Install;
using Xunit;

namespace AraonMC.Versions.Tests;

public class FileDownloaderTests
{
    [Fact]
    public async Task Downloads_WhenFileMissing()
    {
        var content = Encoding.UTF8.GetBytes("hello world");
        var engine = new FakeEngine(content);
        var dl = new FileDownloader(engine);
        var path = TempPath();

        await dl.DownloadAllAsync(
            [new DownloadRequest { Url = new("https://x/a"), TargetPath = path, Sha1 = Sha1Hex(content) }],
            InstallPhase.Libraries, null);

        Assert.Equal(1, engine.CallCount);
        Assert.Equal("hello world", await File.ReadAllTextAsync(path));
    }

    [Fact]
    public async Task Skips_WhenFileExistsWithMatchingSha1()
    {
        var content = Encoding.UTF8.GetBytes("hello world");
        var engine = new FakeEngine(content);
        var dl = new FileDownloader(engine);
        var path = TempPath();
        await File.WriteAllTextAsync(path, "hello world");

        await dl.DownloadAllAsync(
            [new DownloadRequest { Url = new("https://x/a"), TargetPath = path, Sha1 = Sha1Hex(content) }],
            InstallPhase.Libraries, null);

        Assert.Equal(0, engine.CallCount);  // 未发起传输
    }

    [Fact]
    public async Task Redownloads_WhenSha1Mismatches()
    {
        var content = Encoding.UTF8.GetBytes("new content");
        var engine = new FakeEngine(content);
        var dl = new FileDownloader(engine);
        var path = TempPath();
        await File.WriteAllTextAsync(path, "stale content");

        await dl.DownloadAllAsync(
            [new DownloadRequest { Url = new("https://x/a"), TargetPath = path, Sha1 = Sha1Hex(content) }],
            InstallPhase.Libraries, null);

        Assert.Equal(1, engine.CallCount);
        Assert.Equal("new content", await File.ReadAllTextAsync(path));
    }

    private static string TempPath() => Path.Combine(Path.GetTempPath(), "araonmc-test-" + Guid.NewGuid().ToString("N") + ".bin");

    private static string Sha1Hex(byte[] data)
    {
        using var sha = SHA1.Create();
        return Convert.ToHexString(sha.ComputeHash(data));
    }

    /// <summary>绕过网络的假引擎：直接把固定字节写入目标路径。</summary>
    internal sealed class FakeEngine : IDownloadEngine
    {
        public int CallCount;
        private readonly byte[] _content;
        public FakeEngine(byte[] content) => _content = content;

        public async Task DownloadAsync(Uri url, string targetPath, IProgress<EngineProgress>? progress, CancellationToken ct)
        {
            Interlocked.Increment(ref CallCount);
            await File.WriteAllBytesAsync(targetPath, _content, ct);
            progress?.Report(new EngineProgress(_content.Length, _content.Length, 0));
        }
    }
}
