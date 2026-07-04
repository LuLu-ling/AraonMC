using System.IO.Compression;
using System.Security.Cryptography;
using AraonMC.LaunchArgs.Libraries;
using AraonMC.LaunchArgs.Rules;
using AraonMC.LaunchArgs.Version;

namespace AraonMC.Downloads;

/// <summary>
///   上游 <c>MinecraftDownloader.Core</c> 只下载每个库的 <c>artifact</c>，不下载也不处理
///   native 库（classifiers）。这里补齐 native 侧，分两段：
///   <list type="bullet">
///     <item><see cref="EnsureDownloadedAsync"/>（安装期）：把 native classifier jar 下到 <c>libraries/</c>，常驻。</item>
///     <item><see cref="ExtractTo"/>（启动期）：把已下载的 native jar 解压到调用方给的临时目录，
///       供 <c>-Djava.library.path</c> 指向。现代 MC（LWJGL 3）约定 natives 是启动期临时产物，
///       不常驻 <c>.minecraft</c>——故不再放 <c>versions/&lt;id&gt;/&lt;id&gt;-natives</c>。</item>
///   </list>
///   选库逻辑复用 <see cref="ClasspathResolver"/>，保证“装哪些/解压哪些 native”与启动用的一致。
/// </summary>
public sealed class NativeLibraryExtractor
{
    private readonly HttpClient _http;

    public NativeLibraryExtractor(HttpClient http) => _http = http;

    /// <summary>安装期：把当前平台所需的 native classifier jar 下到 <c>libraries/&lt;path&gt;</c>（已存在且校验通过则跳过）。</summary>
    public async Task EnsureDownloadedAsync(string gameDir, string versionId, CancellationToken ct = default)
    {
        var librariesRoot = Path.Combine(gameDir, "libraries");
        foreach (var (native, _) in ResolveNativeLibs(gameDir, versionId))
        {
            if (string.IsNullOrEmpty(native.Url) || string.IsNullOrEmpty(native.Path)) continue;
            await DownloadIfMissingAsync(native.Url, Path.Combine(librariesRoot, native.Path), native.Sha1, ct)
                .ConfigureAwait(false);
        }
    }

    /// <summary>启动期：把 native jar 解压到 <paramref name="targetDir"/>（先清空重建），按 extract.exclude 跳过排除项。</summary>
    public void ExtractTo(string gameDir, string versionId, string targetDir)
    {
        var librariesRoot = Path.Combine(gameDir, "libraries");

        if (Directory.Exists(targetDir)) Directory.Delete(targetDir, recursive: true);
        Directory.CreateDirectory(targetDir);

        foreach (var (native, lib) in ResolveNativeLibs(gameDir, versionId))
        {
            var jarPath = Path.Combine(librariesRoot, native.Path);
            if (!File.Exists(jarPath)) continue;
            ExtractNative(jarPath, targetDir, lib.Extract?.Exclude);
        }
    }

    // ---- helpers ----

    private static List<(VersionArtifact Native, VersionLibrary Lib)> ResolveNativeLibs(string gameDir, string versionId)
    {
        var jsonPath = Path.Combine(gameDir, "versions", versionId, versionId + ".json");
        var meta = VersionMetadataReader.Read(File.ReadAllText(jsonPath));
        var resolved = new ClasspathResolver(new RuleEvaluator()).Resolve(meta.Libraries, new HashSet<string>());

        var list = new List<(VersionArtifact, VersionLibrary)>(capacity: 8);
        foreach (var r in resolved)
            if (r.Native is { } native && !string.IsNullOrEmpty(native.Path))
                list.Add((native, r.Library));
        return list;
    }

    private async Task DownloadIfMissingAsync(string url, string dest, string expectedSha1, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(expectedSha1) && File.Exists(dest) && Sha1Matches(dest, expectedSha1))
            return;

        var dir = Path.GetDirectoryName(dest);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        var tmp = dest + ".tmp";
        try
        {
            using var resp = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();

            await using (var src = await resp.Content.ReadAsStreamAsync(ct).ConfigureAwait(false))
            await using (var dst = File.Create(tmp))
                await src.CopyToAsync(dst, ct).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(expectedSha1) && !Sha1Matches(tmp, expectedSha1))
                throw new InvalidDataException($"sha1 mismatch for native '{url}'.");

            File.Move(tmp, dest, overwrite: true);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    private static void ExtractNative(string jarPath, string nativesDir, IReadOnlyList<string>? excludes)
    {
        using var archive = ZipFile.OpenRead(jarPath);
        foreach (var entry in archive.Entries)
        {
            if (entry.FullName.EndsWith('/')) continue;
            if (excludes is not null
                && excludes.Any(e => entry.FullName.StartsWith(e, StringComparison.OrdinalIgnoreCase)))
                continue;

            var dest = Path.Combine(nativesDir, entry.FullName);
            var entryDir = Path.GetDirectoryName(dest);
            if (!string.IsNullOrEmpty(entryDir)) Directory.CreateDirectory(entryDir);
            entry.ExtractToFile(dest, overwrite: true);
        }
    }

    private static bool Sha1Matches(string path, string expected)
    {
        using var sha = SHA1.Create();
        using var stream = File.OpenRead(path);
        var hash = sha.ComputeHash(stream);
        return string.Equals(Convert.ToHexString(hash), expected, StringComparison.OrdinalIgnoreCase);
    }
}
