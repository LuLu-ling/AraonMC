namespace AraonMC.Versions.Install;

public sealed class InstallException : Exception
{
    public Uri? Url { get; }
    public string? TargetPath { get; }

    public InstallException(string message, Uri? url = null, string? targetPath = null, Exception? inner = null)
        : base(message, inner)
    {
        Url = url;
        TargetPath = targetPath;
    }
}
